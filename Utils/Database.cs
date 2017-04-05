using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;

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
        readonly IsolatedStorageFile storage;
        readonly List<DatabaseNode> nodes;

        public Database(string name)
        {
            this.name = $"{name}.bdb";
            storage = IsolatedStorageFile.GetUserStoreForAssembly();
            nodes = new List<DatabaseNode>();

            Load();
        }

        public void Save()
        {
            var writer = new StreamWriter(new IsolatedStorageFileStream(name, FileMode.Create, storage));
            foreach (var node in nodes)
                writer.WriteLine($"{node.Key}={node.Value}");
            writer.Dispose();

            Logger.Info(GetType(), $"Saved '{nodes.Count}' node(s) to '{name}'.");
        }

        public void Load()
        {
            nodes.Clear();

            var count = 0;
            if (storage.FileExists(name))
            {
                var reader = new StreamReader(new IsolatedStorageFileStream(name, FileMode.Open, storage));
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var key = line?.Split('=')[0];
                    var value = line?.Split('=')[1];

                    nodes.Add(new DatabaseNode { Key = key, Value = value });

                    count++;
                }
                reader.Dispose();
            }

            Logger.Info(GetType(), $"Loaded '{count}' node(s) from '{name}'.");
        }

        public void Clear()
        {
            storage.DeleteFile(name);
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
