namespace Pacman.Business.Control.Selector;

public class RandomSelector<T> : ISelector<T>
{
    public T SelectFrom(IEnumerable<T> collection)
    {
        var collectionArr = collection.ToArray();

        if (!collectionArr.Any()) 
            throw new InvalidOperationException("Cannot invoke select on empty enumerable.");

        return collectionArr[new Random().Next(collectionArr.Length)];
    }
}
