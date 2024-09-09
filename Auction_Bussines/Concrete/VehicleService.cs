using Auction_Bussines.Abstraction;
using Auction_Bussines.Dtos;
using Auction_Core.Models;
using Auction_Data_Access.Context;
using Auction_Data_Access.Domain;
using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_Bussines.Concrete
{
    public class VehicleService : IVehicleService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private ApiResponse _response;

        public VehicleService(ApplicationDbContext context,ApiResponse response, IMapper mapper)
        {
            _response = response;
            _context = context;
            _mapper = mapper;
            
        }

        public async Task<ApiResponse> ChangeVehicleStatus(int vehicleId)
        {
            var result = await _context.Vehicles.FindAsync(vehicleId);
            if (result == null)
            {
                _response.isSucces=false;
                return _response;
            }
            result.IsActive=false;
            _response.isSucces=true;
            await _context.SaveChangesAsync();
            return _response;
            
        }

        public async Task<ApiResponse> CreateVehicle(CreateVehicleDTO model)
        {
            var _response = new ApiResponse();

            if (model != null)
            {
                var objDTO = _mapper.Map<Vehicle>(model);
                if (objDTO != null)
                {
                    _context.Vehicles.Add(objDTO);
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        _response.isSucces = true;
                        _response.Result = model;
                        _response.StatusCode = System.Net.HttpStatusCode.Created;
                        return _response;
                    }
                }
            }
            _response.isSucces = false;
            _response.ErrorMessages.Add("Oooops! something went wrong");
            return _response;
        }

        public async Task<ApiResponse> DeleteVehicle(int vehicleId)
        {
            var result = await _context.Vehicles.FindAsync(vehicleId);
            if (result != null)
            {
                _context.Vehicles.Remove(result);
                if (await _context.SaveChangesAsync()>0)
                {
                    _response.isSucces=true;
                    return _response;
                    
                }
            }
            _response.isSucces = false;
            return _response;
        }
        

        public async Task<ApiResponse> GetVehicleById (int vehicleId)
        {
            var result = await _context.Vehicles.Include(x=>x.Seller).Include(x=>x.Bids).FirstOrDefaultAsync(x=>x.VehicleId == vehicleId);
            if(result != null)
            {
                _response.Result = result;
                _response.isSucces = true;
                return _response;
            }
            _response.isSucces = false;
            return _response;
        }

        public async Task<ApiResponse> GetVehicles()
        {
            var vehicle = await _context.Vehicles.Include(x=>x.Seller).ToListAsync();
            if (vehicle != null)
            {
                _response.isSucces = true;
                _response.Result = vehicle;
                return _response;
            }
            _response.isSucces = false;
            return _response;
        }

        public async Task<ApiResponse> UpdateVehicleResponse(int vehicleId, UpdateVehicleDTO model)
        {
            var result = await _context.Vehicles.FindAsync(vehicleId);
            if (result != null) 
            {
                Vehicle objDTO = _mapper.Map(model, result);
                if (await _context.SaveChangesAsync()>0)
                {
                    _response.isSucces = true;
                    _response.Result = objDTO;
                    return _response;
                }
            }
            _response.isSucces= false;
            return _response;
        }
    }
}
