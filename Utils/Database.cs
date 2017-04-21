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

        /// <summary>
        /// Initializes a new instance of thde Database class.
        /// </summary>
        /// <param name="name">The name of the database.</param>
        public Database(string name)
        {
            this.name = $"{name}.bdb";
            nodes = new List<DatabaseNode>();

            Load();
        }

        /// <summary>
        /// Saves the content of the database to the filesystem.
        /// </summary>
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

            Logger.Info($"Saved '{nodes.Count}' node(s) to '{name}'.", GetType());
        }

        /// <summary>
        /// Loads the content of the database from the filesystem.
        /// </summary>
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

            Logger.Info($"Loaded '{count}' node(s) from '{name}'.", GetType());
        }

        /// <summary>
        /// Deletes all entry from the database.
        /// </summary>
        public void Clear()
        {
            nodes.Clear();
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

        /// <summary>
        /// Gets the value of a node by its key.
        /// </summary>
        /// <param name="key">The key of the node.</param>
        public object Get(string key)
        {
            return GetNode(key).Value;
        }

        /// <summary>
        /// Sets the value of a node by its key.
        /// </summary>
        /// <param name="key">The key of the node.</param>
        /// <param name="value">The value of the node.</param>
        public void Set(string key, object value)
        {
            SetNode(key, new DatabaseNode { Key = key, Value = value });
        }

        /// <summary>
        /// Whether the node exists in the database.
        /// </summary>
        /// <param name="key">The key of the node.</param>
        public bool Exists(string key)
        {
            return GetNode(key).Value != null;
        }
    }
}
