namespace FormsTest1;

partial class Form1
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

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {

        //Background
        this.BackgroundImage = Image.FromFile(@"img\X.jpg");
        this.BackgroundImageLayout = ImageLayout.Stretch;
                
        //Boton 1        
        this.button1.Image = new Bitmap(@"img\play.png");
        this.button1.ImageAlign = ContentAlignment.MiddleCenter;
        this.button1.Image = (Image)(new Bitmap(this.button1.Image, new Size(20, 20)));
        this.button1.Location = new Point(20, 120);
        this.button1.Anchor = AnchorStyles.Left | AnchorStyles.Top ;
        button1.Click += new EventHandler(SubmitCommands);
        //Boton 2
        this.button2.Image = new Bitmap(@"img\trash.png");       
        this.button2.ImageAlign = ContentAlignment.MiddleCenter;
        this.button2.Image = (Image)(new Bitmap(this.button2.Image, new Size(20, 20)));
        this.button2.Location = new Point(100, 120);         
        this.button2.Anchor = AnchorStyles.Left | AnchorStyles.Top ;        
        button2.Click += new System.EventHandler(ClearGraphics);
        
        //Panel para graficar
        //Panel panel = new Panel();
        panel.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
        panel.BackColor = System.Drawing.Color.FromArgb(1,1,1,0);
        panel.Location = new System.Drawing.Point(this.ClientSize.Width / 2, 0);
        panel.Size = new System.Drawing.Size(this.ClientSize.Width / 2, this.ClientSize.Height);

        //Text box
        this.textBox1 = new System.Windows.Forms.TextBox();
        this.textBox1.Size = new System.Drawing.Size(this.Height/3, this.Width/5); 
        this.textBox1.Location = new Point(20, 20);
        this.textBox1.Anchor = AnchorStyles.Left | AnchorStyles.Top ;       
        this.textBox1.Multiline = true;
        this.textBox1.AcceptsReturn = true;
        this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.textBox1.Enter += new System.EventHandler(textBox1_Enter);
        this.textBox1.Leave += new System.EventHandler(textBox1_Leave);
        this.textBox1.Text = "Introduzca sus instrucciones";
        this.textBox1.ForeColor = System.Drawing.Color.Gray;
        

        //Form1
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        //this.ClientSize = new System.Drawing.Size(800, 450);
        this.Text = "Geo Wall-E";

        
        this.textBox1.Location = new System.Drawing.Point(20,20);
        this.textBox1.Anchor = AnchorStyles.None;
        this.button1.Anchor = AnchorStyles.None;
        this.button2.Anchor = AnchorStyles.None;

        this.Controls.Add(panel);
        this.Controls.Add(this.button1);  
        this.Controls.Add(this.button2);
        this.Controls.Add(this.textBox1);

        this.Controls.SetChildIndex(textBox1, 0);
        this.Resize += new EventHandler(Form1_SizeChanged);
    }

    #endregion
}
