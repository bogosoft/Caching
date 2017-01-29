using System;

namespace Bogosoft.Caching
{
    /// <summary>
    /// Event data related to an item being successfully cached.
    /// </summary>
    /// <typeparam name="TItem">
    /// The type of the object cached.
    /// </typeparam>
    /// <typeparam name="TKey">
    /// The type of the object used as the key that the cached item was cached against.
    /// </typeparam>
    public class ItemCachedEventArgs<TItem, TKey> : EventArgs
    {
        /// <summary>
        /// Get the item that was successfully cached.
        /// </summary>
        public TItem Item { get; protected set; }

        /// <summary>
        /// Get the key that was used to cache the associated item against.
        /// </summary>
        public TKey Key { get; protected set; }

        /// <summary>
        /// Create a new instance of the <see cref="ItemCachedEventArgs{TItem, TKey}"/> class.
        /// </summary>
        /// <param name="key">
        /// A value corresponding to the key associated with a given item.
        /// </param>
        /// <param name="item">
        /// A value corresponding to an item that was successfully cached.
        /// </param>
        public ItemCachedEventArgs(TKey key, TItem item)
        {
            Item = item;
            Key = key;
        }
    }
}