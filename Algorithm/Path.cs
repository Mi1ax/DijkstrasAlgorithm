using System;
using System.Drawing;
using System.Numerics;
using Core.Drawing.GUI;
using Core.Drawing.Shapes;

namespace Algorithm
{
    public class Path
    {
        public Label Label { get; }
        
        public Line Line { get; }
        
        public Vertex Start { get; set; }

        public Vertex End { get; set; }

        public int Weight
        {
            get => int.Parse(Label.Text);
            set => Label.Text = value.ToString();
        }
            
        public Path(Vertex start, Vertex end, int weight)
        {
            Line = new Line(start.Circle.Center, end.Circle.Center);
            Label = new Label(weight.ToString(), 20)
            {
                IsCenter = true,
                Position = new Vector2(((Line.PositionFrom.X + Line.PositionTo.X)/2) + 18, (Line.PositionFrom.Y + Line.PositionTo.Y)/2),
                Color = Color.White
            };
            
            Start = start;
            End = end;
            Weight = weight;
        }

        public void Draw()
        {
            Line.Draw();
            Label.Draw();
        }
    }
}