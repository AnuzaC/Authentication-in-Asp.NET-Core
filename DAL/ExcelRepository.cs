using Authentication.Interface;
using Authentication.Model;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Authentication.DAL
{
    public class ExcelRepository : IExcelRepository
    {
        public FunctionResponse ImportExcel(IFormFile file)
        {
            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                var filePath = GetPath(file.FileName);
                DataSet result;
                
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);

                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        result = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            UseColumnDataType = true,
                            ConfigureDataTable = (tableReader) =>
                            new ExcelDataTableConfiguration()
                            {
                                EmptyColumnNamePrefix = "Col",
                                UseHeaderRow = true,

                            }
                        });
                    }
                }
                DataTable table = result.Tables[0];

                //List<ImportRequest> cardList = new List<ImportRequest>();
                //for(int i=0;i<table.Rows.Count; i++)
                //{
                //    ImportRequest user = new ImportRequest();
                //    user.cardNo = Convert.ToInt32(table.Rows[i]["cardNo"]);
                //    user.cardValue = table.Rows[i]["cardValue"].ToString();
                //    user.expiryDate = DateTime.Parse(table.Rows[i]["expiryDate"].ToString());
                //    user.mobile = table.Rows[i]["mobile"].ToString();
                //    user.remarks = table.Rows[i]["remarks"].ToString();
                //    cardList.Add(user);
                //}


                //------convert datatable into list using LINQ-------

                var parsedResult = JsonConvert.DeserializeObject<List<ImportRequest>>
                  (
                  JsonConvert.SerializeObject(table.AsEnumerable().
                     Select(row => new ImportRequest
                     {
                         cardNo = string.IsNullOrEmpty(row["CARDNO"].ToString())? throw new Exception(): int.Parse(row["CARDNO"].ToString()),
                         cardValue = string.IsNullOrEmpty(row["CARDVALUE"].ToString()) ? throw new Exception() : row["CARDVALUE"].ToString(),
                         expiryDate = string.IsNullOrEmpty(row["EXPIRYDATE(mm/DD/YYYY)"].ToString()) ? throw new Exception() : DateTime.Parse(row["EXPIRYDATE(mm/DD/YYYY)"].ToString()),
                         mobile = string.IsNullOrEmpty(row["MOBILE"].ToString()) ? throw new Exception() : row["MOBILE"].ToString(),
                         remarks = string.IsNullOrEmpty(row["REMARKS"].ToString()) ? throw new Exception() : row["REMARKS"].ToString()
                     }).ToList())
                  );
                return new FunctionResponse { status = "ok", result = parsedResult };

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public string GetPath(string fileName)
        {
            var dir = Directory.GetCurrentDirectory();
            var directoryPath = Path.Combine(dir, "ExcelFile");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var path = Path.Combine(directoryPath, fileName);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
           
            return path;
        }
    }
}
