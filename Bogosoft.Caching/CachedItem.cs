using System;

namespace Bogosoft.Caching
{
    struct CachedItem<T>
    {
        internal DateTimeOffset Expiry;

        internal T Item;
    }
}