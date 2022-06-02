using SDPingService.Types;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Xml.Serialization;

namespace SDPingService.Classes
{
    [Serializable]
    public class LogClass
    {
        [XmlAttribute]
        public DateTime time;

        public List<WebsitePingInformation> WebsitePings;
        public List<PostgresPingInformation> PostgresPings;

        private long CountAvgPing()
        {
            long x = 0;
            foreach (WebsitePingInformation p in WebsitePings.FindAll(x => x.status == IPStatus.Success)) x += p.ping;
            return x / WebsitePings.FindAll(x => x.status == IPStatus.Success).Count;
        }

        public void ReadLog()
        {
            long avgPing = !WebsitePings.Exists(x => x.status == IPStatus.Success) ? 0 : CountAvgPing();
            Console.WriteLine($"[{time.Day.ToString("00")}.{time.Month.ToString("00")}.{time.Year.ToString("0000")} {time.Hour.ToString("00")}:{time.Minute.ToString("00")}:{time.Second.ToString("00")}]");
            Console.WriteLine($"  Сайты: Успешное соединение: {WebsitePings.FindAll(x => x.status == IPStatus.Success).Count} | Соединение провалено: {WebsitePings.FindAll(x => x.status != IPStatus.Success).Count} | Всего сайтов: {WebsitePings.Count} | Средний ping: {avgPing}ms");
            foreach (WebsitePingInformation website in WebsitePings) Console.WriteLine($"    {website.target} | PING: {website.ping}ms | Статус: {website.status}");
            Console.WriteLine($"  Базы данных: Успешное соединение: {PostgresPings.FindAll(x => x.status == EResponseType.Success).Count} | Соединение провалено: {PostgresPings.FindAll(x => x.status == EResponseType.Error).Count} | Всего баз данных: {PostgresPings.Count}");
            foreach (PostgresPingInformation database in PostgresPings) Console.WriteLine($"    {database.target} | Статус: {database.status}");
            Console.WriteLine();
        }

        internal List<string> AddLog()
        {
            List<string> listOfInfo = new List<string>();
            long avgPing = !WebsitePings.Exists(x => x.status == IPStatus.Success) ? 0 : CountAvgPing();
            listOfInfo.Add($"[{time.Day.ToString("00")}.{time.Month.ToString("00")}.{time.Year.ToString("0000")} {time.Hour.ToString("00")}:{time.Minute.ToString("00")}:{time.Second.ToString("00")}]");
            listOfInfo.Add($"  Сайты: Успешное соединение: {WebsitePings.FindAll(x => x.status == IPStatus.Success).Count} | Соединение провалено: {WebsitePings.FindAll(x => x.status != IPStatus.Success).Count} | Всего сайтов: {WebsitePings.Count} | Средний ping: {avgPing}ms");
            foreach (WebsitePingInformation website in WebsitePings) listOfInfo.Add($"    {website.target} | PING: {website.ping}ms | Статус: {website.status}");
            listOfInfo.Add($"  Базы данных: Успешное соединение: {PostgresPings.FindAll(x => x.status == EResponseType.Success).Count} | Соединение провалено: {PostgresPings.FindAll(x => x.status == EResponseType.Error).Count} | Всего баз данных: {PostgresPings.Count}");
            foreach (PostgresPingInformation database in PostgresPings) listOfInfo.Add($"    {database.target} | Статус: {database.status}");
            listOfInfo.Add("");
            return listOfInfo;
        }

        internal string HTMLLog()
        {
            string htmlString = "";
            long avgPing = !WebsitePings.Exists(x => x.status == IPStatus.Success) ? 0 : CountAvgPing();
            htmlString += $"<h2>С компьютера {Environment.MachineName} ({Environment.UserName}) была совершена проверка отклика сайтов и баз данных [{time.Day.ToString("00")}.{time.Month.ToString("00")}.{time.Year.ToString("0000")} {time.Hour.ToString("00")}:{time.Minute.ToString("00")}:{time.Second.ToString("00")}]</h2>";
            htmlString += $"<h3>  Сайты: Успешное соединение: {WebsitePings.FindAll(x => x.status == IPStatus.Success).Count} | Соединение провалено: {WebsitePings.FindAll(x => x.status != IPStatus.Success).Count} | Всего сайтов: {WebsitePings.Count} | Средний ping: {avgPing}ms</h3>" +
                $"<table><tr><th>Сайт</th><th>Отклик</th><th>Статус</th></tr>";
            foreach (WebsitePingInformation website in WebsitePings) htmlString += $"<tr><td>{website.target}</td><td>{website.ping}ms</td><td>{website.status}</td></tr>";
            htmlString += $"</table><h3>  Базы данных: Успешное соединение: {PostgresPings.FindAll(x => x.status == EResponseType.Success).Count} | Соединение провалено: {PostgresPings.FindAll(x => x.status == EResponseType.Error).Count} | Всего баз данных: {PostgresPings.Count}</h3>" +
                $"<table><tr><th>База данных</th><th>Статус</th></tr>";
            foreach (PostgresPingInformation database in PostgresPings) htmlString += $"<tr><td>{database.target}</td><td>{database.status}</td></tr>";
            htmlString += "</table>";
            return htmlString;
        }
    }
}
