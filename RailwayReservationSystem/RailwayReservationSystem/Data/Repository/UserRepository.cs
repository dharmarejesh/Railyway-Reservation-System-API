using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RailwayReservationSystem.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace RailwayReservationSystem.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private RailwayDbContext _context;
        private readonly ILogger<UserRepository> _log;
        private readonly IConfiguration _configuration;

        public UserRepository(RailwayDbContext context, ILogger<UserRepository> logger, IConfiguration config) //Implementing Dependency Injection
        {
            _context = context;
            _log = logger;
            _configuration = config;
        }

        #region "Generate JWT Token"
        public string GenerateJWT(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var credintials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            Claim[] claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Gender, user.Gender),
                new Claim(ClaimTypes.DateOfBirth, user.Dob.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var securityToken = new JwtSecurityToken(_configuration["JWT:issuer"],
                                                    _configuration["JWT:audience"],
                                                    claims,
                                                    expires: DateTime.Now.AddMinutes(1500),
                                                    signingCredentials: credintials);
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
        #endregion

        #region "Check User"
        public User CheckUser(Login login)
        {
            try
            {
                var password = EncryptPassword(login.Password);
                if (login != null)
                    return _context.Users.FirstOrDefault(u => u.UserId == login.UserId && u.Password == password);
            }
            catch (Exception exc)
            {
                _log.LogError(exc.Message);
            }
            return null;

        }
        #endregion

        #region "Check Email"
        public bool CheckEmail(Register reg)
        {
            try
            {
                if (reg != null)
                    return _context.Users.FirstOrDefault(u => u.Email == reg.Email) == null;
            }
            catch(Exception exc)
            {
                _log.LogError(exc.Message);
            }
            return false;

        }
        #endregion

        #region Add User"
        public User AddUser(Register reg)
        {
            User user = new User
            {
                Name = reg.Name,
                Email = reg.Email,
                Dob = reg.Dob,
                Password = EncryptPassword(reg.Password),
                Gender = reg.Gender,
                Role = "User"
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }
        #endregion

        #region "Encrypt Password" //New Add
        public string EncryptPassword(string password)
        {
            MD5 md5 = MD5.Create();
            var enc = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(enc);
        }
        #endregion
    }
}
