namespace Federation.App.Models;

public class PaginatedResults<T>
{
    public int Total { get; set; }

    public IEnumerable<T>? Results { get; set; }

    public PaginatedResults<T> WithTotal(int total)
    {
        Total = total;

        return this;
    }

    public static PaginatedResults<T> FromItems(IEnumerable<T> results)
    {
        return new PaginatedResults<T>
        {
            Results = results
        };
    }
}