using BuyersFunctionApp.Model;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyersFunctionApp.Repositories
{
    public interface IBuyerRepository
    {
        Task<ActionResult<IEnumerable<BuyerProduct>>> GetAllBidsAsync();

        Task<ActionResult> PlaceBidAsync(BuyerProduct product);

        Task<ActionResult> UpdateBid(ObjectId productId, string buyerEmail, double newBidAmount);

        Task<ActionResult> DeleteBid(ObjectId id);

        Task<ActionResult<IEnumerable<BuyerProduct>>> GetAllBidsByProductId(string productId);

    }
}
