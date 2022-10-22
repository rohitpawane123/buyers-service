using BuyersFunctionApp.Model;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
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
        private readonly IMongoCollection<SellerProduct> _sellerCollection;

        public BuyerRepository(IMongoDatabase mongoDatabase)
        {
            _buyerCollection = mongoDatabase.GetCollection<BuyerProduct>("buyerproduct");
            _sellerCollection = mongoDatabase.GetCollection<SellerProduct>("sellerproduct");
        }

        public async Task<ActionResult> DeleteBid(ObjectId id)
        {
            try
            {
                await _buyerCollection.FindOneAndDeleteAsync(_ => _.Id.Equals(id));

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

        public async Task<ActionResult<IEnumerable<BuyerProduct>>> GetAllBidsByProductId(string productId)
        {
            try
            {
                return new OkObjectResult(await _buyerCollection.Find(_ => _.ProductId.Equals(productId)).ToListAsync<BuyerProduct>());
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
                throw;
            }
        }

        public async Task<ActionResult> PlaceBidAsync(BuyerProduct product)
        {
            try
            {
                var sellerProduct = await _sellerCollection.Find(_ => _.ProductId.Equals(product.ProductId)).FirstOrDefaultAsync<SellerProduct>();
                if (sellerProduct == null)
                {
                    return new NotFoundResult();
                }
                else
                {
                    if (sellerProduct.StartingPrice <= product.BidAmount)
                    {
                        await _buyerCollection.InsertOneAsync(product);
                        return new OkObjectResult("Bid placed successfully");
                    }
                    else
                    {
                        return new BadRequestObjectResult("Bid Amouunt cannot be less than start bid amount");
                    }

                }

            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
                throw;
            }
        }

        public async Task<ActionResult> UpdateBid(ObjectId id, string buyerEmail, double newBidAmount)
        {
            try
            {
                await _buyerCollection.FindOneAndUpdateAsync<BuyerProduct>(_ => _.Id.Equals(id), Builders<BuyerProduct>.Update.Set("email", buyerEmail).Set("bidamount", newBidAmount));

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
