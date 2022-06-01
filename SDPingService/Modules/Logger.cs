using SDPingService.Classes;
using SDPingService.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SDPingService.Modules
{
    internal class Logger
    {
        internal List<LogClass> log = null;
        internal LogClass lastLog = null;

        internal Logger() 
        {
            TryLoadLogger();
            TryLoadLastLog();
        }

        // Загрузка последнего лога
        internal bool TryLoadLastLog()
        {
            if (!File.Exists($@"LastLog.xml"))
                return false;
            lastLog = XMLConverterUtil.Deserialize<LogClass>(File.ReadAllText($@"LastLog.xml"));
            return true;
        }

        private bool SaveLastLog()
        {
            LogClass logClass;
            if (log.Count == 0) logClass = new LogClass();
            else logClass = log[log.Count - 1];
            string logText = XMLConverterUtil.Serialize(logClass);
            File.WriteAllText($@"LastLog.xml", logText);
            return true;
        }

        private bool TryLoadLogger()
        {
            if (!File.Exists($@"logs.xml"))
                return GenerateLog();
            List<LogClass> logClass = XMLConverterUtil.Deserialize<List<LogClass>>(File.ReadAllText($@"logs.xml"));
            return ReadInfo(logClass);
        }

        private bool GenerateLog()
        {
            List<LogClass> logClass = new List<LogClass>();
            string logText = XMLConverterUtil.Serialize(logClass);
            File.WriteAllText($@"logs.xml", logText);
            return ReadInfo(logClass);
        }

        private bool SaveLog()
        {
            string logText = XMLConverterUtil.Serialize(log);
            File.WriteAllText($@"logs.xml", logText);
            return true;
        }

        private bool ReadInfo(List<LogClass> logClass)
        {
            log = logClass;
            return true;
        }

        internal void AddNewLog(LogClass logClass)
        {
            log.Add(logClass);
            lastLog = logClass;
            CheckMax();
            SaveLog();
            SaveLastLog();
        }

        private void CheckMax()
        {
            if (log.Count <= Program._config.configuration.max) return;
            while (log.Count > Program._config.configuration.max) log.RemoveAt(0);
        }
    }
}
