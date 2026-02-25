
namespace HW2_LevelEditor
{
    public partial class Form2 : Form
    {
        //Fields
        private int _width;
        private int _height;
        private string? _fileName;
        private List<PictureBox>? _tileList;
        private bool _unsavedChanges;

        /// <summary>
        /// Constructs a form for editing a completely new map
        /// </summary>
        /// <param name="width">Width of new map</param>
        /// <param name="height">Height of new map</param>
        public Form2(int width, int height)
        {
            this._width = width;
            this._height = height;
            _tileList = new List<PictureBox>();
            _unsavedChanges = false;

            InitializeComponent();

            //Prompts the method that creates the grid
            CreateMap();
        }

        /// <summary>
        /// Constructs a form for editing an existing map
        /// </summary>
        /// <param name="fileName">Name of map to edit</param>
        public Form2(string fileName)
        {
            this._fileName = fileName;
            _tileList = new List<PictureBox>();
            _unsavedChanges = false;

            InitializeComponent();

            //Call the loadFile method here as this constructor is only called when the user requests to load a file
            LoadFile(fileName);
        }

        //Methods

        /// <summary>
        /// Creates and displays grid of picture tiles
        /// </summary>
        public void CreateMap()
        {
            //Dispose of all existing tiles and empty list first
            for (int i = 0; i < _tileList!.Count; i++)
            {
                _tileList[i].Dispose();
            }
            _tileList!.Clear();

            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    //Temporary tile to place in list
                    PictureBox tempTile = new PictureBox();
                    tempTile.Height = 550 / _height;
                    tempTile.Width = tempTile.Height;

                    //Rescale window based on width
                    groupBoxMap.Width = tempTile.Width * _width + 28;
                    this.Width = 202 + groupBoxMap.Width;

                    //Default colors
                    if ((j + i) % 2 == 0)
                    {
                        tempTile.BackColor = Color.White;
                    }
                    else
                    {
                        tempTile.BackColor = Color.LightGray;
                    }

                    //Adding tile to groupBox and list
                    tempTile.Click += TileMap_Click!;
                    groupBoxMap.Controls.Add(tempTile);
                    Point newPosition = new Point(i * tempTile.Width + 14, j * tempTile.Height + 26);
                    tempTile.Location = newPosition;
                    _tileList!.Add(tempTile);
                    _tileList[j + Math.Abs(_width - Math.Abs(_width - _height)) * i].Show();
                }
            }
        }

        /// <summary>
        /// Sets color of clicked tile to that of the currently selected tile color
        /// </summary>
        private void TileMap_Click(object sender, EventArgs e)
        {
            PictureBox tile = (PictureBox)sender;

            //Unsaved changes
            if (_unsavedChanges == false && tileCurrent.BackColor != tile.BackColor)
            {
                _unsavedChanges = true;
                this.Text += "*";
            }

            tile.BackColor = tileCurrent.BackColor;
        }

        /// <summary>
        /// Sets the user's current tile to the one they click on
        /// </summary>
        private void TileSelect_Click(object sender, EventArgs e)
        {
            PictureBox tile = (PictureBox)sender;

            tileCurrent.BackColor = tile.BackColor;
        }

        /// <summary>
        /// Saves map by reading in width, height, and color of every tile
        /// </summary>
        private void ButtonSave_Click(object sender, EventArgs e)
        {
            //Create file saver
            SaveFileDialog fileSaver = new SaveFileDialog();
            fileSaver.Filter = "Level Files|*.level";
            fileSaver.Title = "Save a level file";

            //Get result and proceed
            DialogResult savedFileStatus = fileSaver.ShowDialog();

            if (savedFileStatus == DialogResult.OK)
            {
                //Save file
                StreamWriter output = null!;

                try
                {
                    output = new StreamWriter(fileSaver.FileName);

                    //Write width and height first
                    output.WriteLine(_width.ToString());
                    output.WriteLine(_height.ToString());

                    //Loop through list and write all colors, one per line
                    for (int i = 0; i < _tileList!.Count; i++)
                    {
                        output.WriteLine(_tileList[i].BackColor.ToArgb());
                    }

                    //Success message
                    MessageBox.Show("File saved successfully", "File saved",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Text = $"Level Editor - {fileSaver.FileName.Substring(fileSaver.FileName.LastIndexOf('\\') + 1)}";
                    _unsavedChanges = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex}", "ERROR",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (output != null)
                    {
                        output.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Loads map by getting a filename and then calling LoadFile()
        /// </summary>
        private void ButtonLoad_Click(object sender, EventArgs e)
        {
            //Create file opener
            OpenFileDialog fileOpener = new OpenFileDialog();
            fileOpener.Filter = "Level Files|*.level";
            fileOpener.Title = "Load a level file";

            //Get result and proceed
            DialogResult openedFileStatus = fileOpener.ShowDialog();

            if (openedFileStatus == DialogResult.OK)
            {
                LoadFile(fileOpener.FileName);
            }
        }

        /// <summary>
        /// Loads the given file, which contains width, height, and all values to
        /// be placed in mapGrid array
        /// </summary>
        /// <param name="fileName">File to load</param>
        public void LoadFile(string fileName)
        {
            StreamReader input = null!;

            try
            {
                input = new StreamReader(fileName);

                //Call CreateMap() to set up map before repainting
                _width = int.Parse(input.ReadLine()!);
                _height = int.Parse(input.ReadLine()!);
                CreateMap();

                //Loop and paint
                for (int i = 0; i < _tileList!.Count; i++)
                {
                    _tileList[i].BackColor = Color.FromArgb(int.Parse(input.ReadLine()!));
                }

                //Success message
                MessageBox.Show("File loaded successfully", "File loaded",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Text = $"Level Editor - {fileName.Substring(fileName.LastIndexOf('\\') + 1)}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex}", "ERROR",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            finally
            {
                if (input != null)
                {
                    input.Close();
                }
            }
        }

        /// <summary>
        /// Prompts user about saving changes before closing
        /// </summary>
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_unsavedChanges)
            {
                DialogResult closeResult = MessageBox.Show("There are unsaved changes. Are you sure you want to quit?", "Unsaved changes",
                    MessageBoxButtons.YesNo,MessageBoxIcon.Warning);

                if (closeResult == DialogResult.No)
                {
                    //File doesn't close
                    e.Cancel = true;
                }

                //Otherwise, file closes as normal
            }
        }
    }
}
