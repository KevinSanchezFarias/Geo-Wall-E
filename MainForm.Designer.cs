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
        BackColor = ColorTranslator.FromHtml("#24273a");

        // Initialize the TextBox somewhere in your code, e.g. in your form's constructor
        errorTextBox = new TextBox();
        errorTextBox.Multiline = true;
        errorTextBox.ScrollBars = ScrollBars.Vertical;
        errorTextBox.ReadOnly = true;
        errorTextBox.Height = 75;
        errorTextBox.ForeColor = ColorTranslator.FromHtml("#ed8796"); // Set the text color to #ed8796
        errorTextBox.BackColor = ColorTranslator.FromHtml("#363a4f"); // Set the background color to #363a4f
        errorTextBox.Width = (int)(this.ClientSize.Width * 0.7);
        errorTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
        errorTextBox.Dock = DockStyle.Bottom;


        //Button 1        
        button1.Image = new Bitmap("./img/play.png");
        button1.ImageAlign = ContentAlignment.MiddleCenter;
        button1.Image = (Image)(new Bitmap(this.button1.Image, new Size(20, 20)));
        button1.Anchor = AnchorStyles.Left | AnchorStyles.Top;
        button1.Click += new EventHandler(SubmitCommands);
        //Button 2
        button2.Image = new Bitmap("./img/trash.png");
        button2.ImageAlign = ContentAlignment.MiddleCenter;
        button2.Image = (Image)(new Bitmap(this.button2.Image, new Size(20, 20)));
        button2.Anchor = AnchorStyles.Left | AnchorStyles.Top;
        button2.Click += new System.EventHandler(ClearGraphics);

        //Graph panel
        panel.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
        panel.BackColor = System.Drawing.Color.FromArgb(1, 1, 1, 0);
        panel.Location = new System.Drawing.Point(0, 0);
        panel.Size = new System.Drawing.Size((int)(this.ClientSize.Width * 0.7), (int)(this.ClientSize.Height * 0.8));

        //Text box
        textBox1 = new System.Windows.Forms.TextBox();
        textBox1.Anchor = AnchorStyles.Left | AnchorStyles.Top;
        textBox1.Multiline = true;
        textBox1.AcceptsReturn = true;
        textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        textBox1.ForeColor = System.Drawing.Color.White;

        //MainForm
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        //this.ClientSize = new System.Drawing.Size(800, 450);
        this.Text = "Geo Wall-E";


        this.textBox1.Location = new System.Drawing.Point(20, 20);
        this.textBox1.Anchor = AnchorStyles.None;
        this.button1.Anchor = AnchorStyles.None;
        this.button2.Anchor = AnchorStyles.None;

        this.Controls.Add(panel);
        this.Controls.Add(this.button1);
        this.Controls.Add(this.button2);
        this.Controls.Add(this.textBox1);
        this.Controls.Add(this.errorTextBox);

        this.Controls.SetChildIndex(textBox1, 0);
        this.Resize += new EventHandler(Form1_SizeChanged);
    }

    #endregion
}
