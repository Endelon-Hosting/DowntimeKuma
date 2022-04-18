
using Microsoft.JSInterop;

using System;
using System.Collections.Generic;
using System.Linq;

namespace DowntimeKuma.S3
{
    public class S3Storage
    {
        public static Dictionary<string, S3Storage> Sessions { get; set; }
            = new Dictionary<string, S3Storage>();

        public Dictionary<string, object> Storage { get; set; }

        public S3Storage()
        {
            Storage = new Dictionary<string, object>();
        }

        public void Add(string key, object data)
        {
            lock (Storage)
            {
                Storage.Add(key, data);
            }
        }

        public bool ContainsKey(string key)
        {
            lock (Storage)
            {
                return Storage.ContainsKey(key);
            }
        }

        public void Remove(string key)
        {
            lock (Storage)
            {
                if (Storage.ContainsKey(key))
                {
                    Storage.Remove(key);
                }
            }
        }

        public void Set(string key, object data)
        {
            if (ContainsKey(key))
            {
                lock (Storage)
                {
                    Storage[key] = data;
                }
            }
            else
            {
                Add(key, data);
            }
        }

        public T Get<T>(string key)
        {
            if (ContainsKey(key))
            {
                lock (Storage)
                {
                    return (T)Storage[key];
                }
            }
            else
                return default;
        }

        public static bool IsTokenValid(string token)
        {
            lock (Sessions)
            {
                byte[] data = Convert.FromBase64String(token);
                DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));

                if (when < DateTime.UtcNow.AddHours(-24))
                {
                    return false;
                }
                else
                {
                    return Sessions.ContainsKey(token);
                }
            }
        }

        private static string GenerateToken()
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string token = Convert.ToBase64String(time.Concat(key).ToArray());
            return token;
        }

        public static S3Storage Create(out string token)
        {
            lock (Sessions)
            {
                S3Storage st = new();
                token = GenerateToken();

                Sessions.Add(token, st);

                return st;
            }
        }

        public static S3Storage Get(string token)
        {
            lock (Sessions)
            {
                return Sessions[token];
            }
        }

        public static void Clear(string token)
        {
            lock (Sessions)
            {
                Sessions.Remove(token);
            }
        }
    }
}
