using Microsoft.AspNetCore.Mvc;

namespace WEBApi.Controllers
{
	[ApiController]  // api specifics
	public class FruitController : ControllerBase
	{
		private readonly FruitService _fruitService;
		public FruitController(FruitService fruitService)
		{
			_fruitService = fruitService;
		}

		[HttpGet("fruit")]  // defines the route template so -> /fruit
		//[Route("")]  // can have multiple [RouteAttributes]
		//[Route("/hi")]
		public IEnumerable<string> Index()  // the name of action is not used for routing
		{
			return _fruitService.Fruit;
		}

		[HttpGet("fruit/{id}")]  // so -> /fruit/{id}
		//[Route("view/{id}")]
		//[Route("view-hi/{id}")]
		public ActionResult<string> View(int id)  // it can return string or an IActionResult
		{
			if (id >= 0 && id < _fruitService.Fruit.Count)
			{
				return _fruitService.Fruit[id];
			}

			return NotFound();
		}
	}
}
