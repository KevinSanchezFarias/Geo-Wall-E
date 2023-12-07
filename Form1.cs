namespace WaLI;

public partial class MainForm : Form
{
    readonly TextBox textBox;
    readonly Button button;
    readonly Label resultLabel;

    /// <summary>
    /// Represents the main form of the application.
    /// </summary>
    public MainForm()
    {
        InitializeComponent();

        // Initialize the TextBox and add it to the Form's controls
        textBox = new TextBox
        {
            Location = new Point(10, 10), // Set the location of the TextBox
            Size = new Size(200, 20) // Set the size of the TextBox
        };
        this.Controls.Add(textBox);

        // Initialize the Button, set its text, and add it to the Form's controls
        button = new Button
        {
            Text = "Submit",
            Location = new Point(10, 40), // Set the location of the Button
            Size = new Size(100, 20) // Set the size of the Button
        };
        this.Controls.Add(button);

        // Initialize the Label and add it to the Form's controls
        resultLabel = new Label
        {
            Location = new Point(10, 70) // Set the location of the Label
        };
        this.Controls.Add(resultLabel);

        // Attach an event handler to the Button's Click event
        button.Click += Button_Click!;
    }
    /// <summary>
    /// Event handler for the button click event.
    /// Calls the GSharp function with the text from the TextBox and sets the Text property of the Label to the result.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">The event arguments.</param>
    private void Button_Click(object sender, EventArgs e)
    {
        // Call the GSharp function with the text from the TextBox
        object result = MiddleEnd.GSharp(textBox.Text);

        // Set the Text property of the Label to the result
        resultLabel.Text = result.ToString();
    }
}