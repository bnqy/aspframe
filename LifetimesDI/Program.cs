var builder = WebApplication.CreateBuilder(args);

// with them result will be: DataContext: 390260753, Repository: 563821397
// bc a new instance is created when the service required
builder.Services.AddTransient<TransientDataContext>(); // with tarnsient whenever a dependency required a new instance will be created
builder.Services.AddTransient<TransientRepository>(); // new instance of a service will be created

// result will be: DataContext: 390260753, Repository: 390260753   // same
builder.Services.AddScoped<ScopedDataContext>();  // with scoped will be created the instance within the request
builder.Services.AddScoped<ScopedRepository>(); // a scope maps to single request

builder.Services.AddSingleton<SingletonDataContext>();  // the one instance will be used through out the life of app
builder.Services.AddSingleton<SingletonRepository>();

var app = builder.Build();

app.MapGet("/", () => @"/transient
/scoped
/singleton");



List<string> _transients = new();
List<string> _scoped = new();
List<string> _singletons = new();

app.MapGet("/transient", TransientHandler);
app.MapGet("/scoped", ScopedHandler);
app.MapGet("/singleton", SingletonHandler);








string TransientHandler(TransientDataContext dc, TransientRepository repo)
{
	return RowCounts(dc, repo, _transients);
}

string ScopedHandler(ScopedDataContext dc, ScopedRepository repo)
{
	return RowCounts(dc, repo, _scoped);
}

string SingletonHandler(SingletonDataContext dc, SingletonRepository repo)
{
	return RowCounts(dc, repo, _singletons);
}

app.Run();




static string RowCounts(DataContext dc, Repository repo, List<string> pre) // with transient the DataContext object differs from the obj in Repository
{
	var counts = $"{dc.GetType().Name}: {dc.RowCount:000,000,000}, {repo.GetType().Name}: {repo.RowCount:000,000,000}";

	var result = $@"
Current values:
{counts}

Previous values:
{string.Join(Environment.NewLine, pre)}";

	pre.Insert(0, counts);
	return result;
}






public class DataContext
{
	public int RowCount { get; } =
		Random.Shared.Next(1, 1_000_000_000);
}

public class TransientDataContext
    : DataContext
{ }

public class ScopedDataContext
	: DataContext
{ }

public class SingletonDataContext 
	: DataContext
{ }



public class Repository
{
	private readonly DataContext _dataContext;  // with transient here the obj differs from RowCount

    public Repository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public int RowCount => _dataContext.RowCount;
}

public class TransientRepository
	: Repository
{
	public TransientRepository(TransientDataContext dataContext) : base(dataContext)
	{
	}
}

public class ScopedRepository
	: Repository
{
	public ScopedRepository(ScopedDataContext dataContext) : base(dataContext)
	{
	}
}

public class SingletonRepository
	: Repository
{
	public SingletonRepository(SingletonDataContext dataContext) : base(dataContext)
	{
	}
}