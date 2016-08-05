using Bogosoft.Data;
using System;
using System.Collections.Generic;

namespace Bogosoft.Caching
{
    /// <summary>A thread-safe, in-memory caching strategy.</summary>
    /// <typeparam name="TItem">The type of the item to be cached.</typeparam>
    /// <typeparam name="TKey">The type of a key corresponding to an item to be cached.</typeparam>
    public sealed class MemoryCache<TItem, TKey> : ICache<TItem, TKey>
    {
        private IConstrain<CachedItem<TItem>> constraint;
        private Func<TItem, TKey> keyExtractor;
        private readonly IDictionary<TKey, CachedItem<TItem>> pool = new Dictionary<TKey, CachedItem<TItem>>();

        /// <summary>Get the number of items in the current cache.</summary>
        public Int32 Count
        {
            get { return this.pool.Count; }
        }

        /// <summary>
        /// Create a new instance of an in-memroy cache by providing a key selection strategy
        /// and a constraint by which cached items will be checked for staleness.
        /// </summary>
        /// <param name="keyExtractor">
        /// A key extraction strategy in the form of an expression.
        /// This strategy will be used during save operations to extract the value the cached item will
        /// be keyed against.   
        /// </param>
        /// <param name="constraint">A constraint by which the staleness of cached items is determined.</param>
        public MemoryCache(
            Func<TItem, TKey> keyExtractor,
            IConstrain<CachedItem<TItem>> constraint = null
            )
        {
            this.constraint = constraint;
            this.keyExtractor = keyExtractor;
        }

        /// <summary>Clear the cache of all cached items.</summary>
        /// <returns>Whether or not the clear operation was successful.</returns>
        public Boolean Clear()
        {
            lock (this.pool)
            {
                this.pool.Clear();

                return true;
            }
        }

        /// <summary>Delete a number of items from the current cache by their keys.</summary>
        /// <param name="keys">Keys corresponding to items cached by the current cache.</param>
        /// <returns>The number of items that were successfully deleted from the current cache.</returns>
        public Int32 Delete(params TKey[] keys)
        {
            lock (this.pool)
            {
                var deleted = 0;

                foreach (var k in keys)
                {
                    if (this.pool.Remove(k))
                    {
                        ++deleted;
                    }
                }

                return deleted;
            }
        }

        /// <summary>Retrieve a cached item from the current cache.</summary>
        /// <param name="key">The key corresponding to a cached item.</param>
        /// <returns>The cached item if found; the type's default value otherwise.</returns>
        public TItem Get(TKey key)
        {
            lock (this.pool)
            {
                if (this.pool.ContainsKey(key))
                {
                    var entry = this.pool[key];

                    if(this.constraint == null)
                    {
                        return entry.Item;
                    }

                    if (this.constraint.Validate(entry))
                    {
                        return entry.Item;
                    }
                    else
                    {
                        this.pool.Remove(key);
                    }
                }

                return default(TItem);
            }
        }

        /// <summary>Store an item in the current cache.</summary>
        /// <param name="item">An item to store in the current cache.</param>
        /// <returns>Whether or not the item was successfully saved to the current cache.</returns>
        public Boolean Save(TItem item)
        {
            lock (this.pool)
            {
                var key = this.keyExtractor.Invoke(item);

                this.pool[key] = new CachedItem<TItem>(item);

                return this.pool.ContainsKey(key);
            }
        }
    }
}