using Authentication.Interface;
using Authentication.Model;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Authentication.Controllers
{
    [Authorize]
    [ApiController]
    public class ExcelImportExport : ControllerBase
    {
        IExcelRepository _cardUser;
        public ExcelImportExport(IExcelRepository card)
        {
            _cardUser = card;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("api/ImportExcel")]
        public   IActionResult GetFile([FromForm] IFormFile file)
        {
            try
            {
                var response=  _cardUser.ImportExcel(file);
                return new OkObjectResult(response);
            }catch(Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }
    }
}
