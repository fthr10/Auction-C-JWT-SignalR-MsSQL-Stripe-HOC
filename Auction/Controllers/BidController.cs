using Auction_Bussines.Abstraction;
using Auction_Bussines.Dtos;
using Auction_Data_Access.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly IBidService _bidService;

        public BidController(IBidService bidService)
        {
            _bidService = bidService;
        }

        [HttpPost("Create")]

        public async Task<IActionResult> CreateBid(CreateBidDTO model)
        {
            if (ModelState.IsValid) 
            {
                    var response = await _bidService.CreateBid(model);                    
                    return Ok(response); 
            }
            return BadRequest();
        }

        [HttpGet("{bidId:int}")]
        public async Task<IActionResult> GetBidById(int bidId)
        {
                var response = await _bidService.GetBidById(bidId);
                if (!response.isSucces)
                {
                    return BadRequest(response);
                }
                return Ok(response);
        }


        [HttpPut("{bidId:int}")]
        public async Task<IActionResult> ModifyBid(int bidId, UpdateBidDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _bidService.UpdateBid(bidId,model);
                if (!response.isSucces)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            return BadRequest();
        }

        [HttpPost]

        public async Task<IActionResult> AutoBid(CreateBidDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _bidService.AutomaticallyCreateBid(model);
                if (!response.isSucces)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            return BadRequest();
        }

        [HttpGet("GetBidsByVehicle/{vehicleId:int}")]
        public async Task<IActionResult> GetBidbyVehicle(int vehicleId)
        {
            var response = await _bidService.GetBidByVehicleId(vehicleId);
            if (!response.isSucces)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }



    }
}
