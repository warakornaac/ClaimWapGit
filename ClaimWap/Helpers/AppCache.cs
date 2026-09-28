using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;

namespace ClaimWap.Helpers
{
    internal class AppCacheEntry
    {
        public object Data { get; set; }
        public int ItemCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

    public class AppCacheDebugEntry
    {
        public string Key { get; set; }
        public string DataType { get; set; }
        public int ItemCount { get; set; }
        public string CreatedAt { get; set; }
        public string ExpiresAt { get; set; }
        public double RemainingMinutes { get; set; }
        public object Items { get; set; }
    }

    public class AppCacheDebugSummary
    {
        public string ServerTime { get; set; }
        public string MachineName { get; set; }
        public int Entries { get; set; }
        public long HitCount { get; set; }
        public long MissCount { get; set; }
        public string HitRate { get; set; }
        public List<AppCacheDebugEntry> Items { get; set; }
    }

    /// <summary>
    /// Cache แบบ Generic ใช้ได้กับข้อมูลทุกชนิด (Dropdown, Master data)
    /// </summary>
    public static class AppCache
    {
        private static readonly ObjectCache Cache = MemoryCache.Default;
        private const string KeyPrefix = "App_";
        private const string DateFormat = "yyyy-MM-dd HH:mm:ss";

        // lock แยกตาม key: โหลด key หนึ่งไม่บล็อก key อื่น
        private static readonly ConcurrentDictionary<string, object> Locks =
            new ConcurrentDictionary<string, object>();

        private static long _hitCount;
        private static long _missCount;

        /// <summary>
        /// ดึงข้อมูลจาก Cache ถ้าไม่มีจะเรียก loader แล้วเก็บลง Cache
        /// </summary>
        public static T GetOrLoad<T>(string key, Func<T> loader, TimeSpan duration) where T : class {
            string fullKey = KeyPrefix + key;

            T cached = TryGet<T>(fullKey);
            if (cached != null) {
                Interlocked.Increment(ref _hitCount);
                Debug.WriteLine("[AppCache] HIT  " + fullKey);
                return cached;
            }

            object keyLock = Locks.GetOrAdd(fullKey, k => new object());
            lock (keyLock) {
                cached = TryGet<T>(fullKey);
                if (cached != null) {
                    Interlocked.Increment(ref _hitCount);
                    return cached;
                }

                Interlocked.Increment(ref _missCount);
                var sw = Stopwatch.StartNew();

                T data = loader();

                sw.Stop();

                if (data == null) {
                    // ไม่เก็บ null ลง Cache
                    return null;
                }

                var collection = data as ICollection;
                DateTime now = DateTime.Now;

                var entry = new AppCacheEntry {
                    Data = data,
                    ItemCount = collection != null ? collection.Count : 1,
                    CreatedAt = now,
                    ExpiresAt = now.Add(duration)
                };

                Cache.Set(fullKey, entry, new CacheItemPolicy {
                    AbsoluteExpiration = new DateTimeOffset(entry.ExpiresAt)
                });

                Debug.WriteLine(string.Format("[AppCache] MISS {0} -> load {1} ms ({2} items)",
                    fullKey, sw.ElapsedMilliseconds, entry.ItemCount));

                return data;
            }
        }

        public static void Remove(string key) {
            Cache.Remove(KeyPrefix + key);
            Debug.WriteLine("[AppCache] REMOVE " + KeyPrefix + key);
        }

        /// <summary>ลบทุก key ที่ขึ้นต้นด้วย prefix เช่น RemoveByPrefix("Product_")</summary>
        public static void RemoveByPrefix(string keyPrefix) {
            string fullPrefix = KeyPrefix + keyPrefix;
            List<string> keys = Cache
                .Where(x => x.Key.StartsWith(fullPrefix))
                .Select(x => x.Key)
                .ToList();

            foreach (string k in keys) {
                Cache.Remove(k);
            }
            Debug.WriteLine(string.Format("[AppCache] REMOVE PREFIX {0} ({1} keys)", fullPrefix, keys.Count));
        }

        public static void ClearAll() {
            RemoveByPrefix(string.Empty);
        }

        public static void ResetStats() {
            Interlocked.Exchange(ref _hitCount, 0);
            Interlocked.Exchange(ref _missCount, 0);
        }

        public static AppCacheDebugSummary GetDebugInfo(bool includeItems) {
            DateTime now = DateTime.Now;
            long hit = Interlocked.Read(ref _hitCount);
            long miss = Interlocked.Read(ref _missCount);
            long total = hit + miss;

            var list = new List<AppCacheDebugEntry>();
            var items = Cache.Where(x => x.Key.StartsWith(KeyPrefix)).ToList();

            foreach (var item in items) {
                var entry = item.Value as AppCacheEntry;
                if (entry == null)
                    continue;

                list.Add(new AppCacheDebugEntry {
                    Key = item.Key,
                    DataType = entry.Data.GetType().Name,
                    ItemCount = entry.ItemCount,
                    CreatedAt = entry.CreatedAt.ToString(DateFormat),
                    ExpiresAt = entry.ExpiresAt.ToString(DateFormat),
                    RemainingMinutes = Math.Round((entry.ExpiresAt - now).TotalMinutes, 1),
                    Items = includeItems ? entry.Data : null
                });
            }

            return new AppCacheDebugSummary {
                ServerTime = now.ToString(DateFormat),
                MachineName = Environment.MachineName,
                Entries = list.Count,
                HitCount = hit,
                MissCount = miss,
                HitRate = total == 0 ? "0%" : Math.Round(hit * 100.0 / total, 2) + "%",
                Items = list.OrderBy(e => e.Key).ToList()
            };
        }

        private static T TryGet<T>(string fullKey) where T : class {
            var entry = Cache.Get(fullKey) as AppCacheEntry;
            return entry != null ? entry.Data as T : null;
        }
    }
}