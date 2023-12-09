namespace FormsTest1;

public partial class Form1 : Form
{
    private TextBox textBox1;
    private Button button1 = new Button();
    private Button button2 = new Button();
    private Panel panel1 = new Panel();

    public Form1()
    {
             
        InitializeComponent(); 
        
        textBox1.Enter += new EventHandler(textBox1_Enter);
        textBox1.Leave += new EventHandler(textBox1_Leave);
        textBox1.Text = "Introduzca sus instrucciones";
        textBox1.ForeColor = System.Drawing.Color.Gray;
        this.Resize += new EventHandler(Form1_SizeChanged);
        button1.Click += new EventHandler(SubmitCommands);
        button2.Click += new EventHandler(ClearGraphics);
    }

    
     private void Form1_SizeChanged(object sender, EventArgs e)
     {
        AdjustDynamicPanelSize();
        this.button2.Location = new Point(20,120);
        this.button1.Location = new Point(120,120); 
        this.textBox1.Location = new System.Drawing.Point(20,20);
     }

     private void AdjustDynamicPanelSize()
        {
            this.panel1.Size = new System.Drawing.Size(this.ClientSize.Width/2, this.ClientSize.Height);
            this.panel1.Location = new System.Drawing.Point(this.ClientSize.Width/2, 0);
        }

    private void textBox1_Enter(object sender, EventArgs e)// quitar el prompt
    {
    if (textBox1.Text == "Introduzca sus instrucciones")
        {
            textBox1.Text = "";
            textBox1.ForeColor = System.Drawing.Color.Black; 
        }
    }

    private void textBox1_Leave(object sender, EventArgs e)//poner el prompt
    {
    if (string.IsNullOrWhiteSpace(textBox1.Text))
        {
            textBox1.Text = "Introduzca sus instrucciones";
            textBox1.ForeColor = System.Drawing.Color.Gray;
        }
    }

    private void SubmitCommands(object sender, EventArgs e)
    {
        //mandar el texto al interprete
    }

    private void ClearGraphics(object sender, EventArgs e)
    {
        //Limpiar el panel de graficos
    }

}
