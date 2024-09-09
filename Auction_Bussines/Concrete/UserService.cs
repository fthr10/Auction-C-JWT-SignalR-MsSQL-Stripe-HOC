using Auction_Bussines.Abstraction;
using Auction_Bussines.Dtos;
using Auction_Core.Models;
using Auction_Data_Access.Context;
using Auction_Data_Access.Enums;
using Auction_Data_Access.Models;
using AutoMapper;
using Azure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Auction_Bussines.Concrete
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ApiResponse _response;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private string secretKey;

        public UserService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ApiResponse response, IMapper mapper, IConfiguration _configuration, ApplicationDbContext context)
        {
            _userManager = userManager;
            _response = response;
            _mapper = mapper;
            _context = context;
            _roleManager = roleManager;
            secretKey = _configuration.GetValue<string>("SecretKey:jwtKey");
        }

        public async Task<ApiResponse> Login(LoginRequestDTO model)
        {
            ApplicationUser UserFromDb = _context.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == model.Username.ToLower());
            if (UserFromDb != null)
            {
                bool isValid = await _userManager.CheckPasswordAsync(UserFromDb, model.Password);
                if (!isValid)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Your Entry Information is not correct");
                    _response.isSucces = false;
                    return _response;
                }

                var role = await _userManager.GetRolesAsync(UserFromDb);
                JwtSecurityTokenHandler tokenHandler = new();
                byte[] key = Encoding.ASCII.GetBytes(secretKey);

                SecurityTokenDescriptor tokenDescriptor = new()
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, UserFromDb.Id),
                        new Claim(ClaimTypes.Email, UserFromDb.Email),
                        new Claim(ClaimTypes.Role, role.FirstOrDefault() == null ? "NormalUser" : role.FirstOrDefault()),
                        new Claim("fullName", UserFromDb.FullName)
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

                LoginResponseModel _model = new()
                {
                    Email = UserFromDb.Email,
                    Token = tokenHandler.WriteToken(token),
                };

                _response.Result = _model;
                _response.isSucces = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return _response;
            }
            else
            {
                _response.isSucces = false;
                _response.ErrorMessages.Add("Opps! something went wrong");
                return _response;
            }
        }

        public async Task<ApiResponse> Register(RegisterRequestDTO model)
        {
            var userFromDb = _context.ApplicationUsers.FirstOrDefault(x => x.UserName.ToLower() == model.UserName.ToLower());
            if (userFromDb != null)
            {
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.isSucces = false;
                _response.ErrorMessages.Add("Username already exist");
                return _response;
            }

            //var newUser = _mapper.Map<ApplicationUser>(model);

            ApplicationUser newUser = new()
            {
                FullName = model.FullName,
                UserName = model.UserName,
                NormalizedEmail = model.UserName.ToUpper(),
                Email = model.UserName
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (result.Succeeded)
            {
                var isTrue = _roleManager.RoleExistsAsync(UserType.Administrator.ToString()).GetAwaiter().GetResult();
                if (!_roleManager.RoleExistsAsync(UserType.Administrator.ToString()).GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserType.Administrator.ToString()));
                    await _roleManager.CreateAsync(new IdentityRole(UserType.Seller.ToString()));
                    await _roleManager.CreateAsync(new IdentityRole(UserType.NormalUser.ToString()));
                }

                if (model.UserType.ToString().ToLower() == UserType.Administrator.ToString().ToLower())
                {
                    await _userManager.AddToRoleAsync(newUser, UserType.Administrator.ToString());
                }
                if (model.UserType.ToString().ToLower() == UserType.Seller.ToString().ToLower())
                {
                    await _userManager.AddToRoleAsync(newUser, UserType.Seller.ToString());
                }
               else if (model.UserType.ToString().ToLower() == UserType.NormalUser.ToString().ToLower())
                {
                    await _userManager.AddToRoleAsync(newUser, UserType.NormalUser.ToString());
                }
                _response.StatusCode = System.Net.HttpStatusCode.Created;
                _response.isSucces = true;
                return _response;
            }

            foreach (var error in result.Errors)
            {
                _response.ErrorMessages.Add(error.ToString());
            }
            return _response;
        }
    }
}
