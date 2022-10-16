using BuyersFunctionApp.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyersFunctionApp.Repositories
{
    public interface IQueueProcessor
    {
        Task<ActionResult> SendMessageAsync(BuyerProduct product);

        Task ReceiveMessageAsync();
    }
}
