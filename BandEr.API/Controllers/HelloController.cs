using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BandEr.API.Controllers
{
    public class HelloController : ApiControllerBase
    {
        [HttpGet]
        public async Task<string> SayHello()
        {
            return await Task.Run(() => "Hello World");
        }
    }
}