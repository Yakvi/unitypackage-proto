using UnityEngine;

namespace VenetStudio
{
    public class CachedData<T>
    {
        public T value;
        private int cacheSnapshot = -1;
        public bool IsRelevant()
        {
            var time = Time.frameCount;
            if (cacheSnapshot == time) return true;
            cacheSnapshot = time;
            return false;
        }
    }
}