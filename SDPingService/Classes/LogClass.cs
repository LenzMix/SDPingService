using SDPingService.Types;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
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

        public void ReadLog()
        {

            long CountAvgPing()
            {
                long x = 0;
                foreach (WebsitePingInformation p in WebsitePings.FindAll(x => x.status == IPStatus.Success)) x += p.ping;
                return x / WebsitePings.FindAll(x => x.status == IPStatus.Success).Count;
            }
            long avgPing = !WebsitePings.Exists(x => x.status == IPStatus.Success) ? 0 : CountAvgPing();
            Console.WriteLine($"[{time.Day.ToString("00")}.{time.Month.ToString("00")}.{time.Year.ToString("0000")} {time.Hour.ToString("00")}:{time.Minute.ToString("00")}:{time.Second.ToString("00")}]");
            Console.WriteLine($"  Сайты: Успешное соединение: {WebsitePings.FindAll(x => x.status == IPStatus.Success).Count} | Соединение провалено: {WebsitePings.FindAll(x => x.status != IPStatus.Success).Count} | Всего сайтов: {WebsitePings.Count} | Средний ping: {avgPing}ms");
            foreach (WebsitePingInformation website in WebsitePings) Console.WriteLine($"    {website.target} | PING: {website.ping}ms | Статус: {website.status}");
            Console.WriteLine($"  Базы данных: Успешное соединение: {PostgresPings.FindAll(x => x.status == EResponseType.success).Count} | Соединение провалено: {PostgresPings.FindAll(x => x.status == EResponseType.error).Count} | Всего баз данных: {PostgresPings.Count}");
            foreach (PostgresPingInformation website in PostgresPings) Console.WriteLine($"    {website.target} | Статус: {website.status}");
            Console.WriteLine();
        }
    }
}
