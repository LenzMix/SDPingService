using SDPingService.Classes;
using SDPingService.Modules;
using System;
using System.Collections.Generic;

namespace SDPingService
{
    class Program
    {
        public static Configuration _config;
        public static Logger _log;
        public static ActionPings _action;
        public static MailSender _mail;

        static void Main(string[] args)
        {
            Console.WriteLine($"SD Ping Service - 1.0.0");
            _log = new Logger();
            if (args.Length == 0)
            {
                _config = new Configuration();
                _action = new ActionPings();
                _mail = new MailSender();
                PingMethod();
                if (!_mail.SendMessage(_log.lastLog))
                {
                    Console.WriteLine("Отправка результата не удалась. Возможно, вы ввели неверные данные для подключения к SMTP. Нажмите любую кнопку чтобы закрыть программу");
                    Console.ReadKey();
                }
                return;
            }
            if (!_log.TryLoadLastLog()) Console.WriteLine("Файл с последним сохранённым результатом проверки не найден!");
            else
            {
                Console.WriteLine("Последний результат проверки: ");
                _log.lastLog.ReadLog();
            }
            Console.ReadKey();
            return;
        }

        private static void PingMethod()
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
        }
    }
}
