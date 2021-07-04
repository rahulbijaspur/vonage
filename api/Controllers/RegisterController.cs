using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dto;
using api.Entities;
using api.Interfaces;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenTokSDK;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {

        private readonly DataContext _context;

        public RegisterController( DataContext context)
        {
            
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult> register( string username)
        {
            var user = new User();
            user.Username = username;
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok("User added");

        }




    }



}
