using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using BizHawk.Emulation.Common;
using BizHawk.Emulation.Cores;

namespace BizHawk.Client.AVHawk
{

	internal class NullDialogController : IDialogController
	{
		public void AddOnScreenMessage(string message, int? duration = null) => Console.WriteLine(message);
		public IReadOnlyList<string>? ShowFileMultiOpenDialog(IDialogParent dialogParent, string? filterStr, ref int filterIndex, string initDir, bool discardCWDChange = false, string? initFileName = null, bool maySelectMultiple = false, string? windowTitle = null)
		{
			Console.WriteLine("ShowFileSaveDialog. Canceling.");
			return null;
		}

		public string? ShowFileSaveDialog(IDialogParent dialogParent, bool discardCWDChange, string? fileExt, string? filterStr, string initDir, string? initFileName, bool muteOverwriteWarning)
		{
			Console.WriteLine("ShowFileSaveDialog. Canceling.");
			return null;
		}

		public void ShowMessageBox(IDialogParent? owner, string text, string? caption = null, EMsgBoxIcon? icon = null) => Console.WriteLine((caption, text));
		public bool ShowMessageBox2(IDialogParent? owner, string text, string? caption = null, EMsgBoxIcon? icon = null, bool useOKCancel = false)
		{
			Console.WriteLine((caption, text));
			return false;
		}

		public bool? ShowMessageBox3(IDialogParent? owner, string text, string? caption = null, EMsgBoxIcon? icon = null)
		{
			Console.WriteLine((caption, text));
			return null;
		}

		public void StartSound() { }

		public void StopSound() { }
	}

	internal class NullDialogParent : IDialogParent
	{
		public IDialogController DialogController { get; } = new NullDialogController();
	}

	internal class DisabledSink : IDisposable
	{
		[DllImport("kernel32.dll")]
		public static extern IntPtr GetStdHandle(int nStdHandle);

		[DllImport("kernel32.dll")]
		public static extern int SetStdHandle(int nStdHandle, IntPtr hHandle);

		private readonly TextWriter _oldOut;
		private readonly TextWriter _oldError;
		private readonly IntPtr _oldOutHandle;
		private readonly IntPtr _oldErrorHandle;

		public DisabledSink()
		{
			_oldOutHandle = GetStdHandle(-11);
			_oldErrorHandle = GetStdHandle(-12);
			_oldOut = Console.Out;
			_oldError = Console.Error;
			_ = SetStdHandle(-11, IntPtr.Zero);
			_ = SetStdHandle(-12, IntPtr.Zero);
		}

		public void Dispose()
		{
			_ = SetStdHandle(-11, _oldOutHandle);
			_ = SetStdHandle(-12, _oldErrorHandle);
		}
	}

	internal class Program
	{
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SetDllDirectoryW(string lpPathName);
		static Program()
		{
			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
			var dllDir = Path.Combine(AppContext.BaseDirectory, "dll");
			SetDllDirectoryW(dllDir);
		}

		private class Args
		{
			public string Rom { get; set; } = "";
			public string Movie { get; set; } = "";
			public string AVCommand { get; set; } = "-vf scale=iw*2:ih*2 -f webp";
			public string Output { get; set; } = "output.webp";
			public bool IsValid => !string.IsNullOrEmpty(Rom) && !string.IsNullOrEmpty(Movie) && !string.IsNullOrEmpty(AVCommand) && !string.IsNullOrEmpty(Output);
		}

		private static Args ParseArgs(string[] args)
		{
			try
			{
				var arg = new Args();
				for (int i = 0; i < args.Length; i++)
				{
					if (args[i].StartsWith("--", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
					{
						if (args[i] == "--rom") {
							arg.Rom = args[i + 1];
							if (!File.Exists(arg.Rom))
							{
								throw new Exception($"Rom not found: {Path.GetFullPath(arg.Rom)}");
							}
						}
						if (args[i] == "--movie")
						{
							arg.Movie = args[i + 1];
							if (!File.Exists(arg.Movie))
							{
								throw new Exception($"Movie not found: {Path.GetFullPath(arg.Movie)}");
							}
						}
						if (args[i] == "--avcommand") { arg.AVCommand = args[i + 1]; }
						if (args[i] == "--output") { arg.Output = args[i + 1]; }
						i++;
					}
				}
				if (!arg.IsValid)
				{
					throw new Exception("Missing arguments.");
				}
				return arg;
			}
			catch
			{
				Console.WriteLine("Error parsing arguments.");
				Console.WriteLine("Example usage:");
				Console.WriteLine(@"AVHawk.exe --rom smw.smc --movie glitch.bk2 --avcommand ""-vf scale=iw*2:ih*2 -f mp4"" --output dump.mp4");
				Console.WriteLine();
				throw;
			}
		}

		private static int Main(string[] args)
		{
			var argConfig = ParseArgs(args);

			// load core, movie, and rom
			var config = new Config()
			{
				FFmpegCustomCommand = argConfig.AVCommand,
				//FFmpegCustomCommand = "-vf scale=iw*2:ih*2 -crf 18 -sws_flags neighbor -pix_fmt yuv420p -b:a 384k -f mp4",
				//FFmpegCustomCommand = @"-filter_complex ""setpts=(N/42)/TB"" -f gif",
				FFmpegFormat = "[Custom]"
			};
			var romLoader = new RomLoader(config);
			romLoader.OnLoadSettings += RomLoader_OnLoadSettings;
			romLoader.OnLoadSyncSettings += RomLoader_OnLoadSyncSettings;

			var par = new NullDialogParent();
			var session = new MovieSession(new MovieConfig(), null, par, () => { }, () => { });
			var movie = session.Get(argConfig.Movie, loadMovie: true);
			session.QueueNewMovie(movie, movie.SystemID, movie.Hash, null, config.PreferredCores);

			var coreComm = new CoreComm((_) => { }, (_, _) => { }, (_) => true, null, CoreComm.CorePreferencesFlags.None, null);
			using (var sink = new DisabledSink()) // disable output for rom loading because it is a lot
			{
				romLoader.LoadRom(argConfig.Rom, coreComm, null);
			}
			session.RunQueuedMovie(recordMode: false, romLoader.LoadedEmulator);

			// prepare video
			var video = (romLoader.LoadedEmulator as IVideoProvider)!;
			var ffmpeg = new FFmpegWriter(par);
			var avi = new AudioStretcher(ffmpeg);
			avi.SetMovieParameters(romLoader.LoadedEmulator.VsyncNumerator(), romLoader.LoadedEmulator.VsyncDenominator());
			avi.SetVideoParameters(video.BufferWidth, video.BufferHeight);
			avi.SetAudioParameters(44100, 2, 16);
			avi.SetDefaultVideoCodecToken(config);
			var sound = romLoader.LoadedEmulator.ServiceProvider.GetService<ISoundProvider>();
			if (sound.CanProvideAsync)
			{
				sound.SetSyncMode(SyncSoundMode.Async);
			}
			else
			{
				sound.SetSyncMode(SyncSoundMode.Sync);
				sound = new SyncToAsyncProvider(romLoader.LoadedEmulator.VsyncRate, sound);
			}

			Console.WriteLine($"Start dumping {movie.FrameCount} frames to {Path.GetFullPath(argConfig.Output)}");

			// dump video
			avi.OpenFile(argConfig.Output);
			for (int i = 0; i < movie.FrameCount; i++)
			{
				session.HandleFrameBefore();
				romLoader.LoadedEmulator.FrameAdvance(session.MovieController, render: true, renderSound: true);
				session.HandleFrameAfter();
				avi.DumpAV(romLoader.LoadedEmulator as IVideoProvider, sound, out short[] samples, out int samplesProvided);
			}
			avi.CloseFile();

			Console.WriteLine("Done");
			return 0;
		}

		private static void RomLoader_OnLoadSyncSettings(object sender, RomLoader.SettingsLoadArgs e)
		{
		}

		private static void RomLoader_OnLoadSettings(object sender, RomLoader.SettingsLoadArgs e)
		{
		}


		/// <remarks>http://www.codeproject.com/Articles/310675/AppDomain-AssemblyResolve-Event-Tips</remarks>
		private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			var requested = args.Name;

			lock (AppDomain.CurrentDomain)
			{
				var firstAsm = Array.Find(AppDomain.CurrentDomain.GetAssemblies(), asm => asm.FullName == requested);
				if (firstAsm != null)
				{
					return firstAsm;
				}

				// load missing assemblies by trying to find them in the dll directory
				var dllname = $"{new AssemblyName(requested).Name}.dll";
				var directory = Path.Combine(AppContext.BaseDirectory, "dll");
				var fname = Path.Combine(directory, dllname);
				// it is important that we use LoadFile here and not load from a byte array; otherwise mixed (managed/unmanaged) assemblies can't load
				return File.Exists(fname) ? Assembly.LoadFile(fname) : null;
			}
		}
	}
}
