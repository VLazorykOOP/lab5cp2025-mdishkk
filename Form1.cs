using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LabApproxFractal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            buttonApprox.Click += buttonApprox_Click;
            panelApprox.Paint += panelApprox_Paint;
            buttonFractal.Click += buttonFractal_Click;
            panelFractal.Paint += panelFractal_Paint;
        }


        private void buttonApprox_Click(object sender, EventArgs e)
        {
            panelApprox.Invalidate();
        }

        private void panelApprox_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (!float.TryParse(textBoxX1.Text, out float x1) ||
                !float.TryParse(textBoxY1.Text, out float y1) ||
                !float.TryParse(textBoxX2.Text, out float x2) ||
                !float.TryParse(textBoxY2.Text, out float y2) ||
                !float.TryParse(textBoxX3.Text, out float x3) ||
                !float.TryParse(textBoxY3.Text, out float y3) ||
                !float.TryParse(textBoxX4.Text, out float x4) ||
                !float.TryParse(textBoxY4.Text, out float y4))
            {
                return;
            }

            var p1 = new PointF(x1, y1);
            var p2 = new PointF(x2, y2);
            var p3 = new PointF(x3, y3);
            var p4 = new PointF(x4, y4);

            const int steps = 200;
            var prev = p1;
            for (int i = 1; i <= steps; i++)
            {
                float t = i / (float)steps;
                float u = 1 - t;
                float x = u * u * u * p1.X
                        + 3 * u * u * t * p2.X
                        + 3 * u * t * t * p3.X
                        + t * t * t * p4.X;
                float y = u * u * u * p1.Y
                        + 3 * u * u * t * p2.Y
                        + 3 * u * t * t * p3.Y
                        + t * t * t * p4.Y;
                var curr = new PointF(x, y);
                g.DrawLine(Pens.Blue, prev, curr);
                prev = curr;
            }
        }

        private void buttonFractal_Click(object sender, EventArgs e)
        {
            panelFractal.Invalidate();
        }

        private void panelFractal_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (!float.TryParse(textBoxX1F.Text, out float x1) ||
                !float.TryParse(textBoxY1F.Text, out float y1) ||
                !float.TryParse(textBoxX2F.Text, out float x2) ||
                !float.TryParse(textBoxY2F.Text, out float y2) ||
                !float.TryParse(textBoxX3F.Text, out float x3) ||
                !float.TryParse(textBoxY3F.Text, out float y3) ||
                !int.TryParse(textBoxKF.Text, out int order))
            {
                return;
            }

            var p1 = new PointF(x1, y1);
            var p2 = new PointF(x2, y2);
            var p3 = new PointF(x3, y3);

            DrawKoch(g, p1, p2, order);
            DrawKoch(g, p2, p3, order);
            DrawKoch(g, p3, p1, order);
        }

        private void DrawKoch(Graphics g, PointF a, PointF b, int order)
        {
            if (order == 0)
            {
                g.DrawLine(Pens.Green, a, b);
            }
            else
            {
                float dx = (b.X - a.X) / 3f;
                float dy = (b.Y - a.Y) / 3f;
                var p1 = new PointF(a.X + dx, a.Y + dy);
                var p2 = new PointF(a.X + 2 * dx, a.Y + 2 * dy);

                double angle = Math.PI / 3; 
                var ux = p2.X - p1.X;
                var uy = p2.Y - p1.Y;
                var px = (float)(p1.X + ux * Math.Cos(angle) - uy * Math.Sin(angle));
                var py = (float)(p1.Y + ux * Math.Sin(angle) + uy * Math.Cos(angle));
                var peak = new PointF(px, py);


                DrawKoch(g, a, p1, order - 1);
                DrawKoch(g, p1, peak, order - 1);
                DrawKoch(g, peak, p2, order - 1);
                DrawKoch(g, p2, b, order - 1);
            }
        }
    }
}
