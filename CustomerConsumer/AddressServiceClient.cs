using CustomerConsumer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace CustomerConsumer
{
    public class AddressServiceClient : IDisposable
    {
        private readonly HttpClient _httpClient;

        public AddressServiceClient(string baseUri)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUri ?? "http://my.api/v2/address") };
        }

        private readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        public async Task<Address> GetAddressById(string addressId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, string.Format("/address/{0}", addressId));
            request.Headers.Add("Accept", "application/json");

            var response = await _httpClient.SendAsync(request);

            try
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<Address>(await response.Content.ReadAsStringAsync(), _jsonSettings);
                }

                await RaiseResponseError(request, response);
            }
            finally
            {
                Dispose(request, response);
            }

            return null;
        }

        public async Task DeleteAddressById(string addressId)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, string.Format("/address/{0}", addressId));

            var response = await _httpClient.SendAsync(request);

            try
            {
                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    await RaiseResponseError(request, response);
                }               
            }
            finally
            {
                Dispose(request, response);
            }
        }

        private static async Task RaiseResponseError(HttpRequestMessage failedRequest, HttpResponseMessage failedResponse)
        {
            throw new HttpRequestException(
                string.Format("The Address API request for {0} {1} failed. Response Status: {2}, Response Body: {3}",
                failedRequest.Method.ToString().ToUpperInvariant(),
                failedRequest.RequestUri,
                (int)failedResponse.StatusCode,
                await failedResponse.Content.ReadAsStringAsync()));
        }

        public void Dispose()
        {
            Dispose(_httpClient);
        }

        public void Dispose(params IDisposable[] disposables)
        {
            foreach (var disposable in disposables.Where(d => d != null))
            {
                disposable.Dispose();
            }
        }
    }
}
