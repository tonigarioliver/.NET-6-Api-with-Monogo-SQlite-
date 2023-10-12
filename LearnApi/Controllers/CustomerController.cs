
using ClosedXML.Excel;
using LearnApi.Models;
using LearnApi.Servies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Data;

namespace LearnApi.Controllers
{
    [Authorize]
    [EnableRateLimiting("fixedwindow")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private ICustomerService customerService;
        private readonly IWebHostEnvironment environment;
        public CustomerController(ICustomerService customerService, IWebHostEnvironment environment)
        {
            this.customerService = customerService;
            this.environment = environment;
        }
        [DisableRateLimiting]
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAllCustomers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var customers= await customerService.GetAll();
            return Ok(customers);
        }
        [HttpGet]
        [Route("GetCustomerByName/{customerName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerModel>> GetByName([FromRoute] string customerName)
        {
            var customer = await customerService.GetbyName(customerName);
            if (customer == null)
            {
                return BadRequest();
            }
            return Ok(customer);
        }
        [HttpPost]
        [Route("CreateCustomer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<CustomerModel>> CreateCustomer([FromBody] CustomerModel createDTO)
        {
            CustomerModel newCustomer =await customerService.AddCustomer(createDTO);
            if (newCustomer==null)
            {
                return BadRequest(createDTO);
            }
            return Ok(newCustomer);
        }
        [HttpPut]
        [Route("UpdateCustomer/{customerName}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerModel>> UpdateCustomer([FromRoute] string customerName,[FromBody] CustomerModel update)
        {
            var response= await customerService.Update(update, customerName);
            if (response == null)
            {
                return BadRequest(response);
            }
            return NoContent();
        }
        [HttpDelete]
        [Route("DeleteCustomer/{customerName}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerModel>> DeleteCustomer([FromRoute] string customerName)
        {
            var response = await customerService.Remove(customerName);
            if (response == null)
            {
                return BadRequest(response);
            }
            return NoContent();
        }
        [HttpGet]
        [Route("ExportExcel")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult>ExportExcel()
        {
            try
            {
                string Filepath = GetFilepath();
                string excelpath = Filepath + "\\customerinfo.xlsx";
                DataTable dt = new DataTable();
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Email", typeof(string));
                dt.Columns.Add("Phone", typeof(string));
                dt.Columns.Add("CreditLimit", typeof(int));
                List<CustomerModel> data =await customerService.GetAll();
                if (data != null && data.Count > 0)
                {
                    data.ForEach(item =>
                    {
                        dt.Rows.Add(item.Name, item.Email, item.Phone, item.Creditlimit);
                    });
                }
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.AddWorksheet(dt, "Customer Info");
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);

                        if (System.IO.File.Exists(excelpath))
                        {
                            System.IO.File.Delete(excelpath);
                        }
                        wb.SaveAs(excelpath);

                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Customer.xlsx");
                    }
                }
            }catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }
        [NonAction]
        private string GetFilepath()
        {
            return this.environment.WebRootPath + "\\Export";
        }

    }

}
