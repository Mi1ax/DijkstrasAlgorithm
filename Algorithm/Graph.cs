using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace Algorithm
{
    public class Graph
    {
        private int _pointsCount;

        public List<Path> Paths { get; }
        public List<Vertex> Vertices { get; }

        public Graph()
        {
            Paths = new List<Path>();
            Vertices = new List<Vertex>();
        }

        public Graph(List<Path> paths, List<Vertex> vertices)
        {
            Paths = paths;
            Vertices = vertices;
            _pointsCount = Vertices.Count;
        }

        public void AddPoint(Vertex point)
        {
            Vertices.Add(point);
            _pointsCount = Vertices.Count;
        }

        public void AddPath(Path path)
        {
            Paths.Add(path);
        }

        public void Draw()
        {
            Paths.ForEach(l =>
            {
                l.Draw();
            });
            Vertices.ForEach(p =>
            {
                p.Draw();
            });
        }
        
        public Vertex? GetPointByPos(Vector2 mousePosition) => 
            Vertices.Find(c => c.Circle.IsIn(mousePosition));

        public void CalculatePath(bool isShortest) 
        {  
            for (var point = 0; point <= _pointsCount; point++)
            {
                var lines = Paths.FindAll(l => l.Start.Number == point);
                foreach (var line in lines)
                {
                    line.Start = Vertices[point];
                    line.End = Vertices[line.End.Number];
                    
                    if (Vertices[line.End.Number].Weight == 0
                        || (isShortest ? line.Start.Weight + line.Weight < Vertices[line.End.Number].Weight 
                            : line.Start.Weight + line.Weight > Vertices[line.End.Number].Weight))
                    {
                        Vertices[line.End.Number].Weight = line.Start.Weight + line.Weight;
                        Vertices[line.End.Number].PreviousVertex = Vertices[line.Start.Number];
                    }

                    line.End = Vertices[line.End.Number];
                    line.Start = Vertices[point];
                }
            }
        }
        
        public void PrintPath(bool isShortest)
        {
            var tempList = new List<Vertex>();
            var point = Vertices[^1];
            do
            {
                tempList.Add(point);
                point = point.PreviousVertex;
            } while (point != null);
            tempList.Reverse();
            tempList.ForEach(p =>
            {
                p.Circle.FillColor = isShortest ? Color.LightGreen : Color.OrangeRed;
                var path = Paths.Find(path => path.Start == p.PreviousVertex && path.End == p);
                if (path != null) path.Line.Color = isShortest ? Color.LightGreen : Color.OrangeRed;
            });
        }
    }
}