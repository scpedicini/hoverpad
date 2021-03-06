﻿namespace Chinese_Hoverpad
{
    partial class WndEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WndEditor));
            this.LabelPopup = new System.Windows.Forms.Label();
            this.TimerPopup = new System.Windows.Forms.Timer(this.components);
            this.TextEditor = new System.Windows.Forms.TextBox();
            this.MainMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuToSimplified = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuToTraditional = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuFont = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // LabelPopup
            // 
            this.LabelPopup.AutoSize = true;
            this.LabelPopup.BackColor = System.Drawing.Color.PeachPuff;
            this.LabelPopup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LabelPopup.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelPopup.Location = new System.Drawing.Point(12, 338);
            this.LabelPopup.Name = "LabelPopup";
            this.LabelPopup.Size = new System.Drawing.Size(65, 26);
            this.LabelPopup.TabIndex = 2;
            this.LabelPopup.Text = "Definition1\r\nDefinition2";
            this.LabelPopup.UseMnemonic = false;
            // 
            // TimerPopup
            // 
            this.TimerPopup.Enabled = true;
            this.TimerPopup.Interval = 1000;
            this.TimerPopup.Tick += new System.EventHandler(this.TimerPopup_Tick);
            // 
            // TextEditor
            // 
            this.TextEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextEditor.ContextMenuStrip = this.MainMenu;
            this.TextEditor.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextEditor.ImeMode = System.Windows.Forms.ImeMode.On;
            this.TextEditor.Location = new System.Drawing.Point(12, 12);
            this.TextEditor.MaxLength = 0;
            this.TextEditor.Multiline = true;
            this.TextEditor.Name = "TextEditor";
            this.TextEditor.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextEditor.Size = new System.Drawing.Size(733, 376);
            this.TextEditor.TabIndex = 3;
            this.TextEditor.Text = "在歷史上那種事件已經發生了很多次。\r\n";
            this.TextEditor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TextEditor_MouseMove);
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuToSimplified,
            this.MenuToTraditional,
            this.MenuFont});
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(189, 70);
            // 
            // MenuToSimplified
            // 
            this.MenuToSimplified.Name = "MenuToSimplified";
            this.MenuToSimplified.Size = new System.Drawing.Size(188, 22);
            this.MenuToSimplified.Text = "Convert to Simplified";
            this.MenuToSimplified.Click += new System.EventHandler(this.MenuToSimplified_Click);
            // 
            // MenuToTraditional
            // 
            this.MenuToTraditional.Name = "MenuToTraditional";
            this.MenuToTraditional.Size = new System.Drawing.Size(188, 22);
            this.MenuToTraditional.Text = "Convert to Traditional";
            this.MenuToTraditional.Click += new System.EventHandler(this.MenuToTraditional_Click);
            // 
            // MenuFont
            // 
            this.MenuFont.Name = "MenuFont";
            this.MenuFont.Size = new System.Drawing.Size(188, 22);
            this.MenuFont.Text = "Font...";
            this.MenuFont.Click += new System.EventHandler(this.MenuFont_Click);
            // 
            // WndEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(757, 400);
            this.Controls.Add(this.LabelPopup);
            this.Controls.Add(this.TextEditor);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WndEditor";
            this.Text = "Chinese Hoverpad (Specular Realms LLC)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WndEditor_FormClosing);
            this.MainMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelPopup;
        private System.Windows.Forms.Timer TimerPopup;
        private System.Windows.Forms.TextBox TextEditor;
        private System.Windows.Forms.ContextMenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem MenuToSimplified;
        private System.Windows.Forms.ToolStripMenuItem MenuToTraditional;
        private System.Windows.Forms.ToolStripMenuItem MenuFont;

    }
}

