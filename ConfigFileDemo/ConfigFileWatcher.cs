using System;
using System.Diagnostics;
using System.IO;

namespace ConfigFileChecker
{
    public class ConfigFileWatcher
    {
        public ConfigFileWatcher(string path)
        {

        }

        public FileSystemWatcher Watcher;

        public void Start()
        {
            Watcher = new FileSystemWatcher(@"C:\temp\mon");

            Watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Size;

            Watcher.Changed += OnChanged;
            Watcher.Filter = "*.json";
            Watcher.EnableRaisingEvents = true;
            Debug.WriteLine($"Started:");
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            //EventHandler handler = OnConfigChanged;
            //handler?.Invoke(this, e);
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            Debug.WriteLine($"Changed: {e.FullPath}");
        }
    }
}
