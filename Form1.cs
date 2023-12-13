namespace FormsTest1;

public partial class Form1 : Form
{
    private TextBox textBox1 ;
    private Button button1 = new Button();
    private Button button2 = new Button();
    private PictureBox panel = new PictureBox();

    public Form1()
    {
             
        InitializeComponent(); 
        this.WindowState = FormWindowState.Maximized;  
        
    }

    
     private void Form1_SizeChanged(object sender, EventArgs e)//responsivo
     {
        AdjustDynamicCanvasSize();
        this.button2.Location = new Point(20,this.Height/3 + 30);
        this.button1.Location = new Point(this.Width/5 - 60,this.Height/3 + 30); 
        this.textBox1.Location = new Point(20,20);
        this.textBox1.Size = new System.Drawing.Size(this.Width/5, this.Height/3);
     }

     private void AdjustDynamicCanvasSize()
        {
            this.panel.Size = new System.Drawing.Size(this.ClientSize.Width/2, this.ClientSize.Height);
            this.panel.Location = new System.Drawing.Point(this.ClientSize.Width/2, 0);
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
        Graphics canvas = panel.CreateGraphics();
        foreach (ToDraw graphic in GSharp(textBox1.Text))
        {
            switch (graphic.figure.ToLower)
            {
                case "point":
                {Draw_Point(graphic.color,graphic.points[0], graphic.comment, canvas);}
                case "line":
                {Draw_Line(graphic.color, graphic.points, graphic.comment, canvas);}
                case "segment":
                {Draw_Segment(graphic.color, graphic.points, graphic.comment, canvas);}
                case "ray":
                {Draw_Ray(graphic.color, graphic.points, graphic.comment, canvas);}
                case "arc":
                {Draw_Arc(graphic.color, graphic.points, graphic.rad, graphic.comment, canvas);}
                case "circle":
                {Draw_Circle(graphic.color, graphic.points[0], graphic.rad, graphic.comment, canvas);}
                default:
                {continue;}
            }
        }
    }

    private void ClearGraphics(object sender, EventArgs e)
    {
        panel.Refresh();
        //textBox1.Text = "";
    }

    //Metodos para graficar

    private void Draw_Point(Brush brush, Point punto, string comment, Graphics canvas)
    {
        canvas.FillEllipse(brush, punto.X , punto.Y , 7, 7);
        canvas.DrawString(comment, new Font("Arial", 12), Brushes.Black, X, Y);
    }
    private void Draw_Arc(Brush brush, Point[] puntos, double radio, string comment, Graphics canvas)
    {
        foreach (Point punto in puntos)
        {
            Draw_Point(brush, punto, "", canvas);
        }
        canvas.DrawArc();
        canvas.DrawString(comment, new Font("Arial", 12), Brushes.Black, X, Y);
    }

    private void Draw_Circle(Brush brush, Point punto, double radio, string comment, Graphics canvas)
    {
        Draw_Point(brush, punto, "", canvas);       
        canvas.DrawEllipse(brush, punto.X, punto.Y, radio, radio);
        canvas.DrawString(comment, new Font("Arial", 12), Brushes.Black, X, Y);
    }
    private void Draw_Segment(Brush brush, Point[] puntos, string comment, Graphics canvas)
    {
        foreach (Point punto in puntos)
        {
            Draw_Point(brush, punto, "", canvas);
        }
        canvas.DrawLine(puntos[0],puntos[1]);
        canvas.DrawString(comment, new Font("Arial", 12), Brushes.Black, X, Y);
    }
    private void Draw_Line(Brush brush, Point[] puntos, string comment, Graphics canvas)
    {
        foreach (Point punto in puntos)
        {
            Draw_Point(brush, punto, "", canvas);
        }
        Point[] interceptos = Interceptos(puntos[0],puntos[1]);
        canvas.DrawLine(interceptos[0], interceptos[1]);
        canvas.DrawString(comment, new Font("Arial", 12), Brushes.Black, X, Y);
    }
    private void Draw_Ray(Brush brush, Point[] puntos, string comment, Graphics canvas)
    {
        foreach (Point punto in puntos)
        {
            Draw_Point(brush, punto, "", canvas);
        }

        canvas.DrawString(comment, new Font("Arial", 12), Brushes.Black, X, Y);
    }
    
    public static float Pendiente(Point p1, Point p2)
    {
        return (p2.X - p1.X) / (p2.Y - p1.Y);
    }

    public static Point[] Interceptos(Point p1, Point p2)
    {
        float m = Pendiente(p1,p2);
        float n = p1.Y - m* p1.X ;
        float cero = -n/m ;
        return // 0,n y cero,0
    }
}
