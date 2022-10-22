using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using BuyersFunctionApp.Model;
using BuyersFunctionApp.Repositories;
using MongoDB.Bson;
using System.Collections.Generic;

namespace BuyersFunctionApp
{
    public class BuyerFunction
    {
        private readonly IBuyerRepository _buyer;

        public BuyerFunction(IBuyerRepository buyer)
        {
            _buyer = buyer;
        }


        [FunctionName("get-all-bids")]
        public async Task<ActionResult<IEnumerable<BuyerProduct>>> GetAllBids([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger logger)
        {
            return new OkObjectResult(await _buyer.GetAllBidsAsync());
        }

        [FunctionName("place-bid")]
        public async Task<ActionResult> PlaceBid([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger logger)
        {
            try
            {
                var reqBody = await new StreamReader(req.Body).ReadToEndAsync();
                var input = JsonConvert.DeserializeObject<BuyerProduct>(reqBody);
                var product = new BuyerProduct
                {
                    Address = input.Address,
                    BidAmount = input.BidAmount,
                    City = input.City,
                    Email = input.Email,
                    FirstName = input.FirstName,
                    LastName = input.LastName,
                    Phone = input.Phone,
                    Pin = input.Pin,
                    State = input.State,
                    ProductId = input.ProductId,
                };

                return new OkObjectResult(await _buyer.PlaceBidAsync(product));
            }
            catch (Exception ex)
            {
                logger.LogError("Exception Caught", ex);
                return new BadRequestResult();
                throw;

            }
        }

        [FunctionName("update-bid")]
        public async Task<ActionResult> UpdateBid([HttpTrigger(AuthorizationLevel.Function, "put", Route = "update-bid/{id}/{email}/{bidAmount}")] HttpRequest req, ILogger logger, string id, string email, double bidAmount)
        {
            try
            {
                await _buyer.UpdateBid(ObjectId.Parse(id), email, bidAmount);
                return new OkObjectResult("Bid Updated Successfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
                throw;
            }
        }

        [FunctionName("delete-bid")]
        public async Task<ActionResult> DeleteBid([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "delete-bid/{id}")] HttpRequest req, ILogger logger, string id)
        {
            try
            {
                await _buyer.DeleteBid(ObjectId.Parse(id));
                return new OkObjectResult("Bid Deleted Succesfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
                throw;
            }

        }

        [FunctionName("get-all-bids-by-productid")]
        public async Task<ActionResult<IEnumerable<BuyerProduct>>> GetAllBidsByProductId([HttpTrigger(AuthorizationLevel.Function, "get", Route = "get-all-bids-by-productid/{id}")] HttpRequest request, ILogger logger, string id)
        {
            try
            {
                return new OkObjectResult(await _buyer.GetAllBidsByProductId(id));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
                throw;
            }
        }


    }
}
