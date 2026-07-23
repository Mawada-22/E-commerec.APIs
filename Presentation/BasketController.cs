using Microsoft.AspNetCore.Authorization;
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
    // Anonymous by design: the storefront creates the basket (client-generated
    // GUID id) while browsing, before the shopper ever logs in. Server-side
    // prices are never trusted from it - Payment/Order re-price from the DB.
    public class BasketController(IServiceManager _serviceManger) : ApiController
    {
        // The storefront sends the id as a query string (?id=...), not a route segment.
        [HttpGet]
        public async Task<ActionResult<BasketDto>>Get([FromQuery] string id)
        {
            var Result = await _serviceManger.BasketService.GetBasketAsync(id);
            return Ok(Result);
        }
        [HttpPost]
        public async Task<ActionResult<BasketDto>>Update(BasketDto dto, TimeSpan? TimeToLive)
        {
            var result = await _serviceManger.BasketService.UpdateBasketAsync(dto,TimeToLive);
            return Ok(result);

        }
        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery] string id)
        {
            await _serviceManger.BasketService.DeleteBasketAsync(id);
            return NoContent(); //204 
        }
    }
}
