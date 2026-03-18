//Laurin Zanon, 2/3/2026, Level editor using forms
namespace HW2_LevelEditor
{
    public partial class Form1 : Form
    {
        //Fields
        private Form2? mapEditor;
        private OpenFileDialog fileOpener;

        /// <summary>
        /// Constructst the form
        /// </summary>
        public Form1()
        {
            fileOpener = new OpenFileDialog();
            InitializeComponent();
        }

        //Methods

        /// <summary>
        /// Creates a new map using the values in the text boxes (after verifying them)
        /// </summary>
        private void ButtonCreate_Click(object sender, EventArgs e)
        {
            //Run checks to validate inputs before creating a new map
            int checks = 0;
            string errMessage = "Errors:";

            //Check 1: Width
            if (int.TryParse(textBoxWidth.Text, out int resultW) && resultW >= 10 && resultW <= 30)
            {
                checks++;
            }
            else
            {
                errMessage += "\n - Invalid width. Range is 10 - 30 tiles.";
            }

            //Check 2: height
            if (int.TryParse(textBoxHeight.Text, out int resultH) && resultH >= 10 && resultH <= 30)
            {
                checks++;
            }
            else
            {
                errMessage += "\n - Invalid Height. Range is 10 - 30 tiles.";
            }

            //Check checks
            if (checks == 2)
            {
                //Success - Open map editor with width and height given
                mapEditor = new Form2(resultW, resultH);
                mapEditor.ShowDialog();
            }
            else
            {
                //Error - Display which field is incorrect
                MessageBox.Show(errMessage, "Error creating map", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Opens file dialog to loet user pick map to load
        /// </summary>
        private void ButtonLoad_Click(object sender, EventArgs e)
        {
            //File opener window
            fileOpener.Title = "Open a level file";
            fileOpener.Filter = "Level Files|*.level";
            DialogResult fileStatus = fileOpener.ShowDialog();

            //Checks if user actually picked a file
            if (fileStatus == DialogResult.OK)
            {
                //User chose file, send filename to second form
                mapEditor = new Form2(fileOpener.FileName);
                mapEditor.ShowDialog();
            }
        }
    }
}
