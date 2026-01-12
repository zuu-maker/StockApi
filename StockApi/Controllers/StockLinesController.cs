using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockApi.Services;
using StockApi.Modals;
using StockApi.Dtos;

namespace StockApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockLinesController (IStockLinesService service): ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<List<StockLine>>> GetStockLines()
        {
            var result = await service.GetStockLinesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StockLine>> GetStockLine(int id)
        {
            var stockLine = await service.GetStockLineAsync(id);
            return Ok(stockLine);
        }

        [HttpPost]
        public async Task<ActionResult<StockLine>> addStockLine(StockLineCreateDto newStockLine)
        {
            var createdStockLine = await service.AddStockLineAsync(newStockLine);
            return Ok(createdStockLine);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> updateStockLine(int id, StockLineUpdateDto stockLine)
        {
            var result = await service.UpdateStockLineAsync(id, stockLine);
            return Ok(result);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> removeStockLine(int id)
        {
            var result = await service.DeleteStockLineAsync(id);
            return Ok(result);
        }
    }
}
