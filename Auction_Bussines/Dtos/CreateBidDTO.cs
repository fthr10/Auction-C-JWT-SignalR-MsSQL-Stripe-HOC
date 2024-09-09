using Auction_Data_Access.Domain;
using Auction_Data_Access.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_Bussines.Dtos
{
    public class CreateBidDTO
    {
        public decimal BidAmount { get; set; }

        public string? UserId { get; set; }

        public int VehicleId { get; set; }

    }
}
