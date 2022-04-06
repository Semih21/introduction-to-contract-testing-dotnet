using PactNet.Matchers;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Xunit;

namespace CustomerConsumer.Tests
{
    public class AddressApiConsumerDeleteTests : IClassFixture<ConsumerAddressApiPact>
    {
        private readonly IMockProviderService _mockProviderService;
        private readonly string _mockProviderServiceBaseUri;

        private readonly string addressId = "8aed8fad-d554-4af8-abf5-a65830b49a5f";

        public AddressApiConsumerDeleteTests(ConsumerAddressApiPact data)
        {
            _mockProviderService = data.MockProviderService;
            _mockProviderServiceBaseUri = data.MockProviderServiceBaseUri;
            _mockProviderService.ClearInteractions();
        }

        [Fact]
        public async Task DeleteAddressById_WhenTheAddressExists_ReturnsHttp204()
        {
            //Arrange
            var addressIdRegex = "[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}";

            _mockProviderService.Given(string.Format("DELETE: there is address data for address id {0}", addressId))
                .UponReceiving(string.Format("a request to delete address data for address id {0}", addressId))
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Delete,
                    Path = Match.Regex($"/address/{addressId}", $"^\\/address\\/{addressIdRegex}$")
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 204
                });

            var consumer = new AddressServiceClient(_mockProviderServiceBaseUri);

            //Act
            await consumer.DeleteAddressById(addressId);

            _mockProviderService.VerifyInteractions();
        }

        [Fact]
        public async Task DeleteAddressById_WhenTheAddressIsInvalid_ReturnsHttp400()
        {
            //Arrange
            var invalidAddressId = "this_is_not_a_valid_address_id";
            _mockProviderService.Given(string.Format("DELETE: the address id {0} is invalid", invalidAddressId))
                .UponReceiving(string.Format("a request to delete address data for address id {0}", invalidAddressId))
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Delete,
                    Path = Match.Type($"/address/{invalidAddressId}")
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 400
                });

            var consumer = new AddressServiceClient(_mockProviderServiceBaseUri);

            //Act //Assert
            await Assert.ThrowsAnyAsync<Exception>(() => consumer.DeleteAddressById(invalidAddressId));

            _mockProviderService.VerifyInteractions();
        }
    }
}
