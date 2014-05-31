using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Interpreter
{
    public partial class IDE : Form
    {
        public RichTextBox rtbMainSourceCode { get; set; }
        private bool textIsChanged = true;
        public IDE()
        {
            InitializeComponent();
            FormClosed += OnExit;
            rtbSourceCode.TextChanged += new EventHandler(rtbMainTextChanged);
        }

        void rtbMainTextChanged(object sender, EventArgs e)
        {
            textIsChanged = true;
        }

        private void tpOutput_Click(object sender, EventArgs e)
        {

        }

        private void OnRun(object sender, EventArgs e)
        {
            lvWatches.Items.Clear();
            lvErrors.Items.Clear();
            tbOutput.Clear();
            if (textIsChanged)
            {
                tbBreakpoints.Clear();
                CallCompile(rtbSourceCode.Text, new BreakpointPosition());
                textIsChanged = false;
            }
            CallInterpreter();
            CallErrorHandler();
            if (lvErrors.Items.Count > 0)
            {
                tabControl1.SelectedIndex = 0;
            }
            else
            {
                CallPrintResult();
                tabControl1.SelectedIndex = 1;
            }
        }

        private void OnDebug(object sender, EventArgs e)
        {
            lvErrors.Items.Clear();
            if (textIsChanged)
            {
                tbBreakpoints.Clear();
                CallCompile(rtbSourceCode.Text, new BreakpointPosition());
                textIsChanged = false;
            }
            CallInterpreterStep();
            CallErrorHandler();
            if (lvErrors.Items.Count > 0)
            {
                tabControl1.SelectedIndex = 0;
            }
            else
            {
                CallPrintResult();
                tabControl1.SelectedIndex = 1;
            }
        }

        private int GetIndexInLine(int indexInText)
        {
            int indexInLine = 0;
            string line = rtbSourceCode.Lines[rtbSourceCode.GetLineFromCharIndex(rtbSourceCode.SelectionStart)];
            while (indexInText + line.Length - indexInLine > rtbSourceCode.Text.Length)
            {
                indexInLine++;
            }
            while (!line.Substring(indexInLine).Equals(rtbSourceCode.Text.Substring(indexInText, line.Length - indexInLine)))
            {
                if (line.Length >= indexInLine)
                {
                    indexInLine++;
                }
                else
                {
                    return -1;
                }
            }
            return indexInLine;
        }

        private void AddRemoveBreakpointClick(object sender, EventArgs e)
        {
            lvWatches.Items.Clear();
            lvErrors.Items.Clear();
            tbOutput.Clear();
            tbBreakpoints.Clear();
            BreakpointPosition breakpointPos = new BreakpointPosition();
            breakpointPos.row = rtbSourceCode.GetLineFromCharIndex(rtbSourceCode.SelectionStart) + 1;
            int indexInLine = GetIndexInLine(rtbSourceCode.SelectionStart);
            if (indexInLine != -1)
            {
                breakpointPos.column = indexInLine;
            }
            else
            {
            }
            CallCompile(rtbSourceCode.Text, breakpointPos);
            textIsChanged = false;
        }

        public void ColoredStatement(int row)
        {
            int lastSelectionStart = rtbSourceCode.SelectionStart;
            rtbSourceCode.SelectionStart = rtbSourceCode.GetFirstCharIndexFromLine(row);
            rtbSourceCode.SelectionLength = rtbSourceCode.Lines[row].Length;
            rtbSourceCode.SelectionBackColor = Color.Orange;
            rtbSourceCode.SelectionStart = lastSelectionStart;
            rtbSourceCode.SelectionLength = 0;
            textIsChanged = false;
        }

        public void RefreshErrors(string reason, int row, int col)
        {
            lvErrors.Items.Add(reason);
            lvErrors.Items[lvErrors.Items.Count - 1].SubItems.Add(string.Format("{0},{1}", row, col));
        }

        public void RefreshOutput(string outputText)
        {
            tbOutput.Text += outputText + "\r\n";
        }

        public void RefreshWatches(string variable, float value)
        {
            if (lvWatches.Items.ContainsKey(variable))
            {
                lvWatches.Items[variable].SubItems.RemoveAt(1);
                lvWatches.Items[variable].SubItems.Add(value.ToString());
            }
            else
            {
                ListViewItem newItem = new ListViewItem();
                newItem.Text = variable;
                newItem.Name = variable;
                newItem.SubItems.Add(value.ToString());
                lvWatches.Items.Add(newItem);
            }
        }

        public void RefreshBreakpoints(BreakpointPosition position)
        {
            if(position.row != 0)
                tbBreakpoints.Text += string.Format("Breakpoint on {0}th line", position.row) + "\r\n";
        }

        private void lvErrorsDoubleClick(object sender, EventArgs e)
        {
            string[] errorPosition = lvErrors.FocusedItem.SubItems[1].Text.Split(',');
            int line = int.Parse(errorPosition[0]) - 1;
            if (!lvErrors.FocusedItem.SubItems[1].Text.Equals("0,0"))
            {
                rtbSourceCode.SelectionStart = rtbSourceCode.GetFirstCharIndexFromLine(line) + int.Parse(errorPosition[1]) - 1;
                rtbSourceCode.Select();
            }
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private OpenFileDialog openFileDialog = new OpenFileDialog();
        private void OnOpen(object sender, EventArgs e)
        {
            openFileDialog.InitialDirectory = "C:\\Users\\1\\Documents\\Visual Studio 2008\\Projects\\Interpreter\\Interpreter";
            openFileDialog.FileOk += new CancelEventHandler(OnOpenClick);
            openFileDialog.ShowDialog(this);
        }

        void OnOpenClick(object sender, CancelEventArgs e)
        {
            rtbSourceCode.Text = File.ReadAllText(openFileDialog.FileName);
        }

        private SaveFileDialog saveFileDialog = new SaveFileDialog();
        private void OnSave(object sender, EventArgs e)
        {
            saveFileDialog.InitialDirectory = "C:\\Users\\1\\Documents\\Visual Studio 2008\\Projects\\Interpreter\\Interpreter";
            saveFileDialog.FileOk += new CancelEventHandler(OnSaveClick);
            saveFileDialog.ShowDialog(this);
        }

        private void OnSaveClick(object sender, CancelEventArgs e)
        {
            File.WriteAllText(saveFileDialog.FileName, rtbSourceCode.Text);
        }

        public delegate void OutputEventHandler(object sender, OutputEventArgs args);

        public event OutputEventHandler CallOutput;

        private void CallPrintResult()
        {
            OutputEventArgs args = new OutputEventArgs();
            if (CallOutput != null)
            {
                CallOutput(this, args);
            }
        }

        public delegate void ErrorEventHandler(object sender, ErrorEventArgs args);

        public event ErrorEventHandler CallHandlerException;

        private void CallErrorHandler()
        {
            ErrorEventArgs args = new ErrorEventArgs();
            if (CallHandlerException != null)
            {
                CallHandlerException(this, args);
            }
        }

        public delegate void InterpretationEventHandler(object sender, InterpretationEventArgs args);

        public event InterpretationEventHandler CallCompiling;
        public event InterpretationEventHandler CallInterpretation;
        public event InterpretationEventHandler CallTurnBasedInterpretation;

        private void CallInterpreter()
        {
            InterpretationEventArgs args = new InterpretationEventArgs();
            if (CallInterpretation != null)
            {
                CallInterpretation(this, args);
            }
        }

        private void CallInterpreterStep()
        {
            InterpretationEventArgs args = new InterpretationEventArgs();
            if (CallTurnBasedInterpretation != null)
            {
                CallTurnBasedInterpretation(this, args);
            }
        }

        private void CallCompile(string programCode, BreakpointPosition breakpointPosition)
        {
            InterpretationEventArgs args = new InterpretationEventArgs(programCode, breakpointPosition);
            if (CallCompiling != null)
            {
                CallCompiling(this, args);
            }
        }

        public delegate void AddBreakpointeventHandler(object sender, BreakpointEventArgs args);

        public event AddBreakpointeventHandler CallBreakpoint;

        private void CallAddingBreakpoint(int row, int colStartPos)
        {
            BreakpointEventArgs args = new BreakpointEventArgs(row, colStartPos);
            if (CallBreakpoint != null)
            {
                CallBreakpoint(this, args);
            }
        }

        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvErrors.Items.Clear();
            tbOutput.Clear();
            lvWatches.Items.Clear();
            CallCompile(rtbSourceCode.Text, new BreakpointPosition());
        }
    }
}