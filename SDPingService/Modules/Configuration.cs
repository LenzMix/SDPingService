using SDPingService.Classes;
using SDPingService.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace SDPingService.Modules
{
    internal class Configuration
    {
        internal ConfigurationClass configuration = null;

        internal Configuration() 
        {
            if (!TryLoadConfiguration()) GenerateConfig();
        }

        private bool TryLoadConfiguration()
        {
            if (!File.Exists($@"config.xml"))
                return GenerateConfig();
            Console.WriteLine("Конфигурация обнаружена - Загружаю конфигурацию");
            ConfigurationClass confClass = XMLConverterUtil.Deserialize<ConfigurationClass>(File.ReadAllText($@"config.xml"));
            Console.WriteLine("Конфигурация загружена успешно");
            return ReadInfo(confClass);
        }

        private bool GenerateConfig()
        {
            Console.WriteLine("Конфигурация не обнаружена - Создаю новую конфигурацию");
            ConfigurationClass confClass = new ConfigurationClass
            {
                directory = @"",
                max = 1000,
                timer = 5f,
                postgresConnections = new List<string> { "Server=127.0.0.1;Port=5432;Database=postgres;UID=postgres;PWD=--123--123--" },
                websitesLinks = new List<string> { "ya.ru" }
            };
            string confText = XMLConverterUtil.Serialize(confClass);
            File.WriteAllText($@"config.xml", confText);
            Console.WriteLine("Конфигурация сохранена");
            return ReadInfo(confClass);
        }

        private bool ReadInfo(ConfigurationClass confClass)
        {
            configuration = confClass;
            return true;
        }
    }
}
