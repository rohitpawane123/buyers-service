using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyersFunctionApp.Model
{
    public class BuyerProduct
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("productId")]
        public string ProductId { get; set; }

        [BsonElement("bidAmount")]
        public double BidAmount { get; set; }

        [BsonElement("firstName")]
        [MinLength(5)]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        public string LastName { get; set; }

        [BsonElement("address")]
        public string Address { get; set; }

        [BsonElement("city")]
        public string City { get; set; }
        [BsonElement("state")]
        public string State { get; set; }
        [BsonElement("pin")]
        public string Pin { get; set; }
        [BsonElement("phone")]
        public string Phone { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        public BuyerProduct()
        {
            Id = ObjectId.GenerateNewId();
        }

    }
}
