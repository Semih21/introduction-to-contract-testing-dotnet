using PactNet;
using PactNet.Mocks.MockHttpService;
using PactNet.Models;

namespace OrderConsumer.Tests
{
    public class OrderConsumerAddressApiPact : IDisposable
    {
        public IPactBuilder PactBuilder { get; }
        public IMockProviderService MockProviderService { get; }

        public int MockServerPort => 9876;
        public string MockProviderServiceBaseUri => $"http://localhost:{MockServerPort}";

        public OrderConsumerAddressApiPact()
        {
            PactBuilder = new PactBuilder(new PactConfig
            {
                SpecificationVersion = "2.0.0",
                LogDir = $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}logs{Path.DirectorySeparatorChar}",
                PactDir = $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}pacts{Path.DirectorySeparatorChar}"
            })
                .ServiceConsumer("order_consumer_dotnet")
                .HasPactWith("address_provider_dotnet");

            MockProviderService = PactBuilder.MockService(MockServerPort, false, IPAddress.Any);
        }

        public void Dispose()
        {
            PactBuilder.Build();
        }
    }
}
