
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
                    this.Width = 280 + groupBoxMap.Width;

                    //Default colors
                    if ((j + i) % 2 == 0)
                    {
                        tempTile.BackColor = Color.White;
                        tempTile.Tag = "-1";
                    }
                    else
                    {
                        tempTile.BackColor = Color.LightGray;
                        tempTile.Tag = "-2";
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

            tile.Tag = tileCurrent.Tag;
            tile.BackgroundImage = new Bitmap(tileCurrent.BackgroundImage!,
                new Size(tile.Width, tile.Height));
        }

        /// <summary>
        /// Sets the user's current tile to the one they click on
        /// </summary>
        private void TileSelect_Click(object sender, EventArgs e)
        {
            PictureBox tile = (PictureBox)sender;

            tileCurrent.Tag = tile.Tag;
            tileCurrent.BackgroundImage = new Bitmap(tile.BackgroundImage!,
                new Size(tile.Width, tile.Height));
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
                        PictureBox currentBox = _tileList[i];

                        output.WriteLine(currentBox.Tag);
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
                    string readInput = input.ReadLine()!;
                    switch (readInput)
                    {
                        case "0":
                            _tileList[i].BackgroundImage = new Bitmap(Properties.Resources.test0,
                            new Size(_tileList[i].Width, _tileList[i].Height));
                            _tileList[i].Tag = "0";
                            break;
                        case "1":
                            _tileList[i].BackgroundImage = new Bitmap(Properties.Resources.test1,
                            new Size(_tileList[i].Width, _tileList[i].Height));
                            _tileList[i].Tag = "1";
                            break;
                        case "2":
                            _tileList[i].BackgroundImage = new Bitmap(Properties.Resources.test2,
                            new Size(_tileList[i].Width, _tileList[i].Height));
                            _tileList[i].Tag = "2";
                            break;
                        case "3":
                            _tileList[i].BackgroundImage = new Bitmap(Properties.Resources.test3,
                            new Size(_tileList[i].Width, _tileList[i].Height));
                            _tileList[i].Tag = "3";
                            break;
                        case "4":
                            _tileList[i].BackgroundImage = new Bitmap(Properties.Resources.test4,
                            new Size(_tileList[i].Width, _tileList[i].Height));
                            _tileList[i].Tag = "4";
                            break;
                        case "5":
                            _tileList[i].BackgroundImage = new Bitmap(Properties.Resources.test5,
                            new Size(_tileList[i].Width, _tileList[i].Height));
                            _tileList[i].Tag = "5";
                            break;
                        case "6":
                            _tileList[i].BackgroundImage = new Bitmap(Properties.Resources.test6,
                            new Size(_tileList[i].Width, _tileList[i].Height));
                            _tileList[i].Tag = "6";
                            break;
                        case "7":
                            _tileList[i].BackgroundImage = new Bitmap(Properties.Resources.test7,
                            new Size(_tileList[i].Width, _tileList[i].Height));
                            _tileList[i].Tag = "7";
                            break;
                        case "8":
                            _tileList[i].BackgroundImage = new Bitmap(Properties.Resources.test8,
                            new Size(_tileList[i].Width, _tileList[i].Height));
                            _tileList[i].Tag = "8";
                            break;
                        case "9":
                            _tileList[i].BackgroundImage = new Bitmap(Properties.Resources.test9,
                            new Size(_tileList[i].Width, _tileList[i].Height));
                            _tileList[i].Tag = "9";
                            break;
                        case "10":
                            _tileList[i].BackgroundImage = new Bitmap(Properties.Resources.test10,
                            new Size(_tileList[i].Width, _tileList[i].Height));
                            _tileList[i].Tag = "10";
                            break;
                        case "11":
                            _tileList[i].BackgroundImage = new Bitmap(Properties.Resources.test11,
                            new Size(_tileList[i].Width, _tileList[i].Height));
                            _tileList[i].Tag = "11";
                            break;
                        case "12":
                            _tileList[i].BackgroundImage = new Bitmap(Properties.Resources.test12,
                            new Size(_tileList[i].Width, _tileList[i].Height));
                            _tileList[i].Tag = "12";
                            break;
                        case "13":
                            _tileList[i].BackgroundImage = new Bitmap(Properties.Resources.test13,
                            new Size(_tileList[i].Width, _tileList[i].Height));
                            _tileList[i].Tag = "13";
                            break;
                        case "14":
                            _tileList[i].BackgroundImage = new Bitmap(Properties.Resources.test14,
                            new Size(_tileList[i].Width, _tileList[i].Height));
                            _tileList[i].Tag = "14";
                            break;
                        case "15":
                            _tileList[i].BackgroundImage = new Bitmap(Properties.Resources.test15,
                            new Size(_tileList[i].Width, _tileList[i].Height));
                            _tileList[i].Tag = "15";
                            break;
                        case "16":
                            _tileList[i].BackgroundImage = new Bitmap(Properties.Resources.groundV1,
                            new Size(_tileList[i].Width, _tileList[i].Height));
                            _tileList[i].Tag = "16";
                            break;
                    }  
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
