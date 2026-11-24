using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Caching;

namespace WebAppBGV.Helpers
{
   

    


    

public static class RateLimit
        {
        private static readonly MemoryCache Cache = MemoryCache.Default;

        public static bool IsLimitExceeded(string key, int limit, int minutes)
        {
            int count = (int?)Cache.Get(key) ?? 0;

            if (count >= limit)
                return true;

            Cache.Set(
                key,
                count + 1,
                DateTimeOffset.Now.AddMinutes(minutes));

            return false;
        }
    

}
}