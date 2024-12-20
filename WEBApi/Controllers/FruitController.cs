﻿using Microsoft.AspNetCore.Mvc;

namespace WEBApi.Controllers
{
	//with this attribute the [frombody is worked self]
	// validation autoly checked. if invalid returns 400
	// error status codes autoly converted to ProblemDetails NotFound() -> ProblemDetails
	[ApiController]  // api specifics
					 //[Route("api/fruit")]partial
	[Route("[controller]")]
	public class FruitController : ControllerBase
	{
		private readonly FruitService _fruitService;
		public FruitController(FruitService fruitService)
		{
			_fruitService = fruitService;
		}

		[HttpGet("fruit")]  // defines the route template so -> /fruit
		//[Route("")]  // can have multiple [RouteAttributes]  // also gives /api/fruit bc of [Route("api/fruit")]
        //[Route("/hi")]  // this will not give /api/fruit/hi bc it starts with '/'
        //[HttpGet]
		public IEnumerable<string> Index()  // the name of action is not used for routing
		{
			return _fruitService.Fruit;
		}

		[HttpGet("fruit/{id}")]  // so -> /fruit/{id}
		//[Route("view/{id}")]
		//[Route("view-hi/{id}")]
		//[HttpGet]
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
