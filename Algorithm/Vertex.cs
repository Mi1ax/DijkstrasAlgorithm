using System;
using System.Drawing;
using System.Numerics;
using Core.Drawing.GUI;
using Core.Drawing.Shapes;

namespace Algorithm
{
    public class Vertex
    {
        private bool _isSelected;

        public Circle Circle { get; }

        public Label Label { get; }

        public int Number => int.Parse(Label.Text);
        
        public int Weight { get; set; }

        public Vertex? PreviousVertex { get; set; }

        public bool Selected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                Circle.BorderColor = value ? Color.Green : Color.Black;
            }
        }
        
        public Vertex(Circle circle, int number)
        {
            Circle = circle;
            Label = new Label(number.ToString(), 25f)
            {
                IsCenter = true,
                Position = circle.Position
            };
        }

        public void Draw()
        {
            Circle.Draw();
            Label.Draw();
        }
    }
}