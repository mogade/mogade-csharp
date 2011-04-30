using System;
using System.Collections;
using System.Collections.Generic;

#if WINDOWS_PHONE
//hahahaha gheto!
namespace Mogade
{
   public class SortedDictionary<T, V> : IDictionary<T, V>
   {
      private readonly List<T> _keys;
      private readonly Dictionary<T, V> _dictionary;

      public SortedDictionary()
      {
         _keys = new List<T>();
         _dictionary = new Dictionary<T, V>();
      }

      public IEnumerator<KeyValuePair<T, V>> GetEnumerator()
      {
         _keys.Sort();
         foreach (var key in _keys)
         {
            yield return new KeyValuePair<T, V>(key, _dictionary[key]);
         }
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      public void Add(KeyValuePair<T, V> item)
      {
         _keys.Add(item.Key);
         _dictionary.Add(item.Key, item.Value);
      }

      public void Clear()
      {
         _keys.Clear();
         _dictionary.Clear();
      }

      public bool Contains(KeyValuePair<T, V> item)
      {
         return _dictionary.ContainsKey(item.Key) && _dictionary.ContainsValue(item.Value);
      }

      public void CopyTo(KeyValuePair<T, V>[] array, int arrayIndex)
      {
         throw new NotImplementedException();
      }

      public bool Remove(KeyValuePair<T, V> item)
      {
         throw new NotImplementedException();
      }

      public int Count
      {
         get { return _keys.Count; }
      }

      public bool IsReadOnly
      {
         get { return false; }
      }

      public bool ContainsKey(T key)
      {
         return _dictionary.ContainsKey(key);
      }

      public void Add(T key, V value)
      {
         _keys.Add(key);
         _dictionary.Add(key, value);
      }

      public bool Remove(T key)
      {
         _keys.Remove(key);
         return _dictionary.Remove(key);
      }

      public bool TryGetValue(T key, out V value)
      {
         return _dictionary.TryGetValue(key, out value);
      }

      public V this[T key]
      {
         get { return _dictionary[key]; }
         set { _dictionary[key] = value; }
      }

      public ICollection<T> Keys
      {
         get { return _keys; }
      }

      public ICollection<V> Values
      {
         get { return _dictionary.Values; }
      }
   }
}
#endif