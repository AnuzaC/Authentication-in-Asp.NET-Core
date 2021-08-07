using Authentication.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication.Interface
{
    public interface IExcelRepository
    {
        FunctionResponse ImportExcel(IFormFile card);
    }
}
