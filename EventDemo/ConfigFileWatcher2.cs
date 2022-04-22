using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Caching;

namespace EventDemo
{
    public class ConfigFileWatcher
    {
        private readonly MemoryCache _memCache;
        private readonly CacheItemPolicy _cacheItemPolicy;
        private const int CacheTimeMilliseconds = 500;

        private static readonly ConfigFileWatcher _configFileWatcher = new ConfigFileWatcher();
        private FileSystemWatcher _fileSystemWatcher;

        private ConfigFileWatcher()
        {
            _memCache = MemoryCache.Default;

            _cacheItemPolicy = new CacheItemPolicy
            {
                RemovedCallback = OnRemovedFromCache
            };

            _fileSystemWatcher = new FileSystemWatcher(@"C:\temp")  // TODO: change to current path
            {
                NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Size
            };

            _fileSystemWatcher.Changed += OnChanged;
            _fileSystemWatcher.Filter = "*.json";
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        // Add file event to cache (won't add if already there so assured of only one occurance)
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            _cacheItemPolicy.AbsoluteExpiration = DateTimeOffset.Now.AddMilliseconds(CacheTimeMilliseconds);
            _memCache.AddOrGetExisting(e.Name, e, _cacheItemPolicy);
        }

        // Handle cache item expiring 
        private void OnRemovedFromCache(CacheEntryRemovedArguments args)
        {
            if (args.RemovedReason != CacheEntryRemovedReason.Expired)
            {
                return;
            }
            var e = (FileSystemEventArgs)args.CacheItem.Value;

            // now to respond to the event
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                // we only care about changed files
                return;
            }
            if (e.Name == "endpoints.json" )
            {
                Debug.WriteLine($"Changed: {e.FullPath}");
            }
        }

        // TODO Inject EndpointManager here
        public static ConfigFileWatcher GetInstance() => _configFileWatcher;
    }
}
