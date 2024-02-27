var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();


// can bind complex object types, and read-only properties with a default.
// The binder will also bind collections, including interfaces.
public class BindableOptions
{
	//Can Bind
	public string String { get; set; }
	public int Integer { get; set; }
	public SubClass Object { get; set; }
	public SubClass ReadOnly { get; } = new SubClass();
	public Dictionary<string, SubClass> Dictionary { get; set; }
	public List<SubClass> List { get; set; }
	public IDictionary<string, SubClass> IDictionary { get; set; }
	public IEnumerable<SubClass> IEnumerable { get; set; }
	public ICollection<SubClass> ReadOnlyCollection { get; } = new List<SubClass>();
	public class SubClass
	{
		public string Value { get; set; }
	}
}


public class UnbindableOptions
{
	//Can't bind
	internal string NotPublic { get; set; }
	public SubClass _setOnly = null;
	public SubClass SetOnly { set => _setOnly = value; }
	public SubClass NullReadOnly { get; } = null;
	public SubClass NullPrivateSetter { get; private set; } = null;
	public Dictionary<int, SubClass> DictionaryWithNonStringKeys { get; set; }
	public IEnumerable<SubClass> NullIEnumerable { get; }
	public IEnumerable<SubClass> ReadOnlyEnumerable { get; } = new List<SubClass>();
	public Dictionary<int, SubClass> IntegerKeys { get; set; }
	private readonly List<SubClass> _indexerList = new List<SubClass>();
	public SubClass this[int i] { get => _indexerList[i]; set => _indexerList[i] = value; }

	public class SubClass
	{
		public string Value { get; set; }
	}
}