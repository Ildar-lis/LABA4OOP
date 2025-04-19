using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LABA4OOP
{
    public partial class Form1 : Form
    {
        public enum ShapeType { None, Circle, Square, Ellipse, Rectangle, Triangle, Segment }

        private ShapeType currentShape = ShapeType.None;
        private List<(ShapeType shape, Point location)> shapes = new();
        private List<Color> shapeColors = new();
        private Dictionary<int, Size> shapeSizes = new();
        private Dictionary<int, (Point start, Point end)> segmentPoints = new();
        private HashSet<int> selectedIndices = new();

        private Point startPoint;
        private Point endPoint;
        private bool isDrawing = false;

        public Form1()
        {
            InitializeComponent();

            this.KeyPreview = true;

            this.MouseDown += Form1_MouseDown;
            this.MouseMove += Form1_MouseMove;
            this.MouseUp += Form1_MouseUp;
            this.Paint += Form1_Paint;
        }

        private void Circle_Click(object sender, EventArgs e) => currentShape = ShapeType.Circle;
        private void Square_Click(object sender, EventArgs e) => currentShape = ShapeType.Square;
        private void Ellips_Click(object sender, EventArgs e) => currentShape = ShapeType.Ellipse;
        private void Rectangle_Click(object sender, EventArgs e) => currentShape = ShapeType.Rectangle;
        private void Triangle_Click(object sender, EventArgs e) => currentShape = ShapeType.Triangle;
        private void Segment_Click(object sender, EventArgs e) => currentShape = ShapeType.Segment;

        private void Color_Click(object sender, EventArgs e)
        {
            if (selectedIndices.Count == 0) return;

            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (int index in selectedIndices)
                        shapeColors[index] = colorDialog.Color;

                    Invalidate();
                }
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (selectedIndices.Count == 0) return;

            var toRemove = selectedIndices.OrderByDescending(i => i).ToList();

            foreach (var i in toRemove)
            {
                shapes.RemoveAt(i);
                shapeColors.RemoveAt(i);
            }

            var newShapeSizes = new Dictionary<int, Size>();
            var newSegmentPoints = new Dictionary<int, (Point, Point)>();

            int newIndex = 0;
            for (int oldIndex = 0; oldIndex <= shapes.Count; oldIndex++)
            {
                if (toRemove.Contains(oldIndex)) continue;

                if (shapeSizes.ContainsKey(oldIndex))
                    newShapeSizes[newIndex] = shapeSizes[oldIndex];

                if (segmentPoints.ContainsKey(oldIndex))
                    newSegmentPoints[newIndex] = segmentPoints[oldIndex];

                newIndex++;
            }

            shapeSizes = newShapeSizes;
            segmentPoints = newSegmentPoints;

            selectedIndices.Clear();
            Invalidate();
        }



        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            bool isCtrlPressed = (ModifierKeys & Keys.Control) == Keys.Control;
            bool clickedOnShape = false;

            for (int i = shapes.Count - 1; i >= 0; i--)
            {
                var (shape, location) = shapes[i];
                if (IsPointInsideShape(i, shape, location, e.Location))
                {
                    clickedOnShape = true;

                    if (isCtrlPressed)
                    {
                        if (!selectedIndices.Contains(i))
                            selectedIndices.Add(i);
                    }
                    else
                    {
                        selectedIndices.Clear();
                        selectedIndices.Add(i);
                    }

                    break; 
                }
            }

            if (!clickedOnShape)
            {

                if (!isCtrlPressed)
                {
                    selectedIndices.Clear();
                }
                else
                {
                    selectedIndices.Clear();
                }

                if (currentShape == ShapeType.Rectangle || currentShape == ShapeType.Ellipse || currentShape == ShapeType.Segment)
                {
                    startPoint = e.Location;
                    endPoint = e.Location;
                    isDrawing = true;
                }
                else if (currentShape != ShapeType.None)
                {
                    shapes.Add((currentShape, e.Location));
                    shapeColors.Add(Color.Black);
                    shapeSizes[shapes.Count - 1] = new Size(50, 50);
                    selectedIndices.Add(shapes.Count - 1);
                }
            }

            Invalidate();
        }



        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                endPoint = e.Location;
                Invalidate();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                isDrawing = false;
                if (currentShape == ShapeType.Segment)
                {
                    shapes.Add((currentShape, Point.Empty));
                    shapeColors.Add(Color.Black);
                    shapeSizes[shapes.Count - 1] = new Size(1, 1);
                    segmentPoints[shapes.Count - 1] = (startPoint, endPoint);
                }
                else
                {
                    var shapeCenter = new Point((startPoint.X + endPoint.X) / 2, (startPoint.Y + endPoint.Y) / 2);
                    var size = new Size(Math.Abs(endPoint.X - startPoint.X), Math.Abs(endPoint.Y - startPoint.Y));
                    if (size.Width == 0 || size.Height == 0) return;

                    shapes.Add((currentShape, shapeCenter));
                    shapeColors.Add(Color.Black);
                    shapeSizes[shapes.Count - 1] = size;
                }

                selectedIndices.Clear();
                selectedIndices.Add(shapes.Count - 1);
                Invalidate();
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            for (int i = 0; i < shapes.Count; i++)
            {
                var (shape, location) = shapes[i];
                Color shapeColor = shapeColors[i];
                bool isSelected = selectedIndices.Contains(i);
                Size size = shapeSizes.ContainsKey(i) ? shapeSizes[i] : new Size(50, 50);

                Brush brush = new SolidBrush(isSelected ? Color.Blue : shapeColor);
                Pen pen = new Pen(isSelected ? Color.Blue : shapeColor, 2);

                int w = size.Width;
                int h = size.Height;

                switch (shape)
                {
                    case ShapeType.Circle:
                        g.FillEllipse(brush, location.X - w / 2, location.Y - h / 2, w, h);
                        break;
                    case ShapeType.Square:
                        g.FillRectangle(brush, location.X - w / 2, location.Y - h / 2, w, h);
                        break;
                    case ShapeType.Ellipse:
                        g.FillEllipse(brush, location.X - w / 2, location.Y - h / 2, w, h);
                        break;
                    case ShapeType.Rectangle:
                        g.FillRectangle(brush, location.X - w / 2, location.Y - h / 2, w, h);
                        break;
                    case ShapeType.Triangle:
                        Point[] triangle = {
                            new Point(location.X, location.Y - h / 2),
                            new Point(location.X - w / 2, location.Y + h / 2),
                            new Point(location.X + w / 2, location.Y + h / 2)
                        };
                        g.FillPolygon(brush, triangle);
                        break;
                    case ShapeType.Segment:
                        if (segmentPoints.TryGetValue(i, out var seg))
                            g.DrawLine(pen, seg.start, seg.end);
                        break;
                }

                brush.Dispose();
                pen.Dispose();
            }

            if (isDrawing && (currentShape == ShapeType.Rectangle || currentShape == ShapeType.Ellipse || currentShape == ShapeType.Segment))
            {
                Pen previewPen = new Pen(Color.Gray, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };

                switch (currentShape)
                {
                    case ShapeType.Rectangle:
                        g.DrawRectangle(previewPen, GetRectangle(startPoint, endPoint));
                        break;
                    case ShapeType.Ellipse:
                        g.DrawEllipse(previewPen, GetRectangle(startPoint, endPoint));
                        break;
                    case ShapeType.Segment:
                        g.DrawLine(previewPen, startPoint, endPoint);
                        break;
                }
                previewPen.Dispose();
            }
        }

        private Rectangle GetRectangle(Point p1, Point p2)
        {
            return new Rectangle(
                Math.Min(p1.X, p2.X),
                Math.Min(p1.Y, p2.Y),
                Math.Abs(p1.X - p2.X),
                Math.Abs(p1.Y - p2.Y));
        }

        private bool IsPointInsideShape(int index, ShapeType shape, Point center, Point click)
        {
            Size size = shapeSizes.ContainsKey(index) ? shapeSizes[index] : new Size(50, 50);
            int w = size.Width;
            int h = size.Height;

            Rectangle bounds = new Rectangle(center.X - w / 2, center.Y - h / 2, w, h);

            return shape switch
            {
                ShapeType.Circle or ShapeType.Square or ShapeType.Ellipse or ShapeType.Rectangle => bounds.Contains(click),
                ShapeType.Triangle => PointInPolygon(click, new[] {
                    new Point(center.X, center.Y - h / 2),
                    new Point(center.X - w / 2, center.Y + h / 2),
                    new Point(center.X + w / 2, center.Y + h / 2)
                }),
                ShapeType.Segment => segmentPoints.TryGetValue(index, out var segPoints) &&
                                     DistanceToSegment(click, segPoints.start, segPoints.end) <= 5.0,
                _ => false
            };
        }

        private double DistanceToSegment(Point p, Point a, Point b)
        {
            float A = p.X - a.X;
            float B = p.Y - a.Y;
            float C = b.X - a.X;
            float D = b.Y - a.Y;

            float dot = A * C + B * D;
            float len_sq = C * C + D * D;
            float param = len_sq != 0 ? dot / len_sq : -1;

            float xx, yy;

            if (param < 0)
            {
                xx = a.X;
                yy = a.Y;
            }
            else if (param > 1)
            {
                xx = b.X;
                yy = b.Y;
            }
            else
            {
                xx = a.X + param * C;
                yy = a.Y + param * D;
            }

            float dx = p.X - xx;
            float dy = p.Y - yy;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private bool PointInPolygon(Point p, Point[] poly)
        {
            bool inside = false;
            for (int i = 0, j = poly.Length - 1; i < poly.Length; j = i++)
            {
                if (((poly[i].Y > p.Y) != (poly[j].Y > p.Y)) &&
                    (p.X < (poly[j].X - poly[i].X) * (p.Y - poly[i].Y) / (float)(poly[j].Y - poly[i].Y) + poly[i].X))
                {
                    inside = !inside;
                }
            }
            return inside;
        }



        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            int moveStep = 5; 

            if (keyData == Keys.Oemplus || keyData == Keys.Add)
            {
                bool canResizeAll = true;

                foreach (int index in selectedIndices)
                {
                    if (shapes[index].shape == ShapeType.Segment)
                    {
                        if (segmentPoints.TryGetValue(index, out var seg))
                        {
                            var dx = seg.end.X - seg.start.X;
                            var dy = seg.end.Y - seg.start.Y;

                            Point newStart = new Point(seg.start.X - dx / 10, seg.start.Y - dy / 10);
                            Point newEnd = new Point(seg.end.X + dx / 10, seg.end.Y + dy / 10);

                            if (!IsPointInsideClient(newStart.X, newStart.Y) || !IsPointInsideClient(newEnd.X, newEnd.Y))
                            {
                                canResizeAll = false;
                                break;
                            }
                        }
                    }
                    else if (shapeSizes.ContainsKey(index))
                    {
                        var (shape, location) = shapes[index];
                        var size = shapeSizes[index];
                        Size newSize = new Size(size.Width + 10, size.Height + 10);

                        Rectangle newBounds = new Rectangle(
                            location.X - newSize.Width / 2,
                            location.Y - newSize.Height / 2,
                            newSize.Width,
                            newSize.Height
                        );

                        if (!ClientRectangle.Contains(newBounds))
                        {
                            canResizeAll = false;
                            break;
                        }
                    }
                }

                if (canResizeAll)
                {
                    foreach (int index in selectedIndices)
                    {
                        if (shapes[index].shape == ShapeType.Segment)
                        {
                            if (segmentPoints.TryGetValue(index, out var seg))
                            {
                                var dx = seg.end.X - seg.start.X;
                                var dy = seg.end.Y - seg.start.Y;
                                seg.start.X -= dx / 10;
                                seg.start.Y -= dy / 10;
                                seg.end.X += dx / 10;
                                seg.end.Y += dy / 10;
                                segmentPoints[index] = seg;
                            }
                        }
                        else if (shapeSizes.ContainsKey(index))
                        {
                            var size = shapeSizes[index];
                            shapeSizes[index] = new Size(size.Width + 10, size.Height + 10);
                        }
                    }
                    Invalidate();
                }
                return true;
            }

            else if (keyData == Keys.OemMinus || keyData == Keys.Subtract)
            {
                foreach (int index in selectedIndices)
                {
                    if (shapes[index].shape == ShapeType.Segment)
                    {
                        if (segmentPoints.TryGetValue(index, out var seg))
                        {
                            var dx = seg.end.X - seg.start.X;
                            var dy = seg.end.Y - seg.start.Y;

                            if (Math.Abs(dx) > 20 && Math.Abs(dy) > 20)
                            {
                                seg.start.X += dx / 10;
                                seg.start.Y += dy / 10;
                                seg.end.X -= dx / 10;
                                seg.end.Y -= dy / 10;
                                segmentPoints[index] = seg;
                            }
                        }
                    }
                    else if (shapeSizes.ContainsKey(index))
                    {
                        var size = shapeSizes[index];
                        shapeSizes[index] = new Size(Math.Max(10, size.Width - 10), Math.Max(10, size.Height - 10));
                    }
                }
                Invalidate();
                return true;
            }
            else if (keyData is Keys.W or Keys.A or Keys.S or Keys.D)
            {
                int dx = 0, dy = 0;
                if (keyData == Keys.W) dy = -moveStep;
                if (keyData == Keys.S) dy = moveStep;
                if (keyData == Keys.A) dx = -moveStep;
                if (keyData == Keys.D) dx = moveStep;

                bool canMoveAll = true;

                foreach (int index in selectedIndices)
                {
                    if (shapes[index].shape == ShapeType.Segment)
                    {
                        if (segmentPoints.TryGetValue(index, out var seg))
                        {
                            if (!IsPointInsideClient(seg.start.X + dx, seg.start.Y + dy) ||
                                !IsPointInsideClient(seg.end.X + dx, seg.end.Y + dy))
                            {
                                canMoveAll = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        var (_, location) = shapes[index];
                        var size = shapeSizes.ContainsKey(index) ? shapeSizes[index] : new Size(50, 50);
                        var newCenter = new Point(location.X + dx, location.Y + dy);

                        if (newCenter.X - size.Width / 2 < 0 ||
                            newCenter.Y - size.Height / 2 < 0 ||
                            newCenter.X + size.Width / 2 > ClientSize.Width ||
                            newCenter.Y + size.Height / 2 > ClientSize.Height)
                        {
                            canMoveAll = false;
                            break;
                        }
                    }
                }

                if (canMoveAll)
                {
                    foreach (int index in selectedIndices)
                    {
                        if (shapes[index].shape == ShapeType.Segment)
                        {
                            if (segmentPoints.TryGetValue(index, out var seg))
                            {
                                seg.start.Offset(dx, dy);
                                seg.end.Offset(dx, dy);
                                segmentPoints[index] = seg;
                            }
                        }
                        else
                        {
                            var (shape, location) = shapes[index];
                            shapes[index] = (shape, new Point(location.X + dx, location.Y + dy));
                        }
                    }
                    Invalidate();
                }

                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private bool IsPointInsideClient(int x, int y)
        {
            return x >= 0 && x <= ClientSize.Width && y >= 0 && y <= ClientSize.Height;
        }

    }
}
