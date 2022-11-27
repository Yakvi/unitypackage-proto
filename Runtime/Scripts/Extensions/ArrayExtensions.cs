using System.Collections.Generic;

namespace VenetStudio
{
    public static partial class Utility
    {
        public static int Wrap<T>(this T[] collection, int index)
        {
            return index.WrapAround(0, 0, collection.Length);
        }

        public static int Wrap<T>(this List<T> collection, int index)
        {
            return index.WrapAround(0, 0, collection.Count);
        }
        
        public static T Safe<T>(this T[] collection, int index)
        {
            return collection[index.WrapAround(0, 0, collection.Length)];
        }
        
        public static T Safe<T>(this List<T> collection, int index)
        {
            return collection[index.WrapAround(0, 0, collection.Count)];
        }

        public static bool IsInRange<T>(this List<T> collection, int index) => index >= 0 && index < collection.Count;
        public static bool IsInRange<T>(this T[] collection, int index) => index >= 0 && index < collection.Length;
    }
}