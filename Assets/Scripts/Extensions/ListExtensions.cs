using System;
using System.Collections.Generic;

namespace Extensions
{
    public static class DictionaryExtensions
    {
        public static void ForEachValue<TK, TV>(this Dictionary<TK, TV> d, Action<TV> action)
        {
            foreach (var keyValuePair in d) action(keyValuePair.Value);
        }
        
        public static void ForEachKey<TK, TV>(this Dictionary<TK, TV> d, Action<TK> action)
        {
            foreach (var keyValuePair in d) action(keyValuePair.Key);
        }
        
        public static void ForEach<TK, TV>(this Dictionary<TK, TV> d, Action<TK, TV> action)
        {
            foreach (var keyValuePair in d) action(keyValuePair.Key, keyValuePair.Value);
        }
        
        public static Dictionary<TK2, TV2> Map<TK, TV, TK2, TV2>(
            this Dictionary<TK, TV> d,
            Func<TK, TV, TK2> newKeyFunc,
            Func<TK, TV, TV2> newValueFunc)
        {
            var newDict = new Dictionary<TK2, TV2>();
            
            d.ForEach((k, v) => newDict.Add(newKeyFunc.Invoke(k, v), newValueFunc.Invoke(k, v)));

            return newDict;
        }
        
        public static List<TR> Map<TK, TV, TR>(this Dictionary<TK, TV> d, Func<TK, TV, TR> f)
        {
            var result = new List<TR>();
            
            d.ForEach((k, v) => result.Add(f.Invoke(k, v)));

            return result;
        }
    }
}