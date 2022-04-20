using System;
using System.Drawing;
using System.Numerics;
using Core.Drawing.GUI;
using Core.Drawing.Shapes;

namespace Algorithm
{
    public class Vertex
    {
        private readonly Circle _circle;
        private readonly Label _label;
        private bool _isSelected;

        public Circle Circle => _circle;
        
        public int Number => int.Parse(_label.Text);
        
        public int Weight { get; set; }

        public Vertex PreviousVertex { get; set; }

        public bool Selected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                _circle.BorderColor = value ? Color.GreenYellow : Color.Transparent;
            }
        }
        
        public Vertex(Circle circle, Label label)
        {
            _circle = circle;
            _label = label;
        }

        public void Draw()
        {
            _circle.Draw();
            _label.Draw();
        }
    }
}