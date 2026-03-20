namespace HW2_LevelEditor
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            pictureBox11 = new PictureBox();
            pictureBox3 = new PictureBox();
            pictureBox4 = new PictureBox();
            pictureBox5 = new PictureBox();
            pictureBox6 = new PictureBox();
            pictureBox7 = new PictureBox();
            pictureBox8 = new PictureBox();
            pictureBox9 = new PictureBox();
            pictureBox10 = new PictureBox();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            tileGray = new PictureBox();
            tileBrown = new PictureBox();
            tileBlue = new PictureBox();
            tileGreen = new PictureBox();
            tileTan = new PictureBox();
            tileRed = new PictureBox();
            groupBox2 = new GroupBox();
            tileCurrent = new PictureBox();
            buttonSave = new Button();
            buttonLoad = new Button();
            groupBoxMap = new GroupBox();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox11).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox9).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox10).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tileGray).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tileBrown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tileBlue).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tileGreen).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tileTan).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tileRed).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tileCurrent).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(pictureBox11);
            groupBox1.Controls.Add(pictureBox3);
            groupBox1.Controls.Add(pictureBox4);
            groupBox1.Controls.Add(pictureBox5);
            groupBox1.Controls.Add(pictureBox6);
            groupBox1.Controls.Add(pictureBox7);
            groupBox1.Controls.Add(pictureBox8);
            groupBox1.Controls.Add(pictureBox9);
            groupBox1.Controls.Add(pictureBox10);
            groupBox1.Controls.Add(pictureBox1);
            groupBox1.Controls.Add(pictureBox2);
            groupBox1.Controls.Add(tileGray);
            groupBox1.Controls.Add(tileBrown);
            groupBox1.Controls.Add(tileBlue);
            groupBox1.Controls.Add(tileGreen);
            groupBox1.Controls.Add(tileTan);
            groupBox1.Controls.Add(tileRed);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(229, 425);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Tile Selector";
            // 
            // pictureBox11
            // 
            pictureBox11.BackColor = Color.Transparent;
            pictureBox11.BackgroundImage = Properties.Resources.groundV1;
            pictureBox11.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox11.Location = new Point(6, 249);
            pictureBox11.Name = "pictureBox11";
            pictureBox11.Size = new Size(50, 50);
            pictureBox11.TabIndex = 16;
            pictureBox11.TabStop = false;
            pictureBox11.Tag = "16";
            pictureBox11.Click += TileSelect_Click;
            // 
            // pictureBox3
            // 
            pictureBox3.BackColor = Color.Transparent;
            pictureBox3.BackgroundImage = Properties.Resources.cornerSEV0;
            pictureBox3.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox3.Location = new Point(174, 193);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(50, 50);
            pictureBox3.TabIndex = 15;
            pictureBox3.TabStop = false;
            pictureBox3.Tag = "15";
            pictureBox3.Click += TileSelect_Click;
            // 
            // pictureBox4
            // 
            pictureBox4.BackColor = Color.Transparent;
            pictureBox4.BackgroundImage = Properties.Resources.cornerSWV2;
            pictureBox4.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox4.Location = new Point(118, 193);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(50, 50);
            pictureBox4.TabIndex = 14;
            pictureBox4.TabStop = false;
            pictureBox4.Tag = "14";
            pictureBox4.Click += TileSelect_Click;
            // 
            // pictureBox5
            // 
            pictureBox5.BackColor = Color.Transparent;
            pictureBox5.BackgroundImage = Properties.Resources.cornerNEV0;
            pictureBox5.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox5.Location = new Point(62, 193);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(50, 50);
            pictureBox5.TabIndex = 13;
            pictureBox5.TabStop = false;
            pictureBox5.Tag = "13";
            pictureBox5.Click += TileSelect_Click;
            // 
            // pictureBox6
            // 
            pictureBox6.BackColor = Color.Transparent;
            pictureBox6.BackgroundImage = Properties.Resources.wallW2V0;
            pictureBox6.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox6.Location = new Point(173, 137);
            pictureBox6.Name = "pictureBox6";
            pictureBox6.Size = new Size(50, 50);
            pictureBox6.TabIndex = 12;
            pictureBox6.TabStop = false;
            pictureBox6.Tag = "11";
            pictureBox6.Click += TileSelect_Click;
            // 
            // pictureBox7
            // 
            pictureBox7.BackColor = Color.Transparent;
            pictureBox7.BackgroundImage = Properties.Resources.wallW0V0;
            pictureBox7.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox7.Location = new Point(62, 137);
            pictureBox7.Name = "pictureBox7";
            pictureBox7.Size = new Size(50, 50);
            pictureBox7.TabIndex = 11;
            pictureBox7.TabStop = false;
            pictureBox7.Tag = "9";
            pictureBox7.Click += TileSelect_Click;
            // 
            // pictureBox8
            // 
            pictureBox8.BackColor = Color.Transparent;
            pictureBox8.BackgroundImage = Properties.Resources.cornerNWV0;
            pictureBox8.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox8.Location = new Point(6, 193);
            pictureBox8.Name = "pictureBox8";
            pictureBox8.Size = new Size(50, 50);
            pictureBox8.TabIndex = 10;
            pictureBox8.TabStop = false;
            pictureBox8.Tag = "12";
            pictureBox8.Click += TileSelect_Click;
            // 
            // pictureBox9
            // 
            pictureBox9.BackColor = Color.Transparent;
            pictureBox9.BackgroundImage = Properties.Resources.wallW1V0;
            pictureBox9.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox9.Location = new Point(118, 137);
            pictureBox9.Name = "pictureBox9";
            pictureBox9.Size = new Size(50, 50);
            pictureBox9.TabIndex = 9;
            pictureBox9.TabStop = false;
            pictureBox9.Tag = "10";
            pictureBox9.Click += TileSelect_Click;
            // 
            // pictureBox10
            // 
            pictureBox10.BackColor = Color.Transparent;
            pictureBox10.BackgroundImage = Properties.Resources.wallS2V0;
            pictureBox10.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox10.Location = new Point(6, 137);
            pictureBox10.Name = "pictureBox10";
            pictureBox10.Size = new Size(50, 50);
            pictureBox10.TabIndex = 8;
            pictureBox10.TabStop = false;
            pictureBox10.Tag = "8";
            pictureBox10.Click += TileSelect_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.BackgroundImage = Properties.Resources.wallS1V0;
            pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox1.Location = new Point(174, 81);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(50, 50);
            pictureBox1.TabIndex = 7;
            pictureBox1.TabStop = false;
            pictureBox1.Tag = "7";
            pictureBox1.Click += TileSelect_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.BackgroundImage = Properties.Resources.wallS0V0;
            pictureBox2.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox2.Location = new Point(118, 81);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(50, 50);
            pictureBox2.TabIndex = 6;
            pictureBox2.TabStop = false;
            pictureBox2.Tag = "6";
            pictureBox2.Click += TileSelect_Click;
            // 
            // tileGray
            // 
            tileGray.BackColor = Color.Transparent;
            tileGray.BackgroundImage = Properties.Resources.wallE2V0;
            tileGray.BackgroundImageLayout = ImageLayout.Zoom;
            tileGray.Location = new Point(62, 81);
            tileGray.Name = "tileGray";
            tileGray.Size = new Size(50, 50);
            tileGray.TabIndex = 5;
            tileGray.TabStop = false;
            tileGray.Tag = "5";
            tileGray.Click += TileSelect_Click;
            // 
            // tileBrown
            // 
            tileBrown.BackColor = Color.Transparent;
            tileBrown.BackgroundImage = Properties.Resources.wallE0V0;
            tileBrown.BackgroundImageLayout = ImageLayout.Zoom;
            tileBrown.Location = new Point(173, 25);
            tileBrown.Name = "tileBrown";
            tileBrown.Size = new Size(50, 50);
            tileBrown.TabIndex = 4;
            tileBrown.TabStop = false;
            tileBrown.Tag = "3";
            tileBrown.Click += TileSelect_Click;
            // 
            // tileBlue
            // 
            tileBlue.BackColor = Color.Transparent;
            tileBlue.BackgroundImage = Properties.Resources.wallN1V0;
            tileBlue.BackgroundImageLayout = ImageLayout.Zoom;
            tileBlue.Location = new Point(62, 25);
            tileBlue.Name = "tileBlue";
            tileBlue.Size = new Size(50, 50);
            tileBlue.TabIndex = 3;
            tileBlue.TabStop = false;
            tileBlue.Tag = "1";
            tileBlue.Click += TileSelect_Click;
            // 
            // tileGreen
            // 
            tileGreen.BackColor = Color.Transparent;
            tileGreen.BackgroundImage = Properties.Resources.wallE1V0;
            tileGreen.BackgroundImageLayout = ImageLayout.Zoom;
            tileGreen.Location = new Point(6, 81);
            tileGreen.Name = "tileGreen";
            tileGreen.Size = new Size(50, 50);
            tileGreen.TabIndex = 2;
            tileGreen.TabStop = false;
            tileGreen.Tag = "4";
            tileGreen.Click += TileSelect_Click;
            // 
            // tileTan
            // 
            tileTan.BackColor = Color.Transparent;
            tileTan.BackgroundImage = Properties.Resources.wallN2V0;
            tileTan.BackgroundImageLayout = ImageLayout.Zoom;
            tileTan.Location = new Point(118, 25);
            tileTan.Name = "tileTan";
            tileTan.Size = new Size(50, 50);
            tileTan.TabIndex = 1;
            tileTan.TabStop = false;
            tileTan.Tag = "2";
            tileTan.Click += TileSelect_Click;
            // 
            // tileRed
            // 
            tileRed.BackColor = Color.Transparent;
            tileRed.BackgroundImage = Properties.Resources.wallN0V0;
            tileRed.BackgroundImageLayout = ImageLayout.Zoom;
            tileRed.Location = new Point(6, 25);
            tileRed.Name = "tileRed";
            tileRed.Size = new Size(50, 50);
            tileRed.TabIndex = 0;
            tileRed.TabStop = false;
            tileRed.Tag = "0";
            tileRed.Click += TileSelect_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(tileCurrent);
            groupBox2.Location = new Point(104, 443);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(137, 153);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Current Tile";
            // 
            // tileCurrent
            // 
            tileCurrent.BackgroundImageLayout = ImageLayout.Zoom;
            tileCurrent.Location = new Point(23, 38);
            tileCurrent.Name = "tileCurrent";
            tileCurrent.Size = new Size(86, 86);
            tileCurrent.TabIndex = 6;
            tileCurrent.TabStop = false;
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(12, 443);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(86, 77);
            buttonSave.TabIndex = 2;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += ButtonSave_Click;
            // 
            // buttonLoad
            // 
            buttonLoad.Location = new Point(12, 533);
            buttonLoad.Name = "buttonLoad";
            buttonLoad.Size = new Size(86, 77);
            buttonLoad.TabIndex = 3;
            buttonLoad.Text = "Load";
            buttonLoad.UseVisualStyleBackColor = true;
            buttonLoad.Click += ButtonLoad_Click;
            // 
            // groupBoxMap
            // 
            groupBoxMap.Location = new Point(247, 12);
            groupBoxMap.Name = "groupBoxMap";
            groupBoxMap.Size = new Size(651, 598);
            groupBoxMap.TabIndex = 4;
            groupBoxMap.TabStop = false;
            groupBoxMap.Text = "Map";
            // 
            // Form2
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(910, 622);
            Controls.Add(groupBoxMap);
            Controls.Add(buttonLoad);
            Controls.Add(buttonSave);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form2";
            Text = "Level Editor";
            FormClosing += Form2_FormClosing;
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox11).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox9).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox10).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)tileGray).EndInit();
            ((System.ComponentModel.ISupportInitialize)tileBrown).EndInit();
            ((System.ComponentModel.ISupportInitialize)tileBlue).EndInit();
            ((System.ComponentModel.ISupportInitialize)tileGreen).EndInit();
            ((System.ComponentModel.ISupportInitialize)tileTan).EndInit();
            ((System.ComponentModel.ISupportInitialize)tileRed).EndInit();
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)tileCurrent).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Button buttonSave;
        private Button buttonLoad;
        private GroupBox groupBoxMap;
        private PictureBox tileGray;
        private PictureBox tileBrown;
        private PictureBox tileBlue;
        private PictureBox tileGreen;
        private PictureBox tileTan;
        private PictureBox tileRed;
        private PictureBox tileCurrent;
        private PictureBox pictureBox11;
        private PictureBox pictureBox3;
        private PictureBox pictureBox4;
        private PictureBox pictureBox5;
        private PictureBox pictureBox6;
        private PictureBox pictureBox7;
        private PictureBox pictureBox8;
        private PictureBox pictureBox9;
        private PictureBox pictureBox10;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
    }
}