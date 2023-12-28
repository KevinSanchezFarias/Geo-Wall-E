using EvaluatorAnalize;
using WaLI.backend;
namespace FormsTest1;

public partial class MainForm : Form
{
    private TextBox textBox1;
    private readonly Button button1 = new();
    private readonly Button button2 = new();
    private readonly PictureBox panel = new();
    private TextBox errorTextBox;


    /// <summary>
    /// Initializes a new instance of the <see cref="MainForm"/> class.
    /// </summary>
    public MainForm()
    {
        textBox1 = new();
        errorTextBox = new();
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
        button2.Location = new Point(400, (int)(Height * 0.875));
        button1.Location = new Point(150, (int)(Height * 0.875));
        textBox1.Location = new Point(ClientSize.Width - (ClientSize.Width / 4), 0);
        textBox1.Width = (int)(ClientSize.Width * 0.3);
        textBox1.Height = ClientSize.Height;
        textBox1.BackColor = ColorTranslator.FromHtml("#181926");
        textBox1.ForeColor = ColorTranslator.FromHtml("#ffffff");
        textBox1.PlaceholderText = "Enter your instructions";
        textBox1.Font = new Font("Arial", 14);
        button1.BringToFront();
        button2.BringToFront();
    }
    /// <summary>
    /// Adjusts the size of the dynamic canvas based on the client size.
    /// </summary>
    private void AdjustDynamicCanvasSize()
    {
        int canvasWidth = ClientSize.Width;
        int canvasHeight = ClientSize.Height;
        panel.Size = new Size(canvasWidth, (int)(canvasHeight * 0.92));
        panel.Location = new Point((ClientSize.Width - canvasWidth) / 2, 0);
    }
    /// <summary>
    /// Handles the event when the submit button is clicked.
    /// </summary>
    /// <param name="sender">The object that triggered the event.</param>
    /// <param name="e">The event arguments.</param>
    private void SubmitCommands(object sender, EventArgs e)
    {
        Graphics canvas = panel.CreateGraphics();

        foreach (var graphics in MiddleEnd.GSharp(textBox1.Text))
        {
            if (graphics is string errorMessage)
            {
                ShowErrorMessage(errorMessage);
            }
            else if (graphics is ToDraw graphic)
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
                        break;
                }
            }
            else
            {
                MessageBox.Show("Could not graph", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    // Modify your ShowErrorMessage method to use the TextBox
    private void ShowErrorMessage(string errorMessage)
    {
        errorTextBox.AppendText(errorMessage + Environment.NewLine); // Append the new error message
        errorTextBox.Font = new Font("Arial", 14); // Set the font size to 14
        errorTextBox.Visible = true;
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
    /// <param name="point">The coordinates of the point.</param>
    /// <param name="comment">The comment to be displayed next to the point.</param>
    /// <param name="canvas">The graphics object representing the canvas.</param>
    private static void Draw_Point(Brush brush, PointF point, string comment, Graphics canvas)
    {
        canvas.FillEllipse(brush, point.X, point.Y, 7, 7);
        canvas.DrawString(comment, new Font("Arial", 12), Brushes.Black, point.X, point.Y);
    }
    /// <summary>
    /// Draws an arc on the specified canvas using the provided brush, points, and comment.
    /// </summary>
    /// <param name="brush">The brush used to fill the arc.</param>
    /// <param name="points">An array of points that define the arc.</param>
    /// <param name="comment">The comment to be displayed.</param>
    /// <param name="canvas">The graphics canvas on which the arc will be drawn.</param>
    /// <exception cref="ArgumentException">Thrown when less than 3 points are provided.</exception>
    private static void Draw_Arc(Brush brush, PointF[] points, string comment, Graphics canvas)
    {
        // Ensure that we have at least 3 points
        if (points.Length < 3)
        {
            throw new ArgumentException("At least 3 points are required to draw an arc.");
        }
        // Calculate the bounding rectangle for the arc
        var x = Math.Min(points[0].X, Math.Min(points[1].X, points[2].X));
        var y = Math.Min(points[0].Y, Math.Min(points[1].Y, points[2].Y));
        var width = Math.Max(points[0].X, Math.Max(points[1].X, points[2].X)) - x;
        var height = Math.Max(points[0].Y, Math.Max(points[1].Y, points[2].Y)) - y;

        // Calculate the start and sweep angles
        var startAngle = Convert.ToSingle(Math.Atan2(points[0].Y - points[1].Y, points[0].X - points[1].X) * 180 / Math.PI);
        var endAngle = Convert.ToSingle(Math.Atan2(points[0].Y - points[2].Y, points[0].X - points[2].X) * 180 / Math.PI);

        // Draw the arc
        canvas.DrawArc(new Pen(brush), x, y, width, height, startAngle, endAngle - startAngle);

        // Draw the comment
        canvas.DrawString(comment, new Font("Arial", 12), Brushes.White, points[0].X, points[0].Y);
    }

    /// <summary>
    /// Draws a circle on the specified canvas using the provided brush, center point, radius, comment, and graphics object.
    /// </summary>
    /// <param name="brush">The brush used to fill the circle.</param>
    /// <param name="point">The center point of the circle.</param>
    /// <param name="radio">The radius of the circle.</param>
    /// <param name="comment">The comment to be displayed near the circle.</param>
    /// <param name="canvas">The graphics object representing the canvas on which the circle will be drawn.</param>
    private static void Draw_Circle(Brush brush, PointF point, double radio, string comment, Graphics canvas)
    {
        var diameter = (int)(radio * 2);
        var x = point.X - (diameter / 2);
        var y = point.Y - (diameter / 2);

        canvas.DrawEllipse(new Pen(brush), x, y, diameter, diameter);
        canvas.DrawString(comment, new Font("Arial", 12), Brushes.White, point.X, point.Y);
    }
    /// <summary>
    /// Draws a segment on the canvas using the specified brush, points, comment, and graphics object.
    /// </summary>
    /// <param name="brush">The brush used to draw the segment.</param>
    /// <param name="points">The array of points that define the segment.</param>
    /// <param name="comment">The comment to be displayed near the starting point of the segment.</param>
    /// <param name="canvas">The graphics object representing the canvas.</param>
    private static void Draw_Segment(Brush brush, PointF[] points, string comment, Graphics canvas)
    {
        foreach (PointF point in points)
        {
            Draw_Point(brush, point, "", canvas);
        }
        canvas.DrawLine(new Pen(brush), points[0], points[1]);
        canvas.DrawString(comment, new Font("Arial", 12), Brushes.Black, points[0].X, points[0].Y);
    }
    /// <summary>
    /// Draws a line connecting the given points on the canvas using the specified brush.
    /// </summary>
    /// <param name="brush">The brush used to draw the line.</param>
    /// <param name="points">An array of points that define the line.</param>
    /// <param name="comment">A comment to be displayed near the line.</param>
    /// <param name="canvas">The graphics canvas on which the line is drawn.</param>
    private static void Draw_Line(Brush brush, PointF[] points, string comment, Graphics canvas)
    {
        foreach (PointF point in points)
        {
            Draw_Point(brush, point, "", canvas);
        }

        // Calculate the slope of the line
        var slope = (points[1].Y - points[0].Y) / (points[1].X - points[0].X);

        // Calculate the y-intercept of the line
        var yIntercept = points[0].Y - slope * points[0].X;

        // Calculate the points where the line intersects the edges of the canvas
        PointF leftPoint = new(0, yIntercept);
        PointF rightPoint = new(canvas.VisibleClipBounds.Width, slope * canvas.VisibleClipBounds.Width + yIntercept);

        // Draw the line
        canvas.DrawLine(new Pen(brush), leftPoint, rightPoint);

        // Draw the comment
        canvas.DrawString(comment, new Font("Arial", 12), Brushes.Black, points[0].X, points[0].Y);
    }
    /// <summary>
    /// Draws a ray on the canvas from the starting point to the point where it intersects the canvas boundary.
    /// </summary>
    /// <param name="brush">The brush used to draw the ray.</param>
    /// <param name="points">An array of points representing the starting and ending points of the ray.</param>
    /// <param name="comment">A comment to be displayed at the starting point of the ray.</param>
    /// <param name="canvas">The graphics canvas on which the ray will be drawn.</param>
    private static void Draw_Ray(Brush brush, PointF[] points, string comment, Graphics canvas)
    {
        foreach (PointF point in points)
        {
            Draw_Point(brush, point, "", canvas);
        }

        // Calculate the slope of the line
        var slope = (points[1].Y - points[0].Y) / (points[1].X - points[0].X);

        // Calculate the y-intercept of the line
        var yIntercept = points[0].Y - slope * points[0].X;

        // Calculate the point where the line intersects the right edge of the canvas
        PointF rightPoint = new(canvas.VisibleClipBounds.Width, slope * canvas.VisibleClipBounds.Width + yIntercept);

        // Draw the line
        canvas.DrawLine(new Pen(brush), points[0], rightPoint);

        // Draw the comment
        canvas.DrawString(comment, new Font("Arial", 12), Brushes.Black, points[0].X, points[0].Y);
    }
    #endregion
}
