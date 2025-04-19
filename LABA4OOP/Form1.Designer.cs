
namespace LABA4OOP
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
            panel1 = new Panel();
            Delete = new Button();
            btnColor = new Button();
            Segment = new Button();
            Rectangle = new Button();
            Triangle = new Button();
            Ellips = new Button();
            Circle = new Button();
            Square = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(Delete);
            panel1.Controls.Add(btnColor);
            panel1.Controls.Add(Segment);
            panel1.Controls.Add(Rectangle);
            panel1.Controls.Add(Triangle);
            panel1.Controls.Add(Ellips);
            panel1.Controls.Add(Circle);
            panel1.Controls.Add(Square);
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(300, 109);
            panel1.TabIndex = 0;
            // 
            // Delete
            // 
            Delete.Location = new Point(209, 41);
            Delete.Name = "Delete";
            Delete.Size = new Size(75, 23);
            Delete.TabIndex = 8;
            Delete.Text = "удалить";
            Delete.UseVisualStyleBackColor = true;
            Delete.Click += Delete_Click;
            // 
            // btnColor
            // 
            btnColor.Location = new Point(209, 12);
            btnColor.Name = "btnColor";
            btnColor.Size = new Size(75, 23);
            btnColor.TabIndex = 7;
            btnColor.Text = "цвет";
            btnColor.UseVisualStyleBackColor = true;
            btnColor.Click += Color_Click;
            // 
            // Segment
            // 
            Segment.Location = new Point(93, 70);
            Segment.Name = "Segment";
            Segment.Size = new Size(110, 23);
            Segment.TabIndex = 6;
            Segment.Text = "отрезок";
            Segment.UseVisualStyleBackColor = true;
            Segment.Click += Segment_Click;
            // 
            // Rectangle
            // 
            Rectangle.Location = new Point(93, 12);
            Rectangle.Name = "Rectangle";
            Rectangle.Size = new Size(110, 23);
            Rectangle.TabIndex = 4;
            Rectangle.Text = "прямоугольник";
            Rectangle.UseVisualStyleBackColor = true;
            Rectangle.Click += Rectangle_Click;
            // 
            // Triangle
            // 
            Triangle.Location = new Point(93, 41);
            Triangle.Name = "Triangle";
            Triangle.Size = new Size(110, 23);
            Triangle.TabIndex = 5;
            Triangle.Text = "треугольник";
            Triangle.UseVisualStyleBackColor = true;
            Triangle.Click += Triangle_Click;
            // 
            // Ellips
            // 
            Ellips.Location = new Point(12, 70);
            Ellips.Name = "Ellips";
            Ellips.Size = new Size(75, 23);
            Ellips.TabIndex = 3;
            Ellips.Text = "эллипс";
            Ellips.UseVisualStyleBackColor = true;
            Ellips.Click += Ellips_Click;
            // 
            // Circle
            // 
            Circle.Location = new Point(12, 12);
            Circle.Name = "Circle";
            Circle.Size = new Size(75, 23);
            Circle.TabIndex = 1;
            Circle.Text = "круг";
            Circle.UseVisualStyleBackColor = true;
            Circle.Click += Circle_Click;
            // 
            // Square
            // 
            Square.Location = new Point(12, 41);
            Square.Name = "Square";
            Square.Size = new Size(75, 23);
            Square.TabIndex = 2;
            Square.Text = "квадрат";
            Square.UseVisualStyleBackColor = true;
            Square.Click += Square_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel1);
            KeyPreview = true;
            Name = "Form1";
            Text = "Form1";
            MouseDown += Form1_MouseDown;
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private Panel panel1;
        private Button Delete;
        private Button btnColor;
        private Button Segment;
        private Button Rectangle;
        private Button Triangle;
        private Button Ellips;
        private Button Circle;
        private Button Square;
    }
}
