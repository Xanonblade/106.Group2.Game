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
            groupBox1.Controls.Add(tileGray);
            groupBox1.Controls.Add(tileBrown);
            groupBox1.Controls.Add(tileBlue);
            groupBox1.Controls.Add(tileGreen);
            groupBox1.Controls.Add(tileTan);
            groupBox1.Controls.Add(tileRed);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(153, 237);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Tile Selector";
            // 
            // tileGray
            // 
            tileGray.BackColor = Color.Gray;
            tileGray.Location = new Point(79, 163);
            tileGray.Name = "tileGray";
            tileGray.Size = new Size(62, 62);
            tileGray.TabIndex = 5;
            tileGray.TabStop = false;
            tileGray.Tag = "5";
            tileGray.Click += TileSelect_Click;
            // 
            // tileBrown
            // 
            tileBrown.BackColor = Color.Sienna;
            tileBrown.Location = new Point(79, 95);
            tileBrown.Name = "tileBrown";
            tileBrown.Size = new Size(62, 62);
            tileBrown.TabIndex = 4;
            tileBrown.TabStop = false;
            tileBrown.Tag = "3";
            tileBrown.Click += TileSelect_Click;
            // 
            // tileBlue
            // 
            tileBlue.BackColor = Color.Transparent;
            tileBlue.BackgroundImage = Properties.Resources.Ritchie;
            tileBlue.Location = new Point(79, 27);
            tileBlue.Name = "tileBlue";
            tileBlue.Size = new Size(62, 62);
            tileBlue.TabIndex = 3;
            tileBlue.TabStop = false;
            tileBlue.Tag = "1";
            tileBlue.Click += TileSelect_Click;
            // 
            // tileGreen
            // 
            tileGreen.BackColor = Color.YellowGreen;
            tileGreen.Location = new Point(11, 163);
            tileGreen.Name = "tileGreen";
            tileGreen.Size = new Size(62, 62);
            tileGreen.TabIndex = 2;
            tileGreen.TabStop = false;
            tileGreen.Tag = "4";
            tileGreen.Click += TileSelect_Click;
            // 
            // tileTan
            // 
            tileTan.BackColor = Color.Transparent;
            tileTan.BackgroundImage = Properties.Resources.Mario;
            tileTan.Location = new Point(11, 95);
            tileTan.Name = "tileTan";
            tileTan.Size = new Size(62, 62);
            tileTan.TabIndex = 1;
            tileTan.TabStop = false;
            tileTan.Tag = "2";
            tileTan.Click += TileSelect_Click;
            // 
            // tileRed
            // 
            tileRed.BackColor = Color.Transparent;
            tileRed.BackgroundImage = Properties.Resources.coin;
            tileRed.Location = new Point(11, 27);
            tileRed.Name = "tileRed";
            tileRed.Size = new Size(62, 62);
            tileRed.TabIndex = 0;
            tileRed.TabStop = false;
            tileRed.Tag = "0";
            tileRed.Click += TileSelect_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(tileCurrent);
            groupBox2.Location = new Point(12, 255);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(153, 153);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Current Tile";
            // 
            // tileCurrent
            // 
            tileCurrent.Location = new Point(32, 39);
            tileCurrent.Name = "tileCurrent";
            tileCurrent.Size = new Size(86, 86);
            tileCurrent.TabIndex = 6;
            tileCurrent.TabStop = false;
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(12, 436);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(153, 77);
            buttonSave.TabIndex = 2;
            buttonSave.Text = "Save File";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += ButtonSave_Click;
            // 
            // buttonLoad
            // 
            buttonLoad.Location = new Point(12, 519);
            buttonLoad.Name = "buttonLoad";
            buttonLoad.Size = new Size(153, 77);
            buttonLoad.TabIndex = 3;
            buttonLoad.Text = "Load File";
            buttonLoad.UseVisualStyleBackColor = true;
            buttonLoad.Click += ButtonLoad_Click;
            // 
            // groupBoxMap
            // 
            groupBoxMap.Location = new Point(171, 12);
            groupBoxMap.Name = "groupBoxMap";
            groupBoxMap.Size = new Size(651, 598);
            groupBoxMap.TabIndex = 4;
            groupBoxMap.TabStop = false;
            groupBoxMap.Text = "Map";
            // 
            // Form2
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(835, 622);
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
    }
}