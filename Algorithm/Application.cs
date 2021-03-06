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
                Settings.Height))
            {
                FillColor = Color.Wheat
            };
            
            _graph = new Graph();

            var addPoint = new Button(Vector2.Zero, new SizeF(175, 35), "Add Point");
            var connectPoint = new Button(Vector2.Zero, new SizeF(175, 35), "Connect Points");
            var loadData = new Button(Vector2.Zero, new SizeF(175, 35), "Load Data");
            var saveData = new Button(Vector2.Zero, new SizeF(175, 35), "Save Data");
            var clearButton = new Button(Vector2.Zero, new SizeF(175, 35), "Clear Graph");
            var calculate = new Button(Vector2.Zero, new SizeF(175, 35), "Calculate");
            var checkBox = new CheckBox(Vector2.Zero, new SizeF(16, 16), "Shortest Path"); 

            addPoint.OnButtonPressed += () =>
            {
                _isTextEditing = false;
                _editingLabel = null;
                addPoint.Disable = true;
                connectPoint.Disable = false;
                _isAdding = true;
                _isConnecting = false;
            };

            connectPoint.OnButtonPressed += () =>
            {
                _isTextEditing = false;
                _editingLabel = null;
                addPoint.Disable = false;
                connectPoint.Disable = true;
                _isAdding = false;
                _isConnecting = true;
            };

            loadData.OnButtonPressed += () =>
            {
                _isTextEditing = false;
                _editingLabel = null;
                _graph = DataManager.LoadData();
                _circleCount = _graph.VertexCount;
            };

            saveData.OnButtonPressed += () =>
            {
                _isTextEditing = false;
                _editingLabel = null;
                DataManager.SaveData(_graph);
            };

            clearButton.OnButtonPressed += () =>
            {
                _isTextEditing = false;
                _editingLabel = null;
                _graph = new Graph();
                _circleCount = 0;
            };

            calculate.OnButtonPressed += () =>
            {
                if (_circleCount == 0) return;
                _isTextEditing = false;
                _editingLabel = null;
                _graph.ClearColor();
                _graph.CalculatePath(checkBox.Checked);
                _graph.PrintPath(checkBox.Checked);
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
                clearButton,
                calculate,
                checkBox,
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
                _isTextEditing = false;
                _editingLabel = null;
                _graph.AddPoint(
                    new Vertex(new Circle(mousePosition, 30f)
                        {
                            FillColor = Color.White,
                            BorderColor = Color.Black,
                            BorderThickness = 3f
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
                    path.Label.Text = "_";
                    _isTextEditing = true;
                    _editingLabel = path.Label;
                });
            }
        }

        private void InputText()
        {
            if (!_isTextEditing || _editingLabel == null) return;
            var key = KeyboardInput.GetCharPressed();
            while (key > 0)
            {
                switch (key)
                {
                    case >= 48 and <= 57:
                    {
                        if (_editingLabel != null)
                        {
                            if (_editingLabel.Text == "_")
                                _editingLabel.Text = "";
                            _editingLabel.Text += (char) key;
                        }

                        break;
                    }
                }

                key = KeyboardInput.GetCharPressed();
            }
                
            if (KeyboardInput.IsKeyPressed(Keys.KeyBackspace) 
                && _editingLabel != null 
                && _editingLabel.Text.Length > 1)
            {
                _editingLabel.Text = _editingLabel.Text[..^1];
            } else if (KeyboardInput.IsKeyPressed(Keys.KeyBackspace)
                       && _editingLabel != null
                       && _editingLabel.Text.Length == 1)
            {
                _editingLabel.Text = "_";
            }

            if (!KeyboardInput.IsKeyPressed(Keys.KeyEnter)) return;
            _isTextEditing = false;
            if (_editingLabel != null)
            {
                if (_editingLabel.Text.Length == 0) 
                    _editingLabel.Text = "0";
                _editingLabel.Color = Color.Black;
            }

            _editingLabel = null;
        }

        protected override void Draw()
        {
            _workSpace.Draw();
            _graph.Draw();
            _rightPanel.Draw();
        }
    }
}