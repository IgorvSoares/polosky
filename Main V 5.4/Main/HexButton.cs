using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main
{
  
        public class HexButton : ButtonApplication
        {
           public ButtonApplication btnExam = new ButtonApplication();



            public Point[] GetPoints(ButtonApplication container)
            {
                Point[] points = new Point[6];
                int half = container.Height / 2;
                int quart = container.Width / 4;
                points[5] = new Point(container.Left + quart, container.Top);
                points[4] = new Point(container.Right - quart, container.Top);
                points[3] = new Point(container.Right, container.Top + half);
                points[2] = new Point(container.Right - quart, container.Bottom);
                points[1] = new Point(container.Left + quart, container.Bottom);
                points[0] = new Point(container.Left, container.Top + half);
          
                return points;
            }
         

            protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
            {
                GraphicsPath polygon_path = new GraphicsPath();
                polygon_path.AddPolygon(GetPoints(btnExam));
                this.Region = new System.Drawing.Region(polygon_path);
                base.OnPaint(e);
            }

        }
    
}
