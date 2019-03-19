using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NoScam.Net;

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
		public ActionResult<bool> Post([FromBody] string text)
		{
			return Program.filter.IsSpam(Corpus.OfText(text));
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
