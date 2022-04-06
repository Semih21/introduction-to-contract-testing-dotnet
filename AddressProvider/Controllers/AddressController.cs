using AddressProvider.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AddressProvider.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly ILogger<AddressController> _logger;

        public AddressController(ILogger<AddressController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/address/{addressId}")]
        public ActionResult<Address> GetAddressForId(string addressId)
        {
            if (addressId.ToLower().Equals("this_is_not_a_valid_address_id"))
            {
                return BadRequest();
            }

            if (addressId.ToLower().Equals("00000000-0000-0000-0000-000000000000"))
            {
                return NotFound();
            }

            return new Address
            {
                Id = Guid.Parse(addressId),
                AddressType = "billing",
                Street = "Main Street",
                Number = 123,
                City = "Nothingville",
                ZipCode = 54321,
                State = "Tennessee",
                Country = "United States"
            };
        }

        [HttpDelete("/address/{addressId}")]
        public ActionResult DeleteAddressForId(string addressId)
        {
            if (addressId.ToLower().Equals("this_is_not_a_valid_address_id"))
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
