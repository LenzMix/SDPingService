using SDPingService.Classes;
using SDPingService.Utils;
using System.IO;

namespace SDPingService.Modules
{
    internal class Logger
    {
        internal LogClass lastLog = null;

        internal Logger() 
        {
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

        private bool SaveLastLog(LogClass log)
        {
            string logText = XMLConverterUtil.Serialize(log);
            File.WriteAllText($@"LastLog.xml", logText);
            return true;
        }


        // Загрузка логов
        private bool GenerateLog()
        {
            File.WriteAllText($@"SDPingService.log", "");
            return true;
        }

        private bool SaveLog(LogClass log)
        {
            if (!File.Exists($@"SDPingService.log"))
                GenerateLog();
            string logText = XMLConverterUtil.Serialize(log);
            File.AppendAllLines($@"SDPingService.log", log.AddLog());
            return true;
        }

        internal void AddNewLog(LogClass logClass)
        {
            SaveLastLog(logClass);
            SaveLog(logClass);
        }
    }
}
