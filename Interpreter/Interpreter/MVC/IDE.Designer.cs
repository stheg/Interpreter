namespace Interpreter
{
    partial class IDE
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IDE));
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.breakpointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addBreakpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.rtbSourceCode = new System.Windows.Forms.RichTextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpErrors = new System.Windows.Forms.TabPage();
            this.lvErrors = new System.Windows.Forms.ListView();
            this.colError = new System.Windows.Forms.ColumnHeader();
            this.colPosition = new System.Windows.Forms.ColumnHeader();
            this.tpOutput = new System.Windows.Forms.TabPage();
            this.tbOutput = new System.Windows.Forms.TextBox();
            this.tpWatches = new System.Windows.Forms.TabPage();
            this.lvWatches = new System.Windows.Forms.ListView();
            this.colVariavle = new System.Windows.Forms.ColumnHeader();
            this.colValue = new System.Windows.Forms.ColumnHeader();
            this.cMenuWatches = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tpBreakpoints = new System.Windows.Forms.TabPage();
            this.tbBreakpoints = new System.Windows.Forms.TextBox();
            this.mainMenu.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpErrors.SuspendLayout();
            this.tpOutput.SuspendLayout();
            this.tpWatches.SuspendLayout();
            this.tpBreakpoints.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(686, 24);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OnOpen);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.OnSave);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.OnExit);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runToolStripMenuItem,
            this.debugToolStripMenuItem,
            this.compileToolStripMenuItem,
            this.breakpointsToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.editToolStripMenuItem.Text = "&Run";
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.runToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.runToolStripMenuItem.Text = "Run";
            this.runToolStripMenuItem.Click += new System.EventHandler(this.OnRun);
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F10;
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.debugToolStripMenuItem.Text = "Debug";
            this.debugToolStripMenuItem.Click += new System.EventHandler(this.OnDebug);
            // 
            // compileToolStripMenuItem
            // 
            this.compileToolStripMenuItem.Name = "compileToolStripMenuItem";
            this.compileToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.compileToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.compileToolStripMenuItem.Text = "Compile";
            this.compileToolStripMenuItem.Click += new System.EventHandler(this.compileToolStripMenuItem_Click);
            // 
            // breakpointsToolStripMenuItem
            // 
            this.breakpointsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addBreakpointToolStripMenuItem});
            this.breakpointsToolStripMenuItem.Name = "breakpointsToolStripMenuItem";
            this.breakpointsToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.breakpointsToolStripMenuItem.Text = "Breakpoints";
            // 
            // addBreakpointToolStripMenuItem
            // 
            this.addBreakpointToolStripMenuItem.Name = "addBreakpointToolStripMenuItem";
            this.addBreakpointToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F9;
            this.addBreakpointToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.addBreakpointToolStripMenuItem.Text = "Add/remove breakpoint";
            this.addBreakpointToolStripMenuItem.Click += new System.EventHandler(this.AddRemoveBreakpointClick);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.rtbSourceCode);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(686, 285);
            this.splitContainer1.SplitterDistance = 153;
            this.splitContainer1.TabIndex = 1;
            // 
            // rtbSourceCode
            // 
            this.rtbSourceCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbSourceCode.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rtbSourceCode.Location = new System.Drawing.Point(0, 0);
            this.rtbSourceCode.Name = "rtbSourceCode";
            this.rtbSourceCode.Size = new System.Drawing.Size(686, 153);
            this.rtbSourceCode.TabIndex = 0;
            this.rtbSourceCode.Text = "";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpErrors);
            this.tabControl1.Controls.Add(this.tpOutput);
            this.tabControl1.Controls.Add(this.tpWatches);
            this.tabControl1.Controls.Add(this.tpBreakpoints);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(686, 128);
            this.tabControl1.TabIndex = 0;
            // 
            // tpErrors
            // 
            this.tpErrors.Controls.Add(this.lvErrors);
            this.tpErrors.Location = new System.Drawing.Point(4, 22);
            this.tpErrors.Name = "tpErrors";
            this.tpErrors.Padding = new System.Windows.Forms.Padding(3);
            this.tpErrors.Size = new System.Drawing.Size(678, 102);
            this.tpErrors.TabIndex = 0;
            this.tpErrors.Text = "Errors";
            this.tpErrors.UseVisualStyleBackColor = true;
            // 
            // lvErrors
            // 
            this.lvErrors.AllowColumnReorder = true;
            this.lvErrors.AutoArrange = false;
            this.lvErrors.BackgroundImageTiled = true;
            this.lvErrors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colError,
            this.colPosition});
            this.lvErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvErrors.Location = new System.Drawing.Point(3, 3);
            this.lvErrors.Name = "lvErrors";
            this.lvErrors.ShowGroups = false;
            this.lvErrors.Size = new System.Drawing.Size(672, 96);
            this.lvErrors.TabIndex = 0;
            this.lvErrors.UseCompatibleStateImageBehavior = false;
            this.lvErrors.View = System.Windows.Forms.View.Details;
            this.lvErrors.DoubleClick += new System.EventHandler(this.lvErrorsDoubleClick);
            // 
            // colError
            // 
            this.colError.Text = "Error";
            this.colError.Width = 201;
            // 
            // colPosition
            // 
            this.colPosition.Text = "Position";
            this.colPosition.Width = 148;
            // 
            // tpOutput
            // 
            this.tpOutput.Controls.Add(this.tbOutput);
            this.tpOutput.Location = new System.Drawing.Point(4, 22);
            this.tpOutput.Name = "tpOutput";
            this.tpOutput.Padding = new System.Windows.Forms.Padding(3);
            this.tpOutput.Size = new System.Drawing.Size(678, 102);
            this.tpOutput.TabIndex = 1;
            this.tpOutput.Text = "Output";
            this.tpOutput.UseVisualStyleBackColor = true;
            this.tpOutput.Click += new System.EventHandler(this.tpOutput_Click);
            // 
            // tbOutput
            // 
            this.tbOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbOutput.Location = new System.Drawing.Point(3, 3);
            this.tbOutput.Multiline = true;
            this.tbOutput.Name = "tbOutput";
            this.tbOutput.ReadOnly = true;
            this.tbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbOutput.Size = new System.Drawing.Size(672, 96);
            this.tbOutput.TabIndex = 0;
            // 
            // tpWatches
            // 
            this.tpWatches.Controls.Add(this.lvWatches);
            this.tpWatches.Location = new System.Drawing.Point(4, 22);
            this.tpWatches.Name = "tpWatches";
            this.tpWatches.Size = new System.Drawing.Size(678, 102);
            this.tpWatches.TabIndex = 2;
            this.tpWatches.Text = "Watches";
            this.tpWatches.UseVisualStyleBackColor = true;
            // 
            // lvWatches
            // 
            this.lvWatches.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colVariavle,
            this.colValue});
            this.lvWatches.ContextMenuStrip = this.cMenuWatches;
            this.lvWatches.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvWatches.Location = new System.Drawing.Point(0, 0);
            this.lvWatches.Name = "lvWatches";
            this.lvWatches.Size = new System.Drawing.Size(678, 102);
            this.lvWatches.TabIndex = 0;
            this.lvWatches.UseCompatibleStateImageBehavior = false;
            this.lvWatches.View = System.Windows.Forms.View.Details;
            // 
            // colVariavle
            // 
            this.colVariavle.Text = "Variable";
            this.colVariavle.Width = 75;
            // 
            // colValue
            // 
            this.colValue.Text = "Value";
            // 
            // cMenuWatches
            // 
            this.cMenuWatches.Name = "cMenuWatches";
            this.cMenuWatches.Size = new System.Drawing.Size(61, 4);
            // 
            // tpBreakpoints
            // 
            this.tpBreakpoints.Controls.Add(this.tbBreakpoints);
            this.tpBreakpoints.Location = new System.Drawing.Point(4, 22);
            this.tpBreakpoints.Name = "tpBreakpoints";
            this.tpBreakpoints.Size = new System.Drawing.Size(678, 102);
            this.tpBreakpoints.TabIndex = 3;
            this.tpBreakpoints.Text = "Breakpoints";
            this.tpBreakpoints.UseVisualStyleBackColor = true;
            // 
            // tbBreakpoints
            // 
            this.tbBreakpoints.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbBreakpoints.Location = new System.Drawing.Point(0, 0);
            this.tbBreakpoints.Multiline = true;
            this.tbBreakpoints.Name = "tbBreakpoints";
            this.tbBreakpoints.ReadOnly = true;
            this.tbBreakpoints.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbBreakpoints.Size = new System.Drawing.Size(678, 102);
            this.tbBreakpoints.TabIndex = 0;
            // 
            // IDE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 309);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.mainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.Name = "IDE";
            this.Text = "PInt";
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tpErrors.ResumeLayout(false);
            this.tpOutput.ResumeLayout(false);
            this.tpOutput.PerformLayout();
            this.tpWatches.ResumeLayout(false);
            this.tpBreakpoints.ResumeLayout(false);
            this.tpBreakpoints.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox rtbSourceCode;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpErrors;
        private System.Windows.Forms.TabPage tpOutput;
        private System.Windows.Forms.TabPage tpWatches;
        private System.Windows.Forms.TabPage tpBreakpoints;
        private System.Windows.Forms.ListView lvErrors;
        private System.Windows.Forms.ColumnHeader colError;
        private System.Windows.Forms.ColumnHeader colPosition;
        private System.Windows.Forms.TextBox tbOutput;
        private System.Windows.Forms.ListView lvWatches;
        private System.Windows.Forms.ColumnHeader colVariavle;
        private System.Windows.Forms.ColumnHeader colValue;
        private System.Windows.Forms.ContextMenuStrip cMenuWatches;
        private System.Windows.Forms.ToolStripMenuItem breakpointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addBreakpointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compileToolStripMenuItem;
        private System.Windows.Forms.TextBox tbBreakpoints;
    }
}