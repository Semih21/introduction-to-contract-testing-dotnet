﻿using OrderConsumer.Models;
using PactNet.Matchers;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Xunit;

namespace OrderConsumer.Tests
{
    public class AddressApiConsumerTests : IClassFixture<OrderConsumerAddressApiPact>
    {
        private readonly IMockProviderService _mockProviderService;
        private readonly string _mockProviderServiceBaseUri;

        private readonly string addressId = "8aed8fad-d554-4af8-abf5-a65830b49a5f";
        private readonly string addressType = "billing";
        private readonly string street = "Main Street";
        private readonly int number = 123;
        private readonly string city = "Nothingville";
        private readonly int zipCode = 54321;
        private readonly string state = "Tennessee";
        private readonly string country = "United States";

        public AddressApiConsumerTests(OrderConsumerAddressApiPact data)
        {
            _mockProviderService = data.MockProviderService;
            _mockProviderServiceBaseUri = data.MockProviderServiceBaseUri;
            _mockProviderService.ClearInteractions();
        }

        [Fact]
        public async Task GetAddressById_WhenTheAddressExists_ReturnsAddress()
        {
            //Arrange
            string addressIdRegex = "[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}";
            
            _mockProviderService.Given($"GET: there is address data for address id {addressId}")
                .UponReceiving($"a request to retrieve address data for address id {addressId}")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = Match.Regex($"/address/{addressId}", $"^\\/address\\/{addressIdRegex}$"),
                    Headers = new Dictionary<string, object>
                    {
                        { "Accept", "application/json" }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new
                    {
                        id = Match.Type(addressId),
                        addressType = Match.Type(addressType),
                        street = Match.Type(street),
                        number = Match.Type(number),
                        city = Match.Type(city),
                        zipCode = Match.Type(zipCode),
                        state = Match.Type(state),
                        country = Match.Type(country)
                    }
                });

            AddressServiceClient consumer = new AddressServiceClient(_mockProviderServiceBaseUri);

            //Act
            Address result = await consumer.GetAddressById(addressId);

            //Assert
            Assert.Equal(addressId, result.Id.ToString());
            Assert.Equal(addressType, result.AddressType);
            Assert.Equal(street, result.Street);
            Assert.Equal(number, result.Number);
            Assert.Equal(city, result.City);
            Assert.Equal(zipCode, result.ZipCode);
            Assert.Equal(state, result.State);
            Assert.Equal(country, result.Country);

            _mockProviderService.VerifyInteractions();
        }

        [Fact]
        public async Task GetAddressById_WhenTheAddressDoesNotExist_ReturnsHttp404()
        {
            //Arrange
            string unknownAddressId = "00000000-0000-0000-0000-000000000000";

            _mockProviderService.Given($"GET: there is no address data for address id {unknownAddressId}")
                .UponReceiving($"a request to retrieve address data for address id {unknownAddressId}")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = Match.Type($"/address/{unknownAddressId}"),
                    Headers = new Dictionary<string, object>
                    {
                        { "Accept", "application/json" }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 404
                });

            AddressServiceClient consumer = new AddressServiceClient(_mockProviderServiceBaseUri);

            //Act //Assert
            await Assert.ThrowsAnyAsync<Exception>(() => consumer.GetAddressById(unknownAddressId));

            _mockProviderService.VerifyInteractions();
        }

        [Fact]
        public async Task GetAddressById_WhenTheAddressIsInvalid_ReturnsHttp400()
        {
            //Arrange
            string invalidAddressId = "this_is_not_a_valid_address_id";

            _mockProviderService.Given($"GET: the address id {invalidAddressId} is invalid")
                .UponReceiving($"a request to retrieve address data for address id {invalidAddressId}")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = Match.Type($"/address/{invalidAddressId}"),
                    Headers = new Dictionary<string, object>
                    {
                        { "Accept", "application/json" }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 400
                });

            AddressServiceClient consumer = new AddressServiceClient(_mockProviderServiceBaseUri);

            //Act //Assert
            await Assert.ThrowsAnyAsync<Exception>(() => consumer.GetAddressById(invalidAddressId));

            _mockProviderService.VerifyInteractions();
        }

        [Fact]
        public async Task DeleteAddressById_WhenTheAddressExists_ReturnsHttp204()
        {
            //Arrange
            string addressIdRegex = "[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}";

            _mockProviderService.Given($"DELETE: there is address data for address id {addressId}")
                .UponReceiving($"a request to delete address data for address id {addressId}")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Delete,
                    Path = Match.Regex($"/address/{addressId}", $"^\\/address\\/{addressIdRegex}$")
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 204
                });

            AddressServiceClient consumer = new AddressServiceClient(_mockProviderServiceBaseUri);

            //Act
            await consumer.DeleteAddressById(addressId);

            _mockProviderService.VerifyInteractions();
        }

        [Fact]
        public async Task DeleteAddressById_WhenTheAddressIsInvalid_ReturnsHttp400()
        {
            //Arrange
            string invalidAddressId = "this_is_not_a_valid_address_id";

            _mockProviderService.Given($"DELETE: the address id {invalidAddressId} is invalid")
                .UponReceiving($"a request to delete address data for address id {invalidAddressId}")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Delete,
                    Path = Match.Type($"/address/{invalidAddressId}")
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 400
                });

            AddressServiceClient consumer = new AddressServiceClient(_mockProviderServiceBaseUri);

            //Act //Assert
            await Assert.ThrowsAnyAsync<Exception>(() => consumer.DeleteAddressById(invalidAddressId));

            _mockProviderService.VerifyInteractions();
        }
    }
}
