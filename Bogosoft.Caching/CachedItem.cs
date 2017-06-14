using System;

namespace Bogosoft.Caching
{
    struct CachedItem<TItem>
    {
        internal DateTimeOffset Expiry;

        internal TItem Item;
    }

}