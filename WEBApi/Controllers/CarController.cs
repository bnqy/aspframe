using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WEBApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        [HttpGet("start")]
        [HttpGet("/start-car")]
        public IEnumerable<string> ListCars()
        {
            return new string[]
                { "Nissan Micra", "Ford Focus" };
        }
    }
}
