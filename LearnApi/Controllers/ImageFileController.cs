using LearnApi.Models;
using LearnApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing.Constraints;
using System.IO;

namespace LearnApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageFileController : ControllerBase
    {
        private readonly FileManagementService fileManagementService;
        public ImageFileController(IWebHostEnvironment webHostEnvironment, ILogger<FileManagementService> logger)
        {
            fileManagementService = new FileManagementService(webHostEnvironment, "images", logger);
        }

        [HttpPut]
        [Route("UploadFile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadFile(IFormFile formFile)
        {
            ApiResponse response = await fileManagementService.Upload(formFile, "");
            IActionResult result;

            switch (response.StatusCode)
            {
                case StatusCodes.Status200OK:
                    result = Ok(response);
                    break;
                case StatusCodes.Status500InternalServerError:
                    result = StatusCode(StatusCodes.Status500InternalServerError, response);
                    break;
                case StatusCodes.Status404NotFound:
                    result = NotFound(response);
                    break;
                default:
                    result = BadRequest(response);
                    break;
            }

            return result;
        }

        [HttpPut]
        [Route("UploadMultiplesFile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadMultiplesFile(IFormFileCollection fileCollection)
        {
            ApiResponse response = await fileManagementService.UploadMultipleFiles(fileCollection, "");

            IActionResult result;

            switch (response.StatusCode)
            {
                case StatusCodes.Status200OK:
                    result = Ok(response);
                    break;
                case StatusCodes.Status500InternalServerError:
                    result = StatusCode(StatusCodes.Status500InternalServerError, response);
                    break;
                case StatusCodes.Status404NotFound:
                    result = NotFound(response);
                    break;
                default:
                    result = BadRequest(response);
                    break;
            }

            return result;
        }

        [HttpGet]
        [Route("GetFile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFile(string fileName)
        {
            ApiResponse response = await fileManagementService.GetSingleFile(fileName, "");
            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";

            IActionResult result;

            switch (response.StatusCode)
            {
                case StatusCodes.Status200OK:
                    response.Result = hosturl + response.Result;
                    result = Ok(response);
                    break;
                case StatusCodes.Status500InternalServerError:
                    result = StatusCode(StatusCodes.Status500InternalServerError, response);
                    break;
                case StatusCodes.Status404NotFound:
                    result = NotFound(response);
                    break;
                default:
                    result = BadRequest(response);
                    break;
            }

            return result;

        }
        [HttpGet]
        [Route("GetMultipleFile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMultipleFile(List<string> filesName)
        {
            ApiResponse response = await fileManagementService.GetMultipleFiles(filesName, "");

            IActionResult result;

            switch (response.StatusCode)
            {
                case StatusCodes.Status200OK:
                    result = Ok(response);
                    break;
                case StatusCodes.Status500InternalServerError:
                    result = StatusCode(StatusCodes.Status500InternalServerError, response);
                    break;
                case StatusCodes.Status404NotFound:
                    result = NotFound(response);
                    break;
                default:
                    result = BadRequest(response);
                    break;
            }

            return result;
        }
        [HttpGet]
        [Route("DownloadFile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DownloadFile(string fileName)
        {
            string filePath = Path.Combine(fileManagementService.GetMainFilePath(""), fileName); // Obtener la ruta del archivo

            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                    // Configurar el resultado para transmitir el archivo
                    var result = new FileStreamResult(fileStream, "application/octet-stream")
                    {
                        FileDownloadName = fileName // Nombre del archivo que se mostrará al descargar
                    };

                    return result;
                }catch(Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Error downloading {ex.Message}");
                }
            }
            else
            {
                return NotFound(fileName); // Devolver NotFound si el archivo no existe
            }
        }

        [HttpDelete]
        [Route("DeleteFile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            ApiResponse response = await fileManagementService.RemoveSingleFile(fileName, "");

            IActionResult result;

            switch (response.StatusCode)
            {
                case StatusCodes.Status200OK:
                    result = Ok(response);
                    break;
                case StatusCodes.Status500InternalServerError:
                    result = StatusCode(StatusCodes.Status500InternalServerError, response);
                    break;
                case StatusCodes.Status404NotFound:
                    result = NotFound(response);
                    break;
                default:
                    result = BadRequest(response);
                    break;
            }

            return result;
        }

        [HttpDelete]
        [Route("DeleteMultipleFile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMultipleFile(List<string> filesName)
        {
            ApiResponse response = await fileManagementService.RemoveMultipleFiles(filesName, "");

            IActionResult result;

            switch (response.StatusCode)
            {
                case StatusCodes.Status200OK:
                    result = Ok(response);
                    break;
                case StatusCodes.Status500InternalServerError:
                    result = StatusCode(StatusCodes.Status500InternalServerError, response);
                    break;
                case StatusCodes.Status404NotFound:
                    result = NotFound(response);
                    break;
                default:
                    result = BadRequest(response);
                    break;
            }

            return result;
        }

    }
}
