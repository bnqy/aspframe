using Fruit;

var client = new FruitClient("https://localhost:7286/",   // base url of API
	new HttpClient());   // is used to call the API

Fruit.Fruit created = await client.Fruit2Async("123", new Fruit.Fruit { Name="apple", Stock=32});  //Post
Fruit.Fruit fetched = await client.FruitAsync("123");

Console.WriteLine($"Fetched fruit: {fetched.Name}: {fetched.Stock}");

Console.WriteLine("Hello, World!");
