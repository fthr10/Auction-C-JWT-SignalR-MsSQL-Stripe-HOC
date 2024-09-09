using Auction_Bussines.Dtos;
using Auction_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_Bussines.Abstraction
{
    public interface IUserService
    {
        Task<ApiResponse> Register(RegisterRequestDTO model);

        Task<ApiResponse> Login(LoginRequestDTO model);
    }
}
