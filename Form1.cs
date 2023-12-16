using EvaluatorAnalize;
using WaLI.backend;
namespace FormsTest1;

public partial class Form1 : Form
{
    private TextBox textBox1;
    private readonly Button button1 = new();
    private readonly Button button2 = new();
    private readonly PictureBox panel = new();

    public Form1()
    {
        InitializeComponent();
        WindowState = FormWindowState.Maximized;

    }


    private void Form1_SizeChanged(object sender, EventArgs e)//responsivo
    {
        AdjustDynamicCanvasSize();
        button2.Location = new Point(20, Height / 3 + 30);
        button1.Location = new Point(Width / 5 - 60, Height / 3 + 30);
        textBox1.Location = new Point(20, 20);
        textBox1.Size = new Size(Width / 5, Height / 3);
    }

    private void AdjustDynamicCanvasSize()
    {
        int canvasWidth = ClientSize.Width / 2;
        int canvasHeight = ClientSize.Height;
        panel.Size = new Size(canvasWidth, canvasHeight);
        panel.Location = new Point((ClientSize.Width - canvasWidth) / 2, 0);
    }

    private void TextBox1_Enter(object sender, EventArgs e)// quitar el prompt
    {
        if (textBox1.Text == "Introduzca sus instrucciones")
        {
            textBox1.Text = "";
            textBox1.ForeColor = Color.Black;
        }
    }

    private void TextBox1_Leave(object sender, EventArgs e)//poner el prompt
    {
        if (string.IsNullOrWhiteSpace(textBox1.Text))
        {
            textBox1.Text = "Introduzca sus instrucciones";
            textBox1.ForeColor = Color.Gray;
        }
    }

    private void SubmitCommands(object sender, EventArgs e)
    {
        Graphics canvas = panel.CreateGraphics();
        var graphics = MiddleEnd.GSharp(textBox1.Text);

        if (graphics is string errorMessage)
        {
            MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        else if (graphics is List<ToDraw> toDrawList)
        {
            foreach (ToDraw graphic in toDrawList)
            {
                switch (graphic.figure)
                {
                    case "PointNode":
                        Draw_Point(graphic.color, graphic.points[0], graphic.comment, canvas);
                        break;
                    case "LineNode":
                        Draw_Line(graphic.color, graphic.points, graphic.comment, canvas);
                        break;
                    case "SegmentNode":
                        Draw_Segment(graphic.color, graphic.points, graphic.comment, canvas);
                        break;
                    case "RayNode":
                        Draw_Ray(graphic.color, graphic.points, graphic.comment, canvas);
                        break;
                    case "ArcNode":
                        Draw_Arc(graphic.color, graphic.points, graphic.comment, canvas);
                        break;
                    case "CircleNode":
                        Draw_Circle(graphic.color, graphic.points[0], graphic.rad, graphic.comment, canvas);
                        break;
                    default:
                        continue;
                }
            }
        }
        else
        {
            MessageBox.Show("Could not graph", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearGraphics(object sender, EventArgs e)
    {
        panel.Refresh();
        //textBox1.Text = "";
    }

    //Metodos para graficar

    private static void Draw_Point(Brush brush, Point punto, string comment, Graphics canvas)
    {
        canvas.FillEllipse(brush, punto.X, punto.Y, 7, 7);
        canvas.DrawString(comment, new Font("Arial", 12), Brushes.Black, punto.X, punto.Y);
    }
    private static void Draw_Arc(Brush brush, Point[] puntos, string comment, Graphics canvas)
    {
        // Ensure that we have at least 3 points
        if (puntos.Length < 3)
        {
            throw new ArgumentException("At least 3 points are required to draw an arc.");
        }
        // Calculate the bounding rectangle for the arc
        int x = Math.Min(puntos[0].X, Math.Min(puntos[1].X, puntos[2].X));
        int y = Math.Min(puntos[0].Y, Math.Min(puntos[1].Y, puntos[2].Y));
        int width = Math.Max(puntos[0].X, Math.Max(puntos[1].X, puntos[2].X)) - x;
        int height = Math.Max(puntos[0].Y, Math.Max(puntos[1].Y, puntos[2].Y)) - y;

        // Calculate the start and sweep angles
        float startAngle = Convert.ToSingle(Math.Atan2(puntos[0].Y - puntos[1].Y, puntos[0].X - puntos[1].X) * 180 / Math.PI);
        float endAngle = Convert.ToSingle(Math.Atan2(puntos[0].Y - puntos[2].Y, puntos[0].X - puntos[2].X) * 180 / Math.PI);

        // Draw the arc
        canvas.DrawArc(new Pen(brush), x, y, width, height, startAngle, endAngle - startAngle);

        // Draw the comment
        canvas.DrawString(comment, new Font("Arial", 12), Brushes.Black, puntos[0].X, puntos[0].Y);
    }

    private static void Draw_Circle(Brush brush, Point punto, double radio, string comment, Graphics canvas)
    {
        int diameter = (int)(radio * 2);
        int x = punto.X - (diameter / 2);
        int y = punto.Y - (diameter / 2);

        canvas.DrawEllipse(new Pen(brush), x, y, diameter, diameter);
        canvas.DrawString(comment, new Font("Arial", 12), Brushes.Black, punto.X, punto.Y);
    }
    private static void Draw_Segment(Brush brush, Point[] puntos, string comment, Graphics canvas)
    {
        foreach (Point punto in puntos)
        {
            Draw_Point(brush, punto, "", canvas);
        }
        canvas.DrawLine(new Pen(brush), puntos[0], puntos[1]);
        canvas.DrawString(comment, new Font("Arial", 12), Brushes.Black, puntos[0].X, puntos[0].Y);
    }
    private static void Draw_Line(Brush brush, Point[] puntos, string comment, Graphics canvas)
    {
        foreach (Point punto in puntos)
        {
            Draw_Point(brush, punto, "", canvas);
        }
        Point[] interceptos = Interceptos(puntos[0], puntos[1]);
        canvas.DrawLine(new Pen(brush), interceptos[0], interceptos[1]);
        canvas.DrawString(comment, new Font("Arial", 12), Brushes.Black, puntos[0].X, puntos[1].Y);
    }
    private static void Draw_Ray(Brush brush, Point[] puntos, string comment, Graphics canvas)
    {
        foreach (Point punto in puntos)
        {
            Draw_Point(brush, punto, "", canvas);
        }
        // Get the starting and ending points of the ray
        Point startPoint = puntos[0];
        Point endPoint = puntos[1];
        // Calculate the slope of the ray
        float slope = Pendiente(startPoint, endPoint);

        // Calculate the intercept of the ray with the y-axis
        float intercept = startPoint.Y - slope * startPoint.X;

        // Calculate the x-coordinate of the point where the ray intersects the canvas boundary
        int canvasBoundaryX = (int)canvas.ClipBounds.Width;

        // Calculate the y-coordinate of the point where the ray intersects the canvas boundary
        int canvasBoundaryY = (int)(slope * canvasBoundaryX + intercept);

        // Draw the ray from the starting point to the point where it intersects the canvas boundary
        canvas.DrawLine(new Pen(brush), startPoint, new Point(canvasBoundaryX, canvasBoundaryY));

        canvas.DrawString(comment, new Font("Arial", 12), Brushes.Black, startPoint.X, startPoint.Y);
    }

    public static float Pendiente(Point p1, Point p2)
    {
        return (p2.X - p1.X) / (p2.Y - p1.Y);
    }

    public static Point[] Interceptos(Point p1, Point p2)
    {
        float m = Pendiente(p1, p2);
        float n = p1.Y - m * p1.X;
        float cero = -n / m;
        return new Point[] { new(0, (int)n), new((int)cero, 0) };
    }
}
