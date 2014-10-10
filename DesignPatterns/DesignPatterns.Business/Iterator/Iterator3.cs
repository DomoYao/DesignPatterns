using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
//实现方式（三）：实现 BidirectionalConcurrentDictionary 双向并发字典。

namespace DesignPatterns.Business.Iterator
{

    /// <summary>
    /// 双值对
    /// </summary>
    /// <typeparam name="TFirst">第一个值的类型</typeparam>
    /// <typeparam name="TSecond">第二个值的类型</typeparam>
    [Serializable]
    public struct FirstSecondPair<TFirst, TSecond>
    {
        private readonly TFirst _first;
        private readonly TSecond _second;

        /// <summary>
        /// 第一个值
        /// </summary>
        public TFirst First
        {
            get { return _first; }
        }

        /// <summary>
        /// 第二个值
        /// </summary>
        public TSecond Second
        {
            get { return _second; }
        }

        /// <summary>
        /// 双值对
        /// </summary>
        /// <param name="first">第一个值</param>
        /// <param name="second">第二个值</param>
        public FirstSecondPair(TFirst first, TSecond second)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");

            _first = first;
            _second = second;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var target = (FirstSecondPair<TFirst, TSecond>) obj;
            return First.Equals(target.First) && Second.Equals(target.Second);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('[');

            if (First != null)
            {
                sb.Append(First);
            }

            sb.Append(", ");

            if (Second != null)
            {
                sb.Append(Second);
            }

            sb.Append(']');

            return sb.ToString();
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(FirstSecondPair<TFirst, TSecond> left, FirstSecondPair<TFirst, TSecond> right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(FirstSecondPair<TFirst, TSecond> left, FirstSecondPair<TFirst, TSecond> right)
        {
            return !(left == right);
        }
    }

    public class BidirectionalConcurrentDictionary<TFirst, TSecond> : IEnumerable<FirstSecondPair<TFirst, TSecond>>
    {
        #region Fields

        private readonly ConcurrentDictionary<TFirst, TSecond> _firstToSecond = new ConcurrentDictionary<TFirst, TSecond>();
        private readonly ConcurrentDictionary<TSecond, TFirst> _secondToFirst = new ConcurrentDictionary<TSecond, TFirst>();

        #endregion

        #region Public Methods

        public void Add(TFirst first, TSecond second)
        {
            if (_firstToSecond.ContainsKey(first) || _secondToFirst.ContainsKey(second))
                throw new ArgumentException("Duplicate first or second");

            _firstToSecond.Add(first, second);
            _secondToFirst.Add(second, first);
        }

        public bool ContainsFirst(TFirst first)
        {
            return _firstToSecond.ContainsKey(first);
        }

        public bool ContainsSecond(TSecond second)
        {
            return _secondToFirst.ContainsKey(second);
        }

        public TSecond GetByFirst(TFirst first)
        {
            TSecond second;
            if (!_firstToSecond.TryGetValue(first, out second))
                throw new KeyNotFoundException("Cannot find second by first.");

            return second;
        }

        public TFirst GetBySecond(TSecond second)
        {
            TFirst first;
            if (!_secondToFirst.TryGetValue(second, out first))
                throw new KeyNotFoundException("Cannot find first by second.");

            return first;
        }

        public void RemoveByFirst(TFirst first)
        {
            TSecond second;
            if (!_firstToSecond.TryGetValue(first, out second))
                throw new KeyNotFoundException("Cannot find second by first.");

            _firstToSecond.Remove(first);
            _secondToFirst.Remove(second);
        }

        public void RemoveBySecond(TSecond second)
        {
            TFirst first;
            if (!_secondToFirst.TryGetValue(second, out first))
                throw new KeyNotFoundException("Cannot find first by second.");

            _secondToFirst.Remove(second);
            _firstToSecond.Remove(first);
        }

        public bool TryAdd(TFirst first, TSecond second)
        {
            if (_firstToSecond.ContainsKey(first) || _secondToFirst.ContainsKey(second))
                return false;

            _firstToSecond.Add(first, second);
            _secondToFirst.Add(second, first);
            return true;
        }

        public bool TryGetByFirst(TFirst first, out TSecond second)
        {
            return _firstToSecond.TryGetValue(first, out second);
        }

        public bool TryGetBySecond(TSecond second, out TFirst first)
        {
            return _secondToFirst.TryGetValue(second, out first);
        }

        public bool TryRemoveByFirst(TFirst first)
        {
            TSecond second;
            if (!_firstToSecond.TryGetValue(first, out second))
                return false;

            _firstToSecond.Remove(first);
            _secondToFirst.Remove(second);
            return true;
        }

        public bool TryRemoveBySecond(TSecond second)
        {
            TFirst first;
            if (!_secondToFirst.TryGetValue(second, out first))
                return false;

            _secondToFirst.Remove(second);
            _firstToSecond.Remove(first);
            return true;
        }

        public int Count
        {
            get { return _firstToSecond.Count; }
        }

        public void Clear()
        {
            _firstToSecond.Clear();
            _secondToFirst.Clear();
        }

        #endregion

        #region IEnumerable<FirstSecondPair<TFirst,TSecond>> Members

        IEnumerator<FirstSecondPair<TFirst, TSecond>> IEnumerable<FirstSecondPair<TFirst, TSecond>>.GetEnumerator()
        {
            foreach (var item in _firstToSecond)
            {
                yield return new FirstSecondPair<TFirst, TSecond>(item.Key, item.Value);
            }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var item in _firstToSecond)
            {
                yield return new FirstSecondPair<TFirst, TSecond>(item.Key, item.Value);
            }
        }

        #endregion
    }

    public static class ConcurrentDictionaryExtensions
    {
        public static TValue Add<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> collection, TKey key,
                                               TValue @value)
        {
            TValue result = collection.AddOrUpdate(key, @value, (k, v) => { return @value; });
            return result;
        }

        public static TValue Update<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> collection, TKey key,
                                                  TValue @value)
        {
            TValue result = collection.AddOrUpdate(key, @value, (k, v) => { return @value; });
            return result;
        }

        public static TValue Get<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> collection, TKey key)
        {
            TValue @value = default(TValue);
            collection.TryGetValue(key, out @value);
            return @value;
        }

        public static TValue Remove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> collection, TKey key)
        {
            TValue @value = default(TValue);
            collection.TryRemove(key, out @value);
            return @value;
        }
    }

}
