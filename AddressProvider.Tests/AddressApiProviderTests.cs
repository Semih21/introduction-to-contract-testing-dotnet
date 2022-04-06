using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PactNet;
using PactNet.Infrastructure.Outputters;
using Xunit;
using Xunit.Abstractions;

namespace AddressProvider.Tests
{
    public class AddressApiProviderTests
    {
        private readonly ITestOutputHelper _output;

        private string _providerUri { get; }
        private string _pactServiceUri { get; }
        private IWebHost _webHost { get; }

        public AddressApiProviderTests(ITestOutputHelper output)
        {
            _output = output;
            _providerUri = "http://localhost:9876";
            _pactServiceUri = "http://localhost:9001";

            _webHost = WebHost.CreateDefaultBuilder()
                .UseUrls(_pactServiceUri)
                .UseStartup<TestStartup>()
                .Build();

            _webHost.Start();
        }

        [Fact]
        public void VerifyAddressApiHonoursExpectationsInAllContracts()
        {
            var config = new PactVerifierConfig
            {
                Outputters = new List<IOutput>
                {
                    new XUnitOutput(_output)
                }
            };

            //Act / Assert
            IPactVerifier pactVerifierForCustomer = new PactVerifier(config);

            pactVerifierForCustomer
                .ProviderState($"{_pactServiceUri}/provider-states")
                .ServiceProvider("address_provider_dotnet", _providerUri)
                .HonoursPactWith("customer_consumer_dotnet")
                .PactUri($"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}pacts{Path.DirectorySeparatorChar}customer_consumer_dotnet-address_provider_dotnet.json")
                .Verify();

            IPactVerifier pactVerifierForOrder = new PactVerifier(config);

            pactVerifierForOrder
                .ProviderState($"{_pactServiceUri}/provider-states")
                .ServiceProvider("address_provider_dotnet", _providerUri)
                .HonoursPactWith("order_consumer_dotnet")
                .PactUri($"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}pacts{Path.DirectorySeparatorChar}order_consumer_dotnet-address_provider_dotnet.json")
                .Verify();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _webHost.StopAsync().GetAwaiter().GetResult();
                    _webHost.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
