using Auction_Bussines.Dtos;
using Auction_Data_Access.Domain;
using Auction_Data_Access.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_Bussines.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateVehicleDTO, Vehicle>().ReverseMap();
            CreateMap<UpdateVehicleDTO, Vehicle>().ReverseMap();
            CreateMap<CreateBidDTO, Bid>().ReverseMap();
            CreateMap<UpdateBidDTO, Bid>().ReverseMap();
            CreateMap<CreatePaymentHistoryDTO, PaymentHistory>().ReverseMap();
        }
    }
}
