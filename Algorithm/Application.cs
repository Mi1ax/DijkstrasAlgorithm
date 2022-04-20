using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

using Core;
using Core.Drawing.GUI;
using Core.Drawing.Shapes;
using Core.Input;
using Color = System.Drawing.Color;
using MouseButton = Core.Input.MouseButton;
using Rectangle = Core.Drawing.Shapes.Rectangle;

namespace Algorithm
{
    public class Application : Window
    {
        private readonly Panel _rightPanel;
        private readonly Rectangle _workSpace;

        private Graph _graph;

        private int _circleCount;
        
        private bool _isAdding;
        private bool _isConnecting;
        
        private bool _isTextEditing;
        private Label? _editingLabel;
        
        private Vertex? _selectedStart;
        private Vertex? _selectedEnd;
        
        public Application()
        {
            Settings.Title = "Application";
            Settings.Width = 1280;
            Settings.Height = 720;
            _isTextEditing = false;

            _workSpace = new Rectangle(Vector2.Zero, new SizeF(
                Settings.Width - Settings.Width / 3, 
                Settings.Height));
            
            _graph = new Graph();

            var addPoint = new Button(Vector2.Zero, new SizeF(175, 35), "Add Point");
            var connectPoint = new Button(Vector2.Zero, new SizeF(175, 35), "Connect Points");
            var loadData = new Button(Vector2.Zero, new SizeF(175, 35), "Load Data");
            var saveData = new Button(Vector2.Zero, new SizeF(175, 35), "Save Data");
            var calculate = new Button(Vector2.Zero, new SizeF(175, 35), "Calculate");

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

            loadData.OnButtonPressed += () =>
            {
                Console.WriteLine("Data Loaded");
                _graph = DataManager.LoadData();
            };

            saveData.OnButtonPressed += () =>
            {
                Console.WriteLine("Data Saved");
                DataManager.SaveData(_graph);
            };

            calculate.OnButtonPressed += () =>
            {
                _graph.CalculatePath(true);
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
                loadData,
                saveData,
                calculate,
                new Button(Vector2.Zero, new SizeF(175, 35), "Exit")
                {
                    OnButtonPressed = Close
                });
        }

        protected override void Update(float deltaTime)
        {
            var mousePosition = GetMousePosition();
            
            _rightPanel.Update(deltaTime);
            
            InputText();

            if (!IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT)) return;
            if (_isAdding && Utils.IsInRect(_workSpace, mousePosition))
            {
                _graph.AddPoint(
                    new Vertex(new Circle(mousePosition, 25f)
                        {
                            FillColor = Color.White
                        }, _circleCount++));
            } else if (_isConnecting)
            {
                var vertex = _graph.GetPointByPos(mousePosition);
                if (vertex != null) {
                    vertex.Selected = true;
                    if (_selectedStart == null) _selectedStart = vertex;
                    else if (_selectedEnd == null) _selectedEnd = vertex;

                    if (_selectedStart != null && _selectedEnd != null)
                    {
                        var path = new Path(_selectedStart, _selectedEnd, 0);
                        _graph.AddPath(path);

                        _selectedStart.Selected = false;
                        _selectedEnd.Selected = false;

                        _selectedStart = null;
                        _selectedEnd = null;
                    }
                }
                    
                _graph.Paths.ForEach(path =>
                {
                    if (!Utils.IsInRect(path.Label.Rectangle, mousePosition)) return;
                    path.Label.Text = "";
                    _isTextEditing = true;
                    _editingLabel = path.Label;
                });
            }
        }

        private void InputText()
        {
            if (!_isTextEditing || _editingLabel == null) return;
            _editingLabel.Color = Color.Aqua;
            var key = KeyboardInput.GetCharPressed();
            while (key > 0)
            {
                switch (key)
                {
                    case >= 48 and <= 57:
                    {
                        if (_editingLabel != null) 
                            _editingLabel.Text += (char)key;
                        break;
                    }
                }

                key = KeyboardInput.GetCharPressed();
            }
                
            if (KeyboardInput.IsKeyPressed(Keys.KeyBackspace) 
                && _editingLabel != null 
                && _editingLabel.Text.Length >= 1)
            {
                _editingLabel.Text = _editingLabel.Text[..^1];
            }

            if (!KeyboardInput.IsKeyPressed(Keys.KeyEnter)) return;
            _isTextEditing = false;
            if (_editingLabel != null)
            {
                if (_editingLabel.Text.Length == 0) 
                    _editingLabel.Text = "0";
                _editingLabel.Color = Color.White;
            }

            _editingLabel = null;
        }

        protected override void Draw()
        {
            _graph.Draw();
            _rightPanel.Draw();
        }
    }
}