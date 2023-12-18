using EvaluatorAnalize;
using WaLI.backend;
namespace FormsTest1;

public partial class MainForm : Form
{
    private TextBox textBox1;
    private readonly Button button1 = new();
    private readonly Button button2 = new();
    private readonly PictureBox panel = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="MainForm"/> class.
    /// </summary>
    public MainForm()
    {
        textBox1 = new();
        InitializeComponent();
        WindowState = FormWindowState.Maximized;

    }


    /// <summary>
    /// Event handler for the SizeChanged event of Form1.
    /// Adjusts the dynamic canvas size and updates the locations and sizes of the controls on the form.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">An EventArgs object that contains the event data.</param>
    private void Form1_SizeChanged(object sender, EventArgs e)
    {
        AdjustDynamicCanvasSize();
        button2.Location = new Point(20, Height / 3 + 30);
        button1.Location = new Point(Width / 5 - 60, Height / 3 + 30);
        textBox1.Location = new Point(20, 20);
        textBox1.Size = new Size(Width / 5, Height / 3);
    }

    /// <summary>
    /// Adjusts the size of the dynamic canvas based on the client size.
    /// </summary>
    private void AdjustDynamicCanvasSize()
    {
        int canvasWidth = ClientSize.Width / 2;
        int canvasHeight = ClientSize.Height;
        panel.Size = new Size(canvasWidth, canvasHeight);
        panel.Location = new Point((ClientSize.Width - canvasWidth) / 2, 0);
    }

    /// <summary>
    /// Event handler for the Enter event of TextBox1.
    /// Clears the text in TextBox1 if it is set to the default instruction text.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">The event arguments.</param>
    private void TextBox1_Enter(object sender, EventArgs e)
    {
        if (textBox1.Text == "Enter your instructions")
        {
            textBox1.Text = "";
            textBox1.ForeColor = Color.Black;
        }
    }

    /// <summary>
    /// Event handler for the TextBox1 Leave event.
    /// Sets the default text and color if the TextBox is empty.
    /// </summary>
    /// <param name="sender">The object that triggered the event.</param>
    /// <param name="e">The event arguments.</param>
    private void TextBox1_Leave(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(textBox1.Text))
        {
            textBox1.Text = "Enter your instructionss";
            textBox1.ForeColor = Color.Gray;
        }
    }

    /// <summary>
    /// Handles the event when the submit button is clicked.
    /// </summary>
    /// <param name="sender">The object that triggered the event.</param>
    /// <param name="e">The event arguments.</param>
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

    /// <summary>
    /// Clears the graphics on the panel and optionally clears the text in the textBox.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">The event arguments.</param>
    private void ClearGraphics(object sender, EventArgs e)
    {
        panel.Refresh();
        //textBox1.Text = "";
    }
    #region GraphMethods
    /// <summary>
    /// Draws a point on the canvas using the specified brush, point coordinates, comment, and graphics object.
    /// </summary>
    /// <param name="brush">The brush used to fill the ellipse representing the point.</param>
    /// <param name="punto">The coordinates of the point.</param>
    /// <param name="comment">The comment to be displayed next to the point.</param>
    /// <param name="canvas">The graphics object representing the canvas.</param>
    private static void Draw_Point(Brush brush, Point punto, string comment, Graphics canvas)
    {
        canvas.FillEllipse(brush, punto.X, punto.Y, 7, 7);
        canvas.DrawString(comment, new Font("Arial", 12), Brushes.Black, punto.X, punto.Y);
    }
    /// <summary>
    /// Draws an arc on the specified canvas using the provided brush, points, and comment.
    /// </summary>
    /// <param name="brush">The brush used to fill the arc.</param>
    /// <param name="puntos">An array of points that define the arc.</param>
    /// <param name="comment">The comment to be displayed.</param>
    /// <param name="canvas">The graphics canvas on which the arc will be drawn.</param>
    /// <exception cref="ArgumentException">Thrown when less than 3 points are provided.</exception>
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

    /// <summary>
    /// Draws a circle on the specified canvas using the provided brush, center point, radius, comment, and graphics object.
    /// </summary>
    /// <param name="brush">The brush used to fill the circle.</param>
    /// <param name="punto">The center point of the circle.</param>
    /// <param name="radio">The radius of the circle.</param>
    /// <param name="comment">The comment to be displayed near the circle.</param>
    /// <param name="canvas">The graphics object representing the canvas on which the circle will be drawn.</param>
    private static void Draw_Circle(Brush brush, Point punto, double radio, string comment, Graphics canvas)
    {
        int diameter = (int)(radio * 2);
        int x = punto.X - (diameter / 2);
        int y = punto.Y - (diameter / 2);

        canvas.DrawEllipse(new Pen(brush), x, y, diameter, diameter);
        canvas.DrawString(comment, new Font("Arial", 12), Brushes.Black, punto.X, punto.Y);
    }
    /// <summary>
    /// Draws a segment on the canvas using the specified brush, points, comment, and graphics object.
    /// </summary>
    /// <param name="brush">The brush used to draw the segment.</param>
    /// <param name="puntos">The array of points that define the segment.</param>
    /// <param name="comment">The comment to be displayed near the starting point of the segment.</param>
    /// <param name="canvas">The graphics object representing the canvas.</param>
    private static void Draw_Segment(Brush brush, Point[] puntos, string comment, Graphics canvas)
    {
        foreach (Point punto in puntos)
        {
            Draw_Point(brush, punto, "", canvas);
        }
        canvas.DrawLine(new Pen(brush), puntos[0], puntos[1]);
        canvas.DrawString(comment, new Font("Arial", 12), Brushes.Black, puntos[0].X, puntos[0].Y);
    }
    /// <summary>
    /// Draws a line connecting the given points on the canvas using the specified brush.
    /// </summary>
    /// <param name="brush">The brush used to draw the line.</param>
    /// <param name="puntos">An array of points that define the line.</param>
    /// <param name="comment">A comment to be displayed near the line.</param>
    /// <param name="canvas">The graphics canvas on which the line is drawn.</param>
    private static void Draw_Line(Brush brush, Point[] puntos, string comment, Graphics canvas)
    {
        foreach (Point punto in puntos)
        {
            Draw_Point(brush, punto, "", canvas);
        }
        Point[] interceptos = Intercepts(puntos[0], puntos[1]);
        canvas.DrawLine(new Pen(brush), interceptos[0], interceptos[1]);
        canvas.DrawString(comment, new Font("Arial", 12), Brushes.Black, puntos[0].X, puntos[1].Y);
    }
    /// <summary>
    /// Draws a ray on the canvas from the starting point to the point where it intersects the canvas boundary.
    /// </summary>
    /// <param name="brush">The brush used to draw the ray.</param>
    /// <param name="puntos">An array of points representing the starting and ending points of the ray.</param>
    /// <param name="comment">A comment to be displayed at the starting point of the ray.</param>
    /// <param name="canvas">The graphics canvas on which the ray will be drawn.</param>
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
        float slope = Earring(startPoint, endPoint);

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

    /// <summary>
    /// Calculates the slope of a line segment defined by two points.
    /// </summary>
    /// <param name="p1">The first point of the line segment.</param>
    /// <param name="p2">The second point of the line segment.</param>
    /// <returns>The slope of the line segment.</returns>
    public static float Earring(Point p1, Point p2)
    {
        return (p2.X - p1.X) / (p2.Y - p1.Y);
    }

    /// <summary>
    /// Calculates the intercepts of a line passing through two points.
    /// </summary>
    /// <param name="p1">The first point on the line.</param>
    /// <param name="p2">The second point on the line.</param>
    /// <returns>An array of Point objects representing the intercepts.</returns>
    public static Point[] Intercepts(Point p1, Point p2)
    {
        float m = Earring(p1, p2);
        float n = p1.Y - m * p1.X;
        float cero = -n / m;
        return new Point[] { new(0, (int)n), new((int)cero, 0) };
    }
    #endregion
}
