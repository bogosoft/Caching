using System;

namespace Bogosoft.Caching
{
    /// <summary>Represents a cached item and associated metadata.</summary>
    /// <typeparam name="T">The type of the item cached.</typeparam>
    public class CachedItem<T>
    {
        /// <summary>Get the time that the current item was cached.</summary>
        public readonly DateTime CachedOn;

        /// <summary>Get the currently cached item.</summary>
        public readonly T Item;

        /// <summary>Create a new instance of a cached item.</summary>
        /// <param name="item">The item to be cached.</param>
        public CachedItem(T item)
        {
            this.CachedOn = DateTime.UtcNow;
            this.Item = item;
        }
    }
}