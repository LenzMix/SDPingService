using Npgsql;
using SDPingService.Classes;
using SDPingService.Types;
using System.Net.NetworkInformation;

namespace SDPingService.Modules
{
    internal class ActionPings
    {

        internal WebsitePingInformation PingWebsite(string link)
        {
            Ping ping = new Ping();
            try
            {
                System.Net.NetworkInformation.PingReply pingReply = ping.Send(link);
                //return new WebsitePingInformation(link, pingReply.RoundtripTime, pingReply.Status);
                return new WebsitePingInformation
                {
                    target = link,
                    ping = pingReply.RoundtripTime,
                    status = pingReply.Status
                };
            }
            catch
            {
                //return new WebsitePingInformation(link, 999, IPStatus.Unknown);
                return new WebsitePingInformation
                {
                    target = link,
                    ping = 999,
                    status = IPStatus.Unknown
                };
            }
        }

        internal PostgresPingInformation PingPostgres(string link)
        {
            string connectionString = link;
            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            try
            {
                npgSqlConnection.Open();
                string host = npgSqlConnection.Host;
                int port = npgSqlConnection.Port;
                //return new PostgresPingInformation($"{host}:{port}", 0, EResponseType.success);
                return new PostgresPingInformation
                {
                    target = $"{host}:{port}",
                    ping = 0,
                    status = EResponseType.Success
                };
            }
            catch
            {
                //return new PostgresPingInformation($"Нет данных", 0, EResponseType.error);
                return new PostgresPingInformation
                {
                    target = $"Нет данных",
                    ping = 0,
                    status = EResponseType.Error
                };
            }
        }
    }
}
