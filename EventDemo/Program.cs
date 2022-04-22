
using System;

namespace EventDemo
{
    class Program
    {
        private static ConfigFileWatcher _configFileWatcher;
        static void Main()
        {
            _configFileWatcher = ConfigFileWatcher.GetInstance();
            Console.ReadLine();
        }

    }
    
}
