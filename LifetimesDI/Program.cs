var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/b", RowCounts);  // throws an error

app.Run();

string RowCounts(DataContext dc, Repository r)
{
    int dci = dc.RowCount;
    int ri = r.RowCount;

    return $"DataContext: {dci}, Repository: {ri}";
}

public class DataContext
{
	public int RowCount { get; } =
		Random.Shared.Next(1, 1_000_000_000);
}

public class Repository
{
	private readonly DataContext _dataContext;

    public Repository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public int RowCount => _dataContext.RowCount;
}