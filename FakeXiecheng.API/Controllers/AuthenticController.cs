using FakeXiecheng.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FakeXiecheng.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticController : ControllerBase
    {
        //添加配置服务的依赖
        private readonly IConfiguration _configuration;

        public AuthenticController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Authorize]
        [HttpPost("test")]
        public IActionResult Test(string email)
        {

            return Ok(email);
        }

        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult Login(string email,string password)
        {

            //1、验证用户名和密码

            //2、创建jwt
            // header
            var signingAlgorithm = SecurityAlgorithms.HmacSha256;
            // payload
            var claims = new[]
            {
                //sub
                new Claim(JwtRegisteredClaimNames.Sub,email),
                new Claim(ClaimTypes.Role,"Admin")
            };
            // signiture
            var secretByte = Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]);  //从配置文件中获取私钥
            var signingKey = new SymmetricSecurityKey(secretByte); //非对称加密
            var signingCredentials = new SigningCredentials(signingKey,signingAlgorithm);  //生成签名

            var token = new JwtSecurityToken(
                    issuer: _configuration["Authentication:Issuer"],
                    audience: _configuration["Authentication:Audience"],
                    claims,
                    notBefore:DateTime.UtcNow,
                    expires:DateTime.UtcNow.AddDays(1),
                    signingCredentials
                );

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);  //将JWT token转换成字符串

            //3、 return 200 ok + jwt
            return Ok(tokenStr);
        }


    }
}
