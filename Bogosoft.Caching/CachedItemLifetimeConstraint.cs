using Bogosoft.Data;
using System;

namespace Bogosoft.Caching
{
    /// <summary>
    /// A validation strategy for determining when a <see cref="CachedItem{T}"/> is stale
    /// by comparing the item's cache time to a provided <see cref="TimeSpan"/>. 
    /// </summary>
    /// <typeparam name="T">The type of the cached item.</typeparam>
    public sealed class CachedItemLifetimeConstraint<T> : IConstrain<CachedItem<T>>
    {
        private TimeSpan lifetime;

        /// <summary>
        /// Create a new instance of the <see cref="CachedItemLifetimeConstraint{T}"/> by providing
        /// a lifetime in the form of a <see cref="TimeSpan"/>. 
        /// </summary>
        /// <param name="lifetime">A timespan after which a cached item is considered stale.</param>
        public CachedItemLifetimeConstraint(TimeSpan lifetime)
        {
            this.lifetime = lifetime;
        }

        /// <summary>Validate that a given <see cref="CachedItem{T}"/> is not stale.</summary>
        /// <param name="cachedItem">The <see cref="CachedItem{T}"/> to check for staleness.</param>
        /// <returns>Whether or not the <see cref="CachedItem{T}"/> is fresh.</returns>
        public Boolean Validate(CachedItem<T> cachedItem)
        {
            return cachedItem.CachedOn + this.lifetime <= DateTime.UtcNow;
        }
    }
}