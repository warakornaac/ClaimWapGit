using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Caching;
using ClaimWap.Models;

namespace ClaimWap.Helpers
{
    public static class DefineCodeCache
    {
        private static readonly ObjectCache Cache = MemoryCache.Default;
        private static readonly object LockObj = new object();
        private const string KeyPrefix = "DefineCode_";

        // ปรับระยะเวลาได้ตามความถี่ที่ข้อมูลเปลี่ยน
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(480);

        /// <summary>
        /// ดึงข้อมูลจาก Cache ถ้าไม่มีจะเรียก loader (DB) แล้วเก็บลง Cache
        /// </summary>
        public static List<DefineCode> GetOrLoad(int defineId, Func<int, List<DefineCode>> loader) {
            string key = KeyPrefix + defineId;

            var cached = Cache.Get(key) as List<DefineCode>;
            if (cached != null)
                return cached;

            lock (LockObj) {
                // ตรวจซ้ำ เผื่อ thread อื่นโหลดเสร็จไปแล้วระหว่างรอ lock
                cached = Cache.Get(key) as List<DefineCode>;
                if (cached != null)
                    return cached;

                List<DefineCode> data = loader(defineId);

                var policy = new CacheItemPolicy {
                    AbsoluteExpiration = DateTimeOffset.Now.Add(CacheDuration)
                };
                Cache.Set(key, data, policy);

                return data;
            }
        }

        /// <summary>ล้าง Cache เฉพาะ DefineID</summary>
        public static void Remove(int defineId) {
            Cache.Remove(KeyPrefix + defineId);
        }

        /// <summary>ล้าง Cache DefineCode ทั้งหมด</summary>
        public static void ClearAll() {
            // เก็บ key ลง List ก่อน เพื่อไม่ลบระหว่าง enumerate
            List<string> keys = Cache
                .Where(x => x.Key.StartsWith(KeyPrefix))
                .Select(x => x.Key)
                .ToList();

            foreach (string key in keys) {
                Cache.Remove(key);
            }
        }
    }
}