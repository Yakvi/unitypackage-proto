using System.Collections.Generic;
using UnityEngine;

namespace VenetStudio
{
    public class CachedData<T>
    {
        public T value;
        private int cacheSnapshot = -1;
        public bool IsFresh()
        {
            var time = Time.frameCount;
            if (cacheSnapshot == time) return true;
            cacheSnapshot = time;
            return false;
        }
    }
    
    public class CachedData<TIndex, T>
    {
        public Dictionary<TIndex, Data> values;

        public T GetValue(TIndex index)
        {
            if (values == null) return default;
            if (!values.ContainsKey(index)) return default;
            return values[index].value;
        }

        public void SetValue(TIndex index, T value)
        {
            values ??= new Dictionary<TIndex, Data>();
            values[index] = new Data
            {
                snapshot = Time.frameCount,
                value = value
            };
        }
        
        public bool IsFresh(TIndex index)
        {
            values ??= new Dictionary<TIndex, Data>();
            if (!values.ContainsKey(index)) return false;
            return values[index].snapshot == Time.frameCount;
        }

        public struct Data
        {
            public int snapshot;
            public T value;
        }
    }
}