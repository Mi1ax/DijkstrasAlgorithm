using System.Collections.Generic;
using System.Numerics;

namespace Algorithm
{
    public class Graph
    {
        private int _pointsCount;
        private readonly List<Vertex> _points;
        private readonly List<Path> _lines;

        public List<Path> Paths => _lines;

        public Graph()
        {
            _lines = new List<Path>();
            _points = new List<Vertex>();
        }

        public void AddPoint(Vertex point)
        {
            _points.Add(point);
            _pointsCount = _points.Count;
        }

        public void AddPath(Path path)
        {
            _lines.Add(path);
        }

        public void Draw()
        {
            _lines.ForEach(l =>
            {
                l.Draw();
            });
            _points.ForEach(p =>
            {
                p.Draw();
            });
        }
        
        public Vertex? GetPointByPos(Vector2 mousePosition) => 
            _points.Find(c => c.Circle.IsIn(mousePosition));

        public void CalculatePath(bool isShortest) 
        {  
            for (var point = 0; point <= _pointsCount; point++)
            {
                var lines = _lines.FindAll(l => l.Start.Number == point);
                foreach (var line in lines)
                {
                    line.Start = _points[point];
                    line.End = _points[line.End.Number];
                    if (_points[line.End.Number].Weight == 0
                        || (isShortest ? line.Start.Weight + line.Weight < _points[line.End.Number].Weight 
                            : line.Start.Weight + line.Weight > _points[line.End.Number].Weight))
                    {
                        _points[line.End.Number].Weight = line.Start.Weight + line.Weight;
                        _points[line.End.Number].PreviousVertex = _points[line.Start.Number];
                    }

                    line.End = _points[line.End.Number];
                    line.Start = _points[point];
                }
            }
        }
        
        
    }
}