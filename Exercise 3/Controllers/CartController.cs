using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Exercise_4.Eventstore;
using Microsoft.AspNetCore.Mvc;

namespace Exercise_4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private static readonly Dictionary<int, List<Product>> Carts = new Dictionary<int, List<Product>>();

        [HttpGet("{userId}")]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        public ActionResult<IEnumerable<Product>> Get(int userId)
        {
            Console.WriteLine($"Get cart for user {userId}");
            if (!Carts.ContainsKey(userId))
                return StatusCode(404);

            return Ok(Carts[userId]);
        }

        [HttpPost("{userId}")]
        public ActionResult<IEnumerable<Product>> Post(int userId, [FromBody] Product product)
        {
            Console.WriteLine($"Add {product.Name} to cart for user {userId}");
            if (!Carts.ContainsKey(userId))
                Carts.Add(userId, new List<Product>());

            Carts[userId].Add(product);

            _eventStore.Add(new Event()
            {
                EventName = "Product added",
                Products = product,
                UserId = userId
            });

            return Ok(Carts[userId]);
        }

        [HttpDelete("{userId}")]
        public ActionResult Delete(int userId)
        {
            Console.WriteLine($"Cleaning cart for the user {userId}");

            if (!Carts.ContainsKey(userId))
                return NotFound();

            Carts.Remove(userId);
            _eventStore.Add(new Event()
            {
                EventName = "Cart removed",
                Products = null,
                UserId = userId
            }
            );

            return NoContent();
        }

        [HttpDelete("{userId}/{productName}")]
        public ActionResult Delete(int userId, string productName)
        {
            Console.WriteLine($"Removing {productName} from user {userId}'s cart");
            if (!Carts.ContainsKey(userId))
                return NotFound();

            Carts[userId] = Carts[userId].Where(x => x.Name != productName).ToList();

            if (!Carts[userId].Any())
                Carts.Remove(userId);

            _eventStore.Add(new Event()
            {
                EventName = "Product deleted",
                Products = new Product { Name = productName },
                UserId = userId
            }
            );

            return Ok(Carts[userId]);
        }

        [HttpGet("events")]
        public ActionResult GetEvents(long firstEventSequenceNumber = 1, long lastEventSequenceNumber = 100)
        {
            return Ok(_eventStore.GetEvents(firstEventSequenceNumber, lastEventSequenceNumber));
        }

        private static readonly EventStore _eventStore = new EventStore();
    }
}