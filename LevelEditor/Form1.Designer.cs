namespace HW2_LevelEditor
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            buttonLoad = new Button();
            groupBox1 = new GroupBox();
            textBoxHeight = new TextBox();
            textBoxWidth = new TextBox();
            buttonCreate = new Button();
            label2 = new Label();
            label1 = new Label();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // buttonLoad
            // 
            buttonLoad.Location = new Point(12, 12);
            buttonLoad.Name = "buttonLoad";
            buttonLoad.Size = new Size(322, 83);
            buttonLoad.TabIndex = 0;
            buttonLoad.Text = "Load Map";
            buttonLoad.UseVisualStyleBackColor = true;
            buttonLoad.Click += ButtonLoad_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(textBoxHeight);
            groupBox1.Controls.Add(textBoxWidth);
            groupBox1.Controls.Add(buttonCreate);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(12, 112);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(322, 223);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Create New Map";
            // 
            // textBoxHeight
            // 
            textBoxHeight.Location = new Point(134, 82);
            textBoxHeight.Name = "textBoxHeight";
            textBoxHeight.Size = new Size(168, 27);
            textBoxHeight.TabIndex = 4;
            // 
            // textBoxWidth
            // 
            textBoxWidth.Location = new Point(134, 34);
            textBoxWidth.Name = "textBoxWidth";
            textBoxWidth.Size = new Size(168, 27);
            textBoxWidth.TabIndex = 3;
            // 
            // buttonCreate
            // 
            buttonCreate.Location = new Point(6, 128);
            buttonCreate.Name = "buttonCreate";
            buttonCreate.Size = new Size(310, 83);
            buttonCreate.TabIndex = 2;
            buttonCreate.Text = "Create Map";
            buttonCreate.UseVisualStyleBackColor = true;
            buttonCreate.Click += ButtonCreate_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(15, 85);
            label2.Name = "label2";
            label2.Size = new Size(95, 20);
            label2.TabIndex = 1;
            label2.Text = "Height (tiles)";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(15, 34);
            label1.Name = "label1";
            label1.Size = new Size(90, 20);
            label1.TabIndex = 0;
            label1.Text = "Width (tiles)";
            // 
            // Form1
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(346, 347);
            Controls.Add(groupBox1);
            Controls.Add(buttonLoad);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            Text = "Level Editor";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button buttonLoad;
        private GroupBox groupBox1;
        private TextBox textBoxHeight;
        private TextBox textBoxWidth;
        private Button buttonCreate;
        private Label label2;
        private Label label1;
    }
}
