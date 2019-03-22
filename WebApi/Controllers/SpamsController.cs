using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SpamsController : ControllerBase
	{
		// GET api/values
		[HttpGet]
		public ActionResult<int> Get()
		{
			return 200;
		}

		// POST api/values
		[HttpPost]
		public ActionResult<int> Post([FromBody] string text)
		{
			return (int)Program.filter.IsSpam(text);
		}

		// PUT api/values/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/values/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
