using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog;
using NLog.Layouts;
using NLog.Targets;

namespace BankDataDownloader.Core.Service.Impl
{
    public class LogService : ILogService {
        public IEnumerable<string> GetLogFilePaths()
        {
            var config = LogManager.Configuration;
            return config.AllTargets.Where(target => target.GetType() == typeof(FileTarget))
                .Cast<FileTarget>()
                .Select(target => target.FileName)
                .Cast<SimpleLayout>()
                .Select(layout => Path.GetDirectoryName(layout.FixedText))
                .Distinct()
                .SelectMany(Directory.GetFiles);
        }

        public string GetLogs(string path)
        {
            return File.ReadAllText(path);
        }
    }
}