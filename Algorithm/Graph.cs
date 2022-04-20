using System.Collections.Generic;

namespace Algorithm
{
    public class Graph
    {
        private readonly int _pointsCount;
        private List<Vertex> _points;
        private List<Path> _lines;

        public Graph(List<Vertex> _vertices, List<Path> lines)
        {
            _pointsCount = _vertices.Count;
            _lines = new List<Path>();
            _points = _vertices;
        }

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