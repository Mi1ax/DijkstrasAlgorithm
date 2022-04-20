using System.Drawing;
using System.Numerics;
using Core.Drawing.GUI;
using Core.Drawing.Shapes;

namespace Algorithm
{
    public class Path
    {
        private Label _label;
        
        public Line Line { get; }
        
        public Vertex Start { get; set; }

        public Vertex End { get; set; }
        
        public int Weight { get; }
            
        public Path(Vertex start, Vertex end, int weight)
        {
            Start = start;
            End = end;
            Weight = weight;
            Line = new Line(start.Circle.Center, end.Circle.Center);
            _label = new Label(weight.ToString(), 20)
            {
                IsCenter = true,
                Position = new Vector2(((Line.PositionFrom.X + Line.PositionTo.X)/2) + 18, (Line.PositionFrom.Y + Line.PositionTo.Y)/2),
                Color = Color.White
            };
        }

        public void Draw()
        {
            Line.Draw();
            _label.Draw();
        }
    }
}