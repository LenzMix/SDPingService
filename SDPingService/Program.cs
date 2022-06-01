using SDPingService.Classes;
using SDPingService.Modules;
using SDPingService.Types;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace SDPingService
{
    class Program
    {
        public static Thread ping;
        public static Configuration _config;
        public static Logger _log;
        public static ActionPings _action;

        static void Main(string[] args)
        {
            Console.WriteLine($"SD Ping Service - 1.0.0");
            if (args.Length == 0)
            {
                _config = new Configuration();
                _action= new ActionPings();
                _log = new Logger();
                ping = new Thread(() => PingMethod());
                ping.Start();
                return;
            }
            _log = new Logger();
            if (!_log.TryLoadLastLog()) Console.WriteLine("Файл с последним сохранённым результатом проверки не найден!");
            else
            {
                Console.WriteLine("Последний результат проверки: ");
                _log.lastLog.ReadLog();
            }
            Console.ReadKey();
            return;
        }

        private static Task PingMethod()
        {
            while (true)
            {
                List<WebsitePingInformation> websitesResponses = new List<WebsitePingInformation>();
                List<PostgresPingInformation> postgresResponses = new List<PostgresPingInformation>();
                foreach (string link in _config.configuration.websitesLinks) websitesResponses.Add(_action.PingWebsite(link));
                foreach (string link in _config.configuration.postgresConnections) postgresResponses.Add(_action.PingPostgres(link));

                DateTime time = DateTime.Now;
                LogClass newLog = new LogClass
                {
                    time = time,
                    WebsitePings = websitesResponses,
                    PostgresPings = postgresResponses,
                };

                newLog.ReadLog();
                _log.AddNewLog(newLog);

                Thread.Sleep((int)_config.configuration.timer * 1000);
            }
        }
    }
}
