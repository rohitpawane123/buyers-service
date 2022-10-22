using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyersFunctionApp.Model
{
    public class SellerProduct
    {
        [BsonId]
        public ObjectId ProductId { get; private set; }

        [BsonElement("productName")]
        public string ProductName { get; set; }
        [BsonElement("shortDescription")]
        public string? ShortDescription { get; set; }
        [BsonElement("detailedDescription")]
        public string? DetailedDescription { get; set; }
        [BsonElement("category")]
        public string? Category { get; set; }
        [BsonElement("startingPrice")]
        public double StartingPrice { get; set; }
        [BsonElement("bidEndDate")]
        public DateTime BidEndDate { get; set; }

        [BsonElement("firstName")]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        public string? LastName { get; set; }
        [BsonElement("address")]
        public string? Address { get; set; }
        [BsonElement("city")]
        public string? City { get; set; }
        [BsonElement("state")]
        public string? State { get; set; }
        [BsonElement("pin")]
        public string? Pin { get; set; }
        [BsonElement("phone")]
        public string? Phone { get; set; }

        [BsonElement("email")]
        public string? Email { get; set; }

        public SellerProduct()
        {
            this.ProductId = ObjectId.GenerateNewId();
        }
    }
}
