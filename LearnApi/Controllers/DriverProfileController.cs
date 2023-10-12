using LearnApi.COR;
using LearnApi.Entity;
using LearnApi.Models;
using LearnApi.Models.DTO;
using LearnApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing.Constraints;
using SharpCompress.Common;
using System.IO;
using System.IO.Pipelines;

namespace LearnApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverProfileController : ControllerBase
    {
        public IUnitOfWork unitOfWork;
        public DriverProfileController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("CreateProfile")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProfile([FromForm] RegisterNewDriverDto registerNewDriverDto)
        {
            try
            {
                if (registerNewDriverDto == null || registerNewDriverDto.profilePicture == null)
                {
                    return BadRequest("Invalid request data.");
                }

                using (var memoryStream = new MemoryStream())
                {
                    await registerNewDriverDto.profilePicture.CopyToAsync(memoryStream);

                    DriverProfile newDriver = new DriverProfile
                    {
                        Number = registerNewDriverDto.Number,
                        DriverName = registerNewDriverDto.DriverName,
                        Team = registerNewDriverDto.Team,
                        DriverImage = memoryStream.ToArray()
                    };

                    await unitOfWork.DriverProfileRepository.InsertOneAsync(newDriver);

                    return CreatedAtAction(nameof(GetDriverProfile), new { id = newDriver.DriverName });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetDriverProfile/{driverName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDriverProfile([FromRoute] string driverName)
        {
            try
            {
                var response= await unitOfWork.DriverProfileRepository.FindOneAsync(driver => driver.DriverName == driverName);
                if (response == null)
                    return NotFound(driverName);
                return Ok(response);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
           
        }
    }
}
