namespace Shared.Helpers;

public static class ListHelper
{
    public static List<T> Merge<T>(params IEnumerable<T>[] lists)
    {
        return lists.SelectMany(x => x).ToList();
    }
}