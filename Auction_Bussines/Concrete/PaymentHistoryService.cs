using Auction_Bussines.Abstraction;
using Auction_Bussines.Dtos;
using Auction_Core.Models;
using Auction_Data_Access.Context;
using Auction_Data_Access.Domain;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_Bussines.Concrete
{
    public class PaymentHistoryService : IPaymentHistoryService
    {
        private ApiResponse _response;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
       
        public PaymentHistoryService(ApplicationDbContext context, IMapper mapper,ApiResponse response)
        {
            _context = context;
            _mapper = mapper;
            _response = new ApiResponse();
        }
        
        public async Task<ApiResponse> CheckIsStatusForAuction(string userId, int vehicleId)
        {
            var response = await _context.PaymentHistories.Where(x => x.UserId == userId && x.VehicleId == vehicleId && x.IsActive == true).FirstOrDefaultAsync();
            if (response != null)
            {
                _response.isSucces = true;
                _response.Result = response;
                return _response;
            }
            _response.isSucces = false;
            return _response;
        }

        public async Task<ApiResponse> CreatePaymentHistory(CreatePaymentHistoryDTO model)
        {
            if (model==null) 
            {
                _response.isSucces = false;
                _response.ErrorMessages.Add("Model is not include some fields");
                return _response;
            }
            else 
            {
                var objDTO = _mapper.Map<PaymentHistory> (model);
                objDTO.PayDate = DateTime.Now;
                objDTO.IsActive = true;
                _context.PaymentHistories.Add(objDTO);
                _context.PaymentHistories.Add(objDTO);
                if (await _context.SaveChangesAsync()>0)
                {
                    _response.isSucces = true;
                    _response.Result = model;
                    return _response;

                }
                _response.isSucces = false;
                _response.ErrorMessages.Add("OOoooops! something went wrong!");
                return _response;
            }
        }
    }
}
