using System;

namespace Bogosoft.Caching
{
    /// <summary>Provides a means of caching items.</summary>
    /// <typeparam name="TItem">The type of the item to be cached.</typeparam>
    /// <typeparam name="TKey">The type of a key corresponding to an item to be cached.</typeparam>
    public interface ICache<TItem, TKey>
    {
        /// <summary>Get the number of items in the current cache.</summary>
        Int32 Count { get; }

        /// <summary>Clear the cache of all cached items.</summary>
        /// <returns>Whether or not the clear operation was successful.</returns>
        Boolean Clear();

        /// <summary>Delete a number of items from the current cache by their keys.</summary>
        /// <param name="keys">Keys corresponding to items cached by the current cache.</param>
        /// <returns>The number of items that were successfully deleted from the current cache.</returns>
        Int32 Delete(params TKey[] keys);

        /// <summary>Retrieve a cached item from the current cache.</summary>
        /// <param name="key">The key corresponding to a cached item.</param>
        /// <returns>
        /// A previously cached item. Implementations should return the default value of the
        /// item type in the event that a get operation results in a cache miss.
        /// </returns>
        TItem Get(TKey key);

        /// <summary>Store an item in the current cache.</summary>
        /// <param name="item">An item to store in the current cache.</param>
        /// <returns>Whether or not the item was successfully saved to the current cache.</returns>
        Boolean Save(TItem item);
    }
}