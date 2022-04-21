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
            Line = new Line(start.Circle.Center, end.Circle.Center)
            {
                Color = Color.Black,
                Thickness = 3
            };

            const int number = 15;
            var labelPos = (Line.PositionTo + Line.PositionFrom) / 2;
            if ((Line.PositionTo.X > Line.PositionFrom.X && Line.PositionTo.Y < Line.PositionFrom.Y) ||
                Line.PositionFrom.X > Line.PositionTo.X && Line.PositionFrom.Y < Line.PositionTo.Y)
            {
                labelPos += new Vector2(-number);
            } else if ((Line.PositionTo.X > Line.PositionFrom.X && Line.PositionTo.Y > Line.PositionFrom.Y) ||
                       Line.PositionFrom.X > Line.PositionTo.X && Line.PositionFrom.Y > Line.PositionTo.Y)
            {
                labelPos += new Vector2(number, -number);
            } else if (Math.Abs(Line.PositionTo.X - Line.PositionFrom.X) < 0.1)
            {
                labelPos += new Vector2(number, 0);
            }
            
            Label = new Label(weight.ToString(), 30)
            {
                IsCenter = true,
                Position = labelPos,
                Color = Color.Black
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