using Microsoft.AspNetCore.Mvc;

namespace StockAPI.Controllers
{
    [ApiController]
    [Route("api/stocks")]
    public class StockController : ControllerBase
    {
        private readonly AlphaClient _alphaClient;

        public StockController(AlphaClient alphaClient)
        {
            _alphaClient = alphaClient;
        }

        // GET api/stocks/{symbol}
        [HttpGet("{symbol}")]
        public async Task<IActionResult> GetIntraday(string symbol)
        {
            // Call AlphaClient to get intraday data
            var data = await _alphaClient.GetIntraday(symbol);
            
            // If no data is returned, then send 500 error
            if (data == null)
                return StatusCode(500, "Error Retrieving Data from Alpha Vantage");

            return Ok(data);            
        }
    }
}