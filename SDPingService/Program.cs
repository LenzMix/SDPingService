using SDPingService.Classes;
using SDPingService.Modules;
using System;
using System.Collections.Generic;

namespace SDPingService
{
    class Program
    {
        public static Configuration config;
        public static Logger logger;

        static void Main(string[] args)
        {
            Console.WriteLine($"SD Ping Service - 1.1.1");
            logger = new Logger();
            if (args.Length == 0)
            {
                config = new Configuration();
                MailSender _mail = new MailSender();
                PingMethod();
                if (!_mail.SendMessage(logger.lastLog))
                {
                    Console.WriteLine("Отправка результата не удалась. Возможно, вы ввели неверные данные для подключения к SMTP. Нажмите любую кнопку чтобы закрыть программу");
                    Console.ReadKey();
                }
                return;
            }
            if (!logger.TryLoadLastLog()) Console.WriteLine("Файл с последним сохранённым результатом проверки не найден!");
            else
            {
                Console.WriteLine("Последний результат проверки: ");
                logger.lastLog.ReadLog();
            }
            Console.ReadKey();
            return;
        }

        private static void PingMethod()
        {
            List<WebsitePingInformation> websitesResponses = new List<WebsitePingInformation>();
            List<PostgresPingInformation> postgresResponses = new List<PostgresPingInformation>();
            ActionPings action = new ActionPings();
            foreach (string link in config.configuration.websitesLinks) websitesResponses.Add(action.PingWebsite(link));
            foreach (string link in config.configuration.postgresConnections) postgresResponses.Add(action.PingPostgres(link));

            DateTime time = DateTime.Now;
            LogClass newLog = new LogClass
            {
                time = time,
                WebsitePings = websitesResponses,
                PostgresPings = postgresResponses,
            };

            newLog.ReadLog();
            logger.AddNewLog(newLog);
        }
    }
}
