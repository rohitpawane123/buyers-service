using BuyersFunctionApp.Model;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyersFunctionApp.Repositories
{
    public class BuyerRepository : IBuyerRepository
    {
        private readonly IMongoCollection<BuyerProduct> _buyerCollection;

        public BuyerRepository(IMongoDatabase mongoDatabase)
        {
            _buyerCollection = mongoDatabase.GetCollection<BuyerProduct>("buyerproduct");
        }

        public async Task<ActionResult> DeleteBid(ObjectId id)
        {
            try
            {
                await _buyerCollection.FindOneAndDeleteAsync(Builders<BuyerProduct>.Filter.Eq("_id", id));

                return new OkObjectResult("Bid Deleted successfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
                throw;
            }
        }

        public async Task<ActionResult<IEnumerable<BuyerProduct>>> GetAllBidsAsync()
        {
            return await _buyerCollection.Find(_ => true).ToListAsync<BuyerProduct>();
        }

        public async Task PlaceBidAsync(BuyerProduct product)
        {
            try
            {
                await _buyerCollection.InsertOneAsync(product);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<ActionResult> UpdateBid(ObjectId productId, string buyerEmail, double newBidAmount)
        {
            try
            {
                await _buyerCollection.FindOneAndUpdateAsync<BuyerProduct>(_ => _.ProductId.Equals(productId), Builders<BuyerProduct>.Update.Set("email", buyerEmail).Set("bidamount", newBidAmount));

                return new OkObjectResult("Bid Updated successfully");
            }
            catch (Exception ex)
            {
                return new NoContentResult();
                throw;
            }

        }
    }
}
