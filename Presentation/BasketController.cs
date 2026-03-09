using Microsoft.AspNetCore.Mvc;
using ServicesAbstractions;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    [ApiController]
    [Route("/[controller]")] //baseurl \Basket
    public class BasketController(IServiceManger _serviceManger) : Controller
    {
        [HttpGet("{id}")] 
        public async Task<ActionResult<BasketDto>>Get(string id)
        {
            var Result = await _serviceManger.BasketService.GetBasketAync(id);
            return Ok(Result);
        }
        [HttpPost]
        public async Task<ActionResult<BasketDto>>Update(BasketDto dto, TimeSpan? TimeToLive)
        {
            var result = await _serviceManger.BasketService.updateBasketAsync(dto,TimeToLive);
            return Ok(result);

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _serviceManger.BasketService.DeletebasketAsync(id);
            return NoContent(); //204 
        }
    }
}
