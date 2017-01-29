namespace Bogosoft.Caching
{
    /// <summary>
    /// Represents the handler that will handle the <see cref="ICache{TItem, TKey}.ItemCached"/> event.
    /// </summary>
    /// <typeparam name="TItem">The type of the item that can be cached.</typeparam>
    /// <typeparam name="TKey">The type of the key used to retrieve a cached object of the item type.</typeparam>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">
    /// Event data associated with a successful cache operation.
    /// </param>
    public delegate void ItemCachedEventHandler<TItem, TKey>(object sender, ItemCachedEventArgs<TItem, TKey> args);
}