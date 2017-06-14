using Bogosoft.Maybe;
using System.Threading;
using System.Threading.Tasks;

namespace Bogosoft.Caching
{
    /// <summary>
    /// Indicates that an implementation is capable of asynchronously caching items of a specified type.
    /// </summary>
    /// <typeparam name="TItem">The type of the item that can be cached.</typeparam>
    /// <typeparam name="TKey">The type of the key used to retrieve a cached object of the item type.</typeparam>
    public interface ICacheAsync<TItem, TKey>
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
        /// <param name="token">a <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// A value indicating whether or not the caching operation succeeded.
        /// </returns>
        Task<bool> CacheAsync(TKey key, TItem item, CancellationToken token);

        /// <summary>
        /// Clear the current cache of all cached items.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// A value corresponding to the number of cached items cleared.
        /// </returns>
        Task<int> ClearAsync(CancellationToken token);

        /// <summary>
        /// Determine if the current cache currently contains a given key.
        /// </summary>
        /// <param name="key">
        /// An object of the key type.
        /// </param>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// A value indicating whether or not the current cache contains the given key.
        /// </returns>
        Task<bool> ContainsKeyAsync(TKey key, CancellationToken token);

        /// <summary>
        /// Retrieve a previously cached item from the current cache by a given key.
        /// </summary>
        /// <param name="key">An object of the key type.</param>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// A value that might contain an object of the cached item type.
        /// </returns>
        Task<IMayBe<TItem>> GetAsync(TKey key, CancellationToken token);

        /// <summary>
        /// Attempt to remove a previously cached item from the current cache.
        /// </summary>
        /// <param name="key">
        /// A value corresponding to the key that the item was originally cached against.
        /// </param>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// A value indicating whether or not the removal operation was successful.
        /// </returns>
        Task<bool> RemoveAsync(TKey key, CancellationToken token);
    }
}