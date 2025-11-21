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
        public async Task<IActionResult> GetDailyAverages(string symbol)
        {

            try
            {
                // Call AlphaClient to get intraday data
                var data = await _alphaClient.GetDailyAverages(symbol);
                return Ok(data);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Alpha Vantage"))
                {
                    // Error from Alpha Vantage API
                    return BadRequest(new
                    {
                        ex.Message,
                        symbol = symbol.ToUpper(),
                    });
                }

                // Unknown errors get 500
                return StatusCode(500, new
                {
                    error = "An unexpected error occurred.",
                    details = ex.Message
                });
            }
        }
    }
}