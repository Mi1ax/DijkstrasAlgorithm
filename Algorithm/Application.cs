using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

using Core;
using Core.Drawing.GUI;
using Core.Drawing.Shapes;
using Color = System.Drawing.Color;
using MouseButton = Core.Input.MouseButton;
using Rectangle = Core.Drawing.Shapes.Rectangle;

namespace Algorithm
{
    public class Application : Window
    {
        private readonly Panel _rightPanel;
        private Rectangle _workSpace;

        private readonly List<Vertex> _vertices = new ();
        private readonly List<Path> _paths = new ();

        private int _circleCount;
        
        private bool _isAdding;
        private bool _isConnecting;

        private Vertex? _selectedStart;
        private Vertex? _selectedEnd;
        
        public Application()
        {
            Settings.Title = "Application";
            Settings.Width = 1280;
            Settings.Height = 720;

            _workSpace = new Rectangle(Vector2.Zero, new SizeF(
                Settings.Width - Settings.Width / 3, 
                Settings.Height));

            var addPoint = new Button(Vector2.Zero, new SizeF(175, 35), "Add Point");
            var connectPoint = new Button(Vector2.Zero, new SizeF(175, 35), "Connect Points");
            var calculateVertices = new Button(Vector2.Zero, new SizeF(175, 35), "Calculate");

            addPoint.OnButtonPressed += () =>
            {
                addPoint.Disable = true;
                connectPoint.Disable = false;
                _isAdding = true;
                _isConnecting = false;
            };

            connectPoint.OnButtonPressed += () =>
            {
                addPoint.Disable = false;
                connectPoint.Disable = true;
                _isAdding = false;
                _isConnecting = true;
            };

            calculateVertices.OnButtonPressed += () =>
            {
                
            };

            var panelSize = new SizeF(
                Settings.Width / 3f,
                Settings.Height
                );
            
            _rightPanel = new Panel(this, panelSize)
            {
                Position = new Vector2(Settings.Width - panelSize.Width, 0),
                Padding = new Vector2(10)
            };
            
            _rightPanel.Add(
                new Label("Dijkstras Algorithm"),
                addPoint,
                connectPoint,
                calculateVertices,
                new Button(Vector2.Zero, new SizeF(175, 35), "Exit")
                {
                    OnButtonPressed = Close
                });
        }

        private Vertex? GetCircleByPos(Vector2 mousePosition) => 
            _vertices.Find(c => c.Circle.IsIn(mousePosition));

        protected override void Update(float deltaTime)
        {
            _rightPanel.Update(deltaTime);
            if (IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                if (_isAdding && Utils.IsInRect(_workSpace, GetMousePosition()))
                {
                    _vertices.Add(
                        new Vertex(new Circle(GetMousePosition(), 25f)
                        {
                            FillColor = Color.White
                        }, 
                        new Label(_circleCount++.ToString(), 25f)
                        {
                            IsCenter = true,
                            Position = GetMousePosition()
                        }));
                } else if (_isConnecting)
                {
                    var vertex = GetCircleByPos(GetMousePosition());
                    if (vertex != null) {
                        vertex.Selected = true;
                        if (_selectedStart == null) _selectedStart = vertex;
                        else if (_selectedEnd == null) _selectedEnd = vertex;

                        if (_selectedStart != null && _selectedEnd != null)
                        {
                            var path = new Path(_selectedStart, _selectedEnd, 0);
                            _paths.Add(path);

                            _selectedStart.Selected = false;
                            _selectedEnd.Selected = false;

                            _selectedStart = null;
                            _selectedEnd = null;
                        }
                    }
                }
            } else if (IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_RIGHT))
            {
                _paths.ForEach(path =>
                {
                    
                });
            }
        }

        protected override void Draw()
        {
            _paths.ForEach(p => p.Draw());
            _vertices.ForEach(v => v.Draw());
            _rightPanel.Draw();
        }
    }
}