using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BizHawk.Client.EmuHawk.tools.MovieEditor
{
	public partial class MovieEditor : ToolFormBase, IToolFormAutoConfig
	{
		[RequiredService]
		public IEmulator Emulator { get; private set; }

		private readonly Bk2MnemonicConstants Mnemonics = new Bk2MnemonicConstants();

		private const string CursorColumnName = "CursorColumn";
		private const string FrameColumnName = "FrameColumn";

		public MovieEditor()
		{
            InitializeComponent();

			MovieEditorInputRoll.QueryItemText += MovieEditor_QueryItemText;
			MovieEditorInputRoll.QueryItemBkColor += MovieEditor_QueryItemBkColor;

			SetUpColumns();
			MovieEditorInputRoll.RowCount = Global.MovieSession.Movie.InputLogLength;
		}

		private void SetUpColumns()
		{
			MovieEditorInputRoll.AllColumns.Clear();
			AddColumn(CursorColumnName, "", 18);
			AddColumn(FrameColumnName, "Frame#", 68);

			var columnNames = GenerateColumnNames();
			InputRoll.RollColumn.InputType type;
			int digits = 1;
			foreach (var kvp in columnNames)
			{
				if (Global.MovieSession.MovieControllerAdapter.Definition.FloatControls.Contains(kvp.Key))
				{
					Emulation.Common.ControllerDefinition.FloatRange range = Global.MovieSession.MovieControllerAdapter.Definition.FloatRanges
						[Global.MovieSession.MovieControllerAdapter.Definition.FloatControls.IndexOf(kvp.Key)];
					type = InputRoll.RollColumn.InputType.Float;
					digits = Math.Max(kvp.Value.Length, range.MaxDigits());
				}
				else
				{
					type = InputRoll.RollColumn.InputType.Boolean;
					digits = kvp.Value.Length;
				}
				AddColumn(kvp.Key, kvp.Value, (digits * 6) + 14, type);
			}
		}

		private Dictionary<string, string> GenerateColumnNames()
		{
			var lg = Global.MovieSession.LogGeneratorInstance();
			lg.SetSource(Global.MovieSession.MovieControllerAdapter);
			return (lg as Bk2LogEntryGenerator).Map();
		}

		private void AddColumn(string columnName, string columnText, int columnWidth, InputRoll.RollColumn.InputType columnType = InputRoll.RollColumn.InputType.Boolean)
		{
			if (MovieEditorInputRoll.AllColumns[columnName] == null)
			{
				var column = new InputRoll.RollColumn
				{
					Name = columnName,
					Text = columnText,
					Width = columnWidth,
					Type = columnType
				};

				MovieEditorInputRoll.AllColumns.Add(column);
			}
		}

		private void MovieEditor_QueryItemText(int index, InputRoll.RollColumn column, out string text, ref int offsetX, ref int offsetY)
		{
			text = "";
			var columnName = column.Name;
			if (columnName == FrameColumnName)
			{
				text = index.ToString().PadLeft(Global.MovieSession.Movie.InputLogLength.ToString().Length, '0');
			}
			else
			{
				if (index < Global.MovieSession.Movie.InputLogLength)
				{
					text = Global.MovieSession.Movie.GetInputState(index).IsPressed(columnName) ? Mnemonics[columnName].ToString() : "";
				}
			}
		}

		private void MovieEditor_QueryItemBkColor(int index, InputRoll.RollColumn column, ref Color color)
		{
			//color = Color.Blue;
		}

		public bool UpdateBefore
		{
			get { return false; }
		}

		public bool AskSaveChanges()
		{
			return true;
		}

		public void FastUpdate()
		{
			return;
		}

		public void NewUpdate(ToolFormUpdateType type)
		{
			return;
		}

		public void Restart()
		{
			return;
		}

		public void UpdateValues()
		{
			MovieEditorInputRoll.RowCount = Global.MovieSession.Movie.InputLogLength;
			MovieEditorInputRoll.ScrollToIndex(Emulator.Frame);
			MovieEditorInputRoll.Refresh();
		}

		private void MovieEditor_Load(object sender, EventArgs e)
		{
			MovieEditorInputRoll.Refresh();
		}
	}
}
