using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace OpenAPI_Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        /// <summary>
        /// Tests API
        /// </summary>
        /// <param name="Input">Input Parameter</param>
        /// <returns>JSON response</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     
        ///        Input: Hello
        ///    
        ///
        /// </remarks>
        [HttpGet("TestAPIRequest", Name = "Input Parameter")]
        public async Task<IActionResult> GetTestAPIRequestAsync(string Input)
        {
            try
            {
                var responseObject = new {
                    Input = Input 
                };

                return await Task.FromResult(Ok(responseObject));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(StatusCode(500, "Internal Server Error: " + ex.Message));
            }
        }
    }
}
