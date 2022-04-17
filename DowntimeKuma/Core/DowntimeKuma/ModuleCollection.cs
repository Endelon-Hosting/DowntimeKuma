using DowntimeKuma.Core.DowntimeKuma;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DowntimeKuma.Core.DowntimeKuma
{
    public class ModuleCollection<T> : List<T>, IDictionary<string, T> where T : AbstractModule
    {
        public T this[string key] 
        {
            get
            {
                return Find(x => x.Id == key);
            }
            set
            {
                var i = FindIndex(x => x.Id == key);
                this[i] = value;
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                List<string> n = new();
                foreach (var v in this)
                {
                    n.Add(v.Id);
                }

                return n;
            }
        }

        public ICollection<T> Values => this;

        public bool IsReadOnly => false;

        public void Add(string key, T value)
        {
            Add(value);
        }

        public void Add(KeyValuePair<string, T> item)
        {
            Add(item.Value);
        }

        public bool Contains(KeyValuePair<string, T> item)
        {
            return Contains(item.Value);
        }

        public bool ContainsKey(string key)
        {
            return Find(x => x.Id == key) != null;
        }

        public void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
        {
            Dictionary<string, T> dic = new();
            foreach (var v in this)
            {
                dic.Add(v.Id, v);
            }
            dic.ToList().CopyTo(array, arrayIndex);
        }

        public bool Remove(string key)
        {
            return Remove(Find(x => x.Id == key));
        }

        public bool Remove(KeyValuePair<string, T> item)
        {
            return Remove(item.Value);
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out T value)
        {
            var o = Find(x => x.Id == key);
            if (o != null)
            {
                value = o;
                return true;
            }
            value = null;
            return false;
        }

        IEnumerator<KeyValuePair<string, T>> IEnumerable<KeyValuePair<string, T>>.GetEnumerator()
        {
            Dictionary<string, T> dic = new();
            foreach (var v in this)
            {
                dic.Add(v.Id, v);
            }
            return dic.GetEnumerator();
        }
    }
}