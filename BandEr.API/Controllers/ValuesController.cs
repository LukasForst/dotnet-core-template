using System.Collections.Generic;
using BandEr.BL.Facades;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BandEr.DAL.DTO;

namespace BandEr.API.Controllers
{
    public class ValuesController : ApiControllerBase
    {
        private readonly TodoFacade _facade;

        public ValuesController(TodoFacade facade)
        {
            _facade = facade;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ValueListDto>>> GetAsync()
        {
            return await _facade.GetAsync();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ValueDetailDto>> GetAsync(int id)
        {
            return await _facade.GetAsync(id);
        }

        // POST api/values
        [HttpPost]
        public async Task PostAsync([FromBody] AddValueDto model)
        {
            await _facade.AddAsync(model.Value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task PutAsync(int id, [FromBody] UpdateValueDto model)
        {
            await _facade.UpdateAsync(id, model.Value);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task DeleteAsync(int id)
        {
            await _facade.DeleteAsync(id);
        }
    }
}
