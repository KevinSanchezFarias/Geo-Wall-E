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
        this.BackgroundImage = Image.FromFile(@"C:\Users\DELL\Desktop\PEpe\FormsTest1\X.jpg");
        this.BackgroundImageLayout = ImageLayout.Stretch;
                
        //Botones
        //Button button1 = new Button();
        //Button button2 = new Button();
        button1.Anchor = AnchorStyles.Left | AnchorStyles.Top ;
        button2.Anchor = AnchorStyles.Left | AnchorStyles.Top ;
        button2.Image = new Bitmap(@"C:\Users\DELL\Desktop\PEpe\FormsTest1\trash.png");
        button1.Image = new Bitmap(@"C:\Users\DELL\Desktop\PEpe\FormsTest1\play.png");
        button1.Image = (Image)(new Bitmap(button1.Image, new Size(20, 20)));
        button2.Image = (Image)(new Bitmap(button2.Image, new Size(20, 20)));
        button1.ImageAlign = ContentAlignment.MiddleCenter;
        button2.ImageAlign = ContentAlignment.MiddleCenter;
        button2.Location = new Point(100, 120);
        button1.Location = new Point(20, 120); 
        
        //Panel para graficar
        //Panel panel1 = new Panel();
        panel1.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
        panel1.BackColor = System.Drawing.Color.FromArgb(1,1,1,0);
        panel1.Location = new System.Drawing.Point(this.ClientSize.Width / 2, 0);
        panel1.Size = new System.Drawing.Size(this.ClientSize.Width / 2, this.ClientSize.Height);

        //Text box
        this.textBox1 = new System.Windows.Forms.TextBox();
        this.textBox1.Size = new System.Drawing.Size(200, 90); 
        textBox1.Location = new Point(20, 20);
        textBox1.Anchor = AnchorStyles.Left | AnchorStyles.Top ;       
        this.textBox1.Multiline = true;
        this.textBox1.AcceptsReturn = true;
        this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

        //Form1
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        //this.ClientSize = new System.Drawing.Size(800, 450);
        this.Text = "Geo Wall-E";

        
        this.textBox1.Location = new System.Drawing.Point(20,20);
        this.textBox1.Anchor = AnchorStyles.None;
        button1.Anchor = AnchorStyles.None;
        button2.Anchor = AnchorStyles.None;

        this.Controls.Add(panel1);
        this.Controls.Add(button1);  
        this.Controls.Add(button2);
        this.Controls.Add(this.textBox1);

        this.Controls.SetChildIndex(textBox1, 0);
    }

    #endregion
}
