using System;
using System.Collections.Generic;
using System.IO;
using Core;
using Newtonsoft.Json;

namespace Algorithm
{
    public static class DataManager
    {
        public static void SaveData(Graph graph)
        {
            // Save List<Vertex>, List<Path>
            if (!Directory.Exists("Data"))
            {
                var di = Directory.CreateDirectory("Data");
            }

            // Try to create the directory.
            var vertices = graph.Vertices; 
            var paths = graph.Paths; 

            var pathsJson = JsonConvert.SerializeObject(paths, Formatting.Indented);
            var verticesJson = JsonConvert.SerializeObject(vertices, Formatting.Indented);

            var writer = new StreamWriter("Data/PathsJson.json");
            writer.WriteLine(pathsJson);
            writer.Close();
            writer = new StreamWriter("Data/VerticesJson.json");
            writer.WriteLine(verticesJson);
            writer.Close();
        }

        public static Graph LoadData()
        {
            if (!File.Exists("Data/PathsJson.json") && !File.Exists("Data/VerticesJson.json"))
                return new Graph();
            
            var reader = new StreamReader("Data/PathsJson.json");
            var pathsJson = reader.ReadToEnd();
            reader.Close();
            reader = new StreamReader("Data/VerticesJson.json");
            var verticesJson = reader.ReadToEnd();
            reader.Close();
            
            var vertices = JsonConvert.DeserializeObject<List<Vertex>>(verticesJson);
            var paths = JsonConvert.DeserializeObject<List<Path>>(pathsJson);

            if (vertices != null && paths != null)
                return new Graph(paths, vertices);
            return new Graph();
        }
    }
}