using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authentication.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Authentication.Model;

namespace Authentication.Controllers
{    
    [Authorize]
    [ApiController]
    public class LoginController : Controller
    {
        IUserRepository _user;
        public LoginController(IUserRepository user)
        {
            _user = user;
        }

        [AllowAnonymous]
        [Route("api/Authenticate")]
        [HttpPost]
        public IActionResult Post([FromBody] AuthecateRequest model)
        {
            try
            {
                var user = _user.Authenticate(model.userName, model.password);

                if (user.status != "ok")
                    return BadRequest(new { message = "Username or password is incorrect." });
                return Ok(user);
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        
        [HttpGet]
        [Route("api/GetUserByIdAsync")]
        public async  Task<IActionResult> GetUser([FromQuery] int userid)
        {
            try
            {
                var response = await _user.GetUserByIdAsync(userid);

                if (response.status != "ok")
                {
                    return BadRequest(new { message = "Invalid User ID" });
                }
                return Ok(response);
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        
        [HttpPost]
        [Route("api/SaveUser")]
        public IActionResult SaveUserCredentials([FromBody] User model)
        {
            try
            {
                if ( model.id == 0 && (model.userName == null || model.email == null || model.password == null))
                    throw new Exception(message: "Please enter user credentials");
                   
                var res  =_user.SaveUser(model);
                return new  OkObjectResult(res);
            }catch(Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }

        [HttpPost]
        [Route("api/UpdateUser")]
        public IActionResult UpdateUserCredentials([FromBody]User model)
        {
            try
            {
                if(model.id==0 )
                    throw new Exception(message: "Please enter user credentials");

                var res = _user.UpdateUser(model);

                return new OkObjectResult(res);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }
    }
}
