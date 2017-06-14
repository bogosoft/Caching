using Bogosoft.Maybe;

namespace Bogosoft.Caching
{
    /// <summary>
    /// Indicates that an implementation is capable of caching items of a specified type.
    /// </summary>
    /// <typeparam name="TItem">The type of the item that can be cached.</typeparam>
    /// <typeparam name="TKey">The type of the key used to retrieve a cached object of the item type.</typeparam>
    public interface ICache<TItem, TKey>
    {
        /// <summary>
        /// Occurs when an item is successfully cached.
        /// </summary>
        event ItemCachedEventHandler<TItem, TKey> ItemCached;

        /// <summary>
        /// Cache an object of the item type.
        /// </summary>
        /// <param name="key">
        /// An object of the key type which will serve as the key by which an item is cached.
        /// </param>
        /// <param name="item">An object of the cached item type.</param>
        /// <returns>
        /// A value indicating whether or not the caching operation succeeded.
        /// </returns>
        bool Cache(TKey key, TItem item);

        /// <summary>
        /// Clear the current cache of all cached items.
        /// </summary>
        /// <returns>
        /// A value corresponding to the number of cached items cleared.
        /// </returns>
        int Clear();

        /// <summary>
        /// Determine if the current cache currently contains a given key.
        /// </summary>
        /// <param name="key">
        /// An object of the key type.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the current cache contains the given key.
        /// </returns>
        bool Contains(TKey key);

        /// <summary>
        /// Retrieve a previously cached item from the current cache by a given key.
        /// </summary>
        /// <param name="key">An object of the key type.</param>
        /// <returns>
        /// A value that might contain an object of the cached item type.
        /// </returns>
        IMayBe<TItem> Get(TKey key);

        /// <summary>
        /// Attempt to remove a previously cached item from the current cache.
        /// </summary>
        /// <param name="key">
        /// A value corresponding to the key that the item was originally cached against.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the removal operation was successful.
        /// </returns>
        bool Remove(TKey key);
    }
}