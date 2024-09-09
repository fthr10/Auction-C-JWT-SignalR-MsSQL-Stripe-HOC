﻿using Auction_Bussines.Abstraction;
using Auction_Bussines.Dtos;
using Auction_Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentHistoryController : ControllerBase
    {
        private readonly IPaymentHistoryService _paymentHistoryService;
        public PaymentHistoryController(IPaymentHistoryService paymentHistoryService)
        {
            _paymentHistoryService = paymentHistoryService;
        }

        [HttpPost("AddHistory")]
        public async Task<IActionResult> CreatePaymentHistory(CreatePaymentHistoryDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _paymentHistoryService.CreatePaymentHistory(model);
                if (!response.isSucces)
                {
                    return BadRequest(response);

                }
                return Ok(response);
            }
            return BadRequest();
        }

        [HttpPost("CheckStatus")]
        public async Task<IActionResult> CheckStatusAuction(CheckStatusModel model)
        {
            var response = await _paymentHistoryService.CheckIsStatusForAuction(model.UserId, model.VehicleId);
            return Ok(response);

        }
    }
}
