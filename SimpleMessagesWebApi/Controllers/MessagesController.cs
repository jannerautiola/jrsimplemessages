using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SimpleMessagesWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        //// GET api/values
        //[HttpGet]
        //public ActionResult<IEnumerable<string>> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/values/5
        [HttpGet("{key}")]
        public async Task<ActionResult<object>> Get(string key)
        {
            MessageHandler handler = new MessageHandler();
            var messages = await handler.GetMessages(key);
            return messages;
        }

        // POST api/values
        [HttpPost("{key}")]
        public async Task Post([FromRoute] string key)
        {
            MessageHandler handler = new MessageHandler();
            var queryItems = this.HttpContext.Request.Query.ToDictionary(i => i.Key, k => k.Value.ToString());
            await handler.SaveMessage(key, queryItems);
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
