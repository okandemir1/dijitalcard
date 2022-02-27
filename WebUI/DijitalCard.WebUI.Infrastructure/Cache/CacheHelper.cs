using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DijitalCard.Data;

namespace DijitalCard.WebUI.Infrastructure.Cache
{
    public class CacheHelper
    {
        ICache cache;
        PlatformData platformData;

        public CacheHelper(ICache cache, PlatformData platformData)
        {
            this.cache = cache;
            this.platformData = platformData;
        }

        private string Platforms_CacheKey = "Platforms_CacheKey";
        public bool PlatformsClear() { return Clear(Platforms_CacheKey); }
        public List<Model.Platform> Platforms
        {
            get
            {
                var fromCache = Get<List<Model.Platform>>(Platforms_CacheKey);
                if(fromCache == null)
                {
                    var datas = platformData.GetBy(x => !x.IsDeleted && x.IsActive);
                    if(datas != null && datas.Count() > 0)
                    {
                        Set(Platforms_CacheKey, datas);
                        fromCache = datas;
                    }
                }

                return fromCache;
            }
        }

        public bool Clear(string name)
        {
            cache.Remove(name);
            return true;
        }

        public T Get<T>(string cacheKey) where T : class
        {
            object cookies;

            if (!cache.TryGetValue(cacheKey, out cookies))
                return null;

            return cookies as T;
        }

        public void Set(string cacheKey, object value)
        {
            cache.Set(cacheKey, value, 180);
        }
    }
}
