using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BareKit
{
    public struct DatabaseNode
    {
        /// <summary>
        /// Gets or sets the nodes key.
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Gets or sets the nodes value.
        /// </summary>
        public object Value { get; set; }
    }

    public class Database
    {
        readonly string name;
        readonly List<DatabaseNode> nodes;

        public Database(string name)
        {
            this.name = $"{name}.bdb";
            nodes = new List<DatabaseNode>();

            Load();
        }

        public void Save()
        {
            var writer = new StreamWriter(Storage.Write(name));
            foreach (var node in nodes)
            {
                var reversed = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{node.Key}={node.Value}")).Replace("=", ":").ToCharArray();
                Array.Reverse(reversed);
                writer.WriteLine(new string(reversed));
            }
            writer.Dispose();

            Logger.Info(GetType(), $"Saved '{nodes.Count}' node(s) to '{name}'.");
        }

        public void Load()
        {
            nodes.Clear();

            var count = 0;
            if (Storage.Exists(name))
            {
                var reader = new StreamReader(Storage.Read(name));
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line == null || line.StartsWith("#")) continue;

                    var reversed = line.Replace(":", "=").ToCharArray();
                    Array.Reverse(reversed);
                    line = Encoding.UTF8.GetString(Convert.FromBase64String(new string(reversed)));

                    var key = line.Split('=')[0];
                    var value = line.Split('=')[1];
                    nodes.Add(new DatabaseNode {Key = key, Value = value});

                    count++;
                }
                reader.Dispose();
            }

            Logger.Info(GetType(), $"Loaded '{count}' node(s) from '{name}'.");
        }

        public void Clear()
        {
            Storage.Delete(name);
        }

        DatabaseNode GetNode(string key)
        {
            foreach (var node in nodes)
            {
                if (node.Key == key)
                    return node;
            }
            return default(DatabaseNode);
        }

        void SetNode(string key, DatabaseNode node)
        {
            var tempNode = GetNode(key);
            if (tempNode.Value != null)
                nodes[nodes.IndexOf(tempNode)] = node;
            else
                nodes.Add(node);
        }

        public object Get(string key)
        {
            return GetNode(key).Value;
        }

        public void Set(string key, object value)
        {
            SetNode(key, new DatabaseNode { Key = key, Value = value });
        }

        public bool Exists(string key)
        {
            return GetNode(key).Value != null;
        }
    }
}
