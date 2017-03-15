using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;

namespace BareKit
{
    public struct DatabaseNode
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }

    public class Database
    {
        string name;
        IsolatedStorageFile storage;
        List<DatabaseNode> nodes;

        public Database(string name)
        {
            this.name = $"{name}.bdb";
            storage = IsolatedStorageFile.GetUserStoreForApplication();
            nodes = new List<DatabaseNode>();

            Load();
        }

        public void Save()
        {
            StreamWriter writer = new StreamWriter(new IsolatedStorageFileStream(name, FileMode.Create, storage));
            foreach (DatabaseNode node in nodes)
                writer.WriteLine($"{node.Key}={node.Value}");
            writer.Dispose();

            Logger.Info(GetType(), $"Saved '{nodes.Count}' node(s) to '{name}'.");
        }

        public void Load()
        {
            nodes.Clear();

            int count = 0;
            if (storage.FileExists(name))
            {
                StreamReader reader = new StreamReader(new IsolatedStorageFileStream(name, FileMode.Open, storage));
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string key = line.Split('=')[0];
                    string value = line.Split('=')[1];

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

        DatabaseNode getNode(string key)
        {
            foreach (DatabaseNode node in nodes)
            {
                if (node.Key == key)
                    return node;
            }
            return default(DatabaseNode);
        }

        void setNode(string key, DatabaseNode node)
        {
            DatabaseNode tempNode = getNode(key);
            if (tempNode.Value != null)
                nodes[nodes.IndexOf(tempNode)] = node;
            else
                nodes.Add(node);
        }

        public object Get(string key)
        {
            return getNode(key).Value;
        }

        public void Set(string key, object value)
        {
            setNode(key, new DatabaseNode { Key = key, Value = value });
        }

        public bool Exists(string key)
        {
            return getNode(key).Value != null;
        }
    }
}
