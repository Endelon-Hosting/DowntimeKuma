using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace DowntimeKuma.Core.DowntimeKuma
{
    public class Monitor
    {
        private static int _id = 0;
        public string MonitoringModule { get; set; }
        public string Name { get; set; }
        public string Target { get; set; }
        public int Id { get; set; } = _id++;
        public List<string> NotifyModules { get; set; } = new();

        public AbstractMonitorModule GetMonitoringService()
        {
            return DKControll.Monitors.Find(x => x.Id == MonitoringModule);
        }

        private string Now()
        {
            var n = DateTime.Now;
            return $"{n.Day}.{n.Month}.{n.Year}";
        }

        private string DailyStatsPath()
        {
            return $"{PathX()}history/{Now()}.json.gz";
        }

        private string PathX()
        {
            return $"data/monitors/{Id.ToString().ToLower().Replace(" ", "_")}-{MonitoringModule.ToLower().Replace(" ", "_")}/";
        }

        private string XD(string fn)
        {
            var dir = Path.GetDirectoryName(fn);
            if(!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return fn;
        }

        private string JX(string fn, string m = "{}")
        {
            if (!File.Exists(fn))
                File.WriteAllBytes(fn, Compress(Encoding.UTF8.GetBytes(m)));
            return fn;
        }

        public MonitorData[] GetHistory()
        {
            return JsonConvert.DeserializeObject<MonitorData[]>(Encoding.UTF8.GetString(Decompress(File.ReadAllBytes(JX(XD(DailyStatsPath()), "[]")))));
        }

        public void AddToHistory(MonitorData d)
        {
            var x = JsonConvert.DeserializeObject<MonitorData[]>(Encoding.UTF8.GetString(Decompress(File.ReadAllBytes(JX(XD(DailyStatsPath()), "[]"))))).ToList();
            x.Add(d);
            File.WriteAllBytes(DailyStatsPath(), Compress(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(x.ToArray(), Formatting.Indented))));
        }

        public static byte[] Compress(byte[] data)
        {
            MemoryStream output = new();
            using (DeflateStream dstream = new(output, CompressionMode.Compress))
            {
                dstream.Write(data, 0, data.Length);
            }
            return output.ToArray();
        }

        public static byte[] Decompress(byte[] data)
        {
            MemoryStream input = new(data);
            MemoryStream output = new();
            using (DeflateStream dstream = new(input, CompressionMode.Decompress))
            {
                dstream.CopyTo(output);
            }
            return output.ToArray();
        }

        public void UpdateConfig()
        {
            Directory.CreateDirectory(PathX());
            File.WriteAllText(PathX() + "config.json", JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public static Monitor FromConfig(string fn)
        {
            var o = JsonConvert.DeserializeObject<Monitor>(File.ReadAllText(fn));
            _id++;
            return o;
        }

        public void DeleteData()
        {
            Directory.Delete(PathX(), true);
        }
    }
}
