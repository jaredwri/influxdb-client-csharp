using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Core.Proposal.UseCase.Models;
using Microsoft.AspNetCore.Mvc;

namespace Client.Core.Proposal.UseCase.Controllers
{
    [Route("")]
    public class Example : ControllerBase
    {
        private IWriteApi WriteApi { get; }

        public Example(IWriteApi writeApi)
        {
            WriteApi = writeApi;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginMeasurement measurement)
        {
            try
            {
                await WriteApi.WriteAsync(measurement);
                return Ok();
            }
            catch (Exception exception)
            {
                // logging and capturing the actual return/status codes etc.
                return BadRequest();
            }
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutMeasurement measurement)
        {
            try
            {
                await WriteApi.WriteAsync(measurement);
                return Ok();
            }
            catch (Exception exception)
            {
                // logging and capturing the actual return/status codes etc.
                return BadRequest();
            }
        }

        [HttpPost("usage")]
        public async Task<IActionResult> Usage([FromBody] UsageMeasurement measurement)
        {
            try
            {
                await WriteApi.WriteAsync(measurement);
                return Ok();
            }
            catch (Exception exception)
            {
                // logging and capturing the actual return/status codes etc.
                return BadRequest();
            }
        }

    }
}
