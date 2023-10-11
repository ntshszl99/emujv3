using System;
using System.Data;
using System.Runtime.Caching;
namespace ConnectionModule
{
    public class CacheFunc
    {
        private MemoryCache Cache = MemoryCache.Default;
        public void SetCache(string Key, DataTable Value, int exp)
        {
            Cache.Set(Key, Value, DateTimeOffset.UtcNow.AddMinutes(exp));
        }

        public void SetCacheString(string Key, string Value, int exp)
        {
            Cache.Set(Key, Value, DateTimeOffset.UtcNow.AddMinutes(exp));
        }

        public bool GetCache(string Key, ref DataTable Data)
        {
            object tmp = Cache.Get(Key);
            Data = tmp as DataTable;
            if (Data != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GetCacheString(string Key, ref string Data)
        {
            object tmp = Cache.Get(Key);
            Data = tmp as string;
            if (Data != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RemCache(string Key)
        {
            Cache.Remove(Key);
        }
    }
}
