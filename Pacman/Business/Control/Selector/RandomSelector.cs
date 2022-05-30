namespace Pacman.Business.Control.Selector;

public class RandomSelector<T> : ISelector<T>
{
    public T Select(IEnumerable<T> collection)
    {
        var collectionArr = collection.ToArray();

        if (!collectionArr.Any())
            throw new InvalidOperationException("Cannot invoke select on empty enumerable.");
        
        var random = new Random();

        return collectionArr.ElementAt(random.Next(0, collectionArr.Length + 1));
    }
}
