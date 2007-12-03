using System;
using System.Collections.Generic;
using System.Text;
using SJRAtlas.Core;
using System.Collections;

namespace SJRAtlas.CGNS
{
    public class CachedGnssLookup : IPlaceNameLookup
    {
        private LruCache<object, IPlaceName[]> cache;
        private GeographicalNameSearchService gnss;
        private int MAX_CACHE_SIZE = 100;

        public CachedGnssLookup(IGnssWebApi gnssWebApi)
        {
            gnss = new GeographicalNameSearchService(gnssWebApi);
            cache = new LruCache<object, IPlaceName[]>(MAX_CACHE_SIZE);
        }

        private void CacheResult(object key, IPlaceName[] result)
        {
            cache.Add(key, result);
        }

        #region ILookupService<IPlaceName> Members

        public IPlaceName Find(object id)
        {
            if (cache.ContainsKey(id))
                return cache[id][0];

            IPlaceName placename = gnss.Find(id);
            CacheResult(id, new IPlaceName[] { placename });
            return placename;
        }

        public IPlaceName[] FindAll()
        {
            return gnss.FindAll();
        }

        public IPlaceName[] FindByQuery(string query)
        {
            if (cache.ContainsKey(query))
                return cache[query];

            IPlaceName[] results = gnss.FindByQuery(query);
            CacheResult(query, results);
            return results;
        }

        public IPlaceName[] FindAllByProperty(string property, object value)
        {
            string key = String.Format("PROP:{0}=VAL:{1}", property, value);
            if (cache.ContainsKey(key))
                return cache[key];

            IPlaceName[] results = gnss.FindAllByProperty(property, value);
            CacheResult(key, results);
            return results;
        }

        #endregion
    }

    interface ICache : System.Collections.IEnumerable
    {
        int Capacity { get; set;}
        int Count { get;}
        void Clear();
        bool IsReadOnly { get;}
        int FlushCount { get;}
    }

    class LruCache<TKey, TValue> : IDictionary<TKey, TValue>, ICache
    {
        private class Entry
        {
            public TKey key;
            public TValue value;
            public Entry next;
            public Entry previous;

            public Entry(TKey key, TValue value)
            {
                this.key = key;
                this.value = value;
            }
        }

        private Entry anchor = new Entry(default(TKey), default(TValue));
        private Dictionary<TKey, Entry> entries = new Dictionary<TKey, Entry>();

        public LruCache(int capacity)
        {
            Capacity = capacity;
            Clear();
        }
        public LruCache(IDictionary<TKey, TValue> dict, int capacity)
            : this(capacity)
        {
            lock (this)
            {
                if (Capacity > dict.Count) Capacity = dict.Count;
                foreach (KeyValuePair<TKey, TValue> item in dict)
                {
                    Add(item);
                }
            }
        }

        public virtual void Add(TKey key, TValue value)
        {
            lock (this)
            {
                if (ContainsKey(key)) throw new ArgumentException("Key already in dictionary");
                Entry entry = new Entry(key, value);
                entries.Add(key, entry);
                entry.previous = anchor;
                entry.next = anchor.next;
                anchor.next = entry;
                entry.next.previous = entry;
                shrinkToCapacity();
                peakCount = PeakCount;
            }
        }

        private int capacity;
        public int Capacity
        {
            get { return capacity; }
            set { lock (this) { capacity = value; shrinkToCapacity(); } }
        }

        protected virtual bool EvictionNeeded
        {
            get { lock (this) { return Count > Capacity; } }
        }

        public int Count { get { lock (this) { return entries.Count; } } }
        public ICollection<TKey> Keys { get { lock (this) { return entries.Keys; } } }
        public bool ContainsKey(TKey key) { lock (this) { return entries.ContainsKey(key); } }

        private int flushCount;
        public int FlushCount { get { lock (this) { return flushCount; } } }

        private int peakCount;
        public int PeakCount
        {
            get
            {
                lock (this)
                {
                    return Count > peakCount ? Count : peakCount;
                }
            }
        }

        private void moveToTop(Entry entry)
        {
            lock (this)
            {
                entry.previous.next = entry.next;
                entry.next.previous = entry.previous;
                entry.previous = anchor;
                entry.next = anchor.next;
                anchor.next.previous = entry;
                anchor.next = entry;
            }
        }

        public virtual TValue this[TKey key]
        {
            get
            {
                lock (this)
                {
                    Entry entry = entries[key];
                    moveToTop(entry);
                    return entry.value;
                }
            }
            set
            {
                lock (this)
                {
                    if (!entries.ContainsKey(key))
                    {
                        Add(key, value);
                    }
                    else
                    {
                        Entry entry = (Entry)entries[key];
                        moveToTop(entry);
                        entry.value = value;
                    }
                }
            }
        }

        public bool Remove(TKey key)
        {
            lock (this)
            {
                if (!entries.ContainsKey(key)) return false;
                remove(entries[key]);
                return true;
            }
        }

        private void remove(Entry entry)
        {
            lock (this)
            {
                entry.previous.next = entry.next;
                entry.next.previous = entry.previous;
                entries.Remove(entry.key);
            }
        }

        protected void shrinkToCapacity()
        {
            lock (this)
            {
                while (EvictionNeeded) remove(anchor.previous);
            }
        }

        public void Clear()
        {
            lock (this)
            {
                if (Count > 0) flushCount++;
                anchor.next = anchor;
                anchor.previous = anchor;
                entries.Clear();
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (this)
            {
                if (ContainsKey(key))
                {
                    value = this[key];
                    return true;
                }
                else
                {
                    value = default(TValue);
                    return false;
                }
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                lock (this)
                {
                    List<TValue> values = new List<TValue>();
                    foreach (KeyValuePair<TKey, TValue> item in this)
                    {
                        values.Add(item.Value);
                    }
                    return values;
                }
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            lock (this)
            {
                return ContainsKey(item.Key) && object.Equals(this[item.Key], item.Value);
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            lock (this)
            {
                foreach (KeyValuePair<TKey, TValue> item in this)
                {
                    array[arrayIndex++] = item;
                }
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            lock (this)
            {
                if (Contains(item)) return Remove(item.Key);
                return false;
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (KeyValuePair<TKey, Entry> item in entries)
            {
                yield return new KeyValuePair<TKey, TValue>(item.Key, item.Value.value);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
