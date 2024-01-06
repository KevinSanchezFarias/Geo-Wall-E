namespace FormsTest1;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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

    #region WindowsFormDesigner

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        //Background
        //BackColor = ColorTranslator.FromHtml("#24273a");

        errorTextBox = new TextBox();
        errorTextBox.Multiline = true;
        errorTextBox.ScrollBars = ScrollBars.Vertical;
        errorTextBox.ReadOnly = true;
        errorTextBox.Height = 75;
        errorTextBox.BackColor = ColorTranslator.FromHtml("#363a4f"); // Set the background color to #363a4f
        errorTextBox.Width = (int)(this.ClientSize.Width * 0.7);
        errorTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
        errorTextBox.Dock = DockStyle.Bottom;


        //Button 1        
        button1.Image = new Bitmap("./img/play.png");
        button1.ImageAlign = ContentAlignment.MiddleCenter;
        button1.Image = (Image)(new Bitmap(this.button1.Image, new Size(20, 20)));
        button1.Click += new EventHandler(SubmitCommands);
        button1.FlatStyle = FlatStyle.Popup;
        button1.BackColor = ColorTranslator.FromHtml("#181926");

        //Button 2
        button2.Image = new Bitmap("./img/trash.png");
        button2.ImageAlign = ContentAlignment.MiddleCenter;
        button2.Image = (Image)(new Bitmap(this.button2.Image, new Size(20, 20)));
        button2.Click += new System.EventHandler(ClearGraphics);
        button2.FlatStyle = FlatStyle.Popup;
        button2.BackColor = ColorTranslator.FromHtml("#181926");

        //Graph panel
        panel.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
        panel.BackColor = System.Drawing.Color.FromArgb(1, 1, 1, 0);
        panel.Location = new System.Drawing.Point(0, 0);
        panel.Size = new System.Drawing.Size((int)(this.ClientSize.Width * 0.7), (int)(this.ClientSize.Height * 0.8));
        panel.BackColor = ColorTranslator.FromHtml("#24273a");
        //Text box
        textBox1 = new System.Windows.Forms.TextBox();
        textBox1.Anchor = AnchorStyles.Left | AnchorStyles.Top;
        textBox1.Multiline = true;
        textBox1.AcceptsReturn = true;
        textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        textBox1.ForeColor = System.Drawing.Color.White;


        //MainForm
        components = new System.ComponentModel.Container();
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        //this.ClientSize = new System.Drawing.Size(800, 450);
        Text = "Geo Wall-E";


        textBox1.Location = new System.Drawing.Point(20, 20);
        textBox1.Anchor = AnchorStyles.None;
        button1.Anchor = AnchorStyles.None;
        button2.Anchor = AnchorStyles.None;

        Controls.Add(panel);
        Controls.Add(button1);
        Controls.Add(button2);
        Controls.Add(textBox1);
        Controls.Add(errorTextBox);

        Controls.SetChildIndex(textBox1, 0);
        Resize += new EventHandler(Form1_SizeChanged);
    }

    #endregion
}
