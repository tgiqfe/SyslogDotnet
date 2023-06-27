﻿using SyslogDotnet.Lib.Syslog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace SyslogDotnet.Lib.Config
{
    public class SettingCollection
    {
        public Setting Setting { get; set; }

        public static SettingCollection Deserialize(string path)
        {
            if (path == null) { return new SettingCollection(); }

            SettingCollection collection = null;
            try
            {
                var yml = File.ReadAllText(path);
                collection = new Deserializer().Deserialize<SettingCollection>(yml);
            }
            catch { }
            if (collection == null)
            {
                collection = new();
            }
            return collection;
        }

        public void Serialize(string path)
        {
            string parent = Path.GetDirectoryName(path);
            if (!Directory.Exists(parent))
            {
                Directory.CreateDirectory(parent);
            }
            string yml = new Serializer().Serialize(this);
            File.WriteAllText(path, yml);
        }

        public Setting.SettingServerRule GetMatchServerRule(Facility facility, Severity severity)
        {
            return Setting.Server.Rules.
                FirstOrDefault(x => x.Value.IsMatch(facility, severity)).Value;
        }

        public Setting.SettingClientRule GetMatchClientRule(string name)
        {
            return Setting.Client.Rules.FirstOrDefault(x => x.Value.Equals(name)).Value;
        }
    }
}
