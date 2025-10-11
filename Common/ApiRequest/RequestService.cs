using lexicana.Common.ApiRequest.models;

namespace lexicana.Common.ApiRequest
{
    public class RequestService
    {
        private readonly HttpClient _httpClient;
        public RequestService(HttpClient client)
        {
            _httpClient = client;
        }
        
        public async Task<RequestResponse> Post(string path,MultipartFormDataContent formDataContent)
        {
            var response = await _httpClient.PostAsync(path,formDataContent);

            return new RequestResponse(response);
        }
        
        public async Task<RequestResponse> Post(string path)
        {
            return await Post<EmptyResult>(path,default);
        }
        
        public async Task<RequestResponse> Post<T>(string path,T value)
        {
            var response =await _httpClient.PostAsJsonAsync(path,value);

            return new RequestResponse(response);
        }

        public async Task<RequestResponse> Get(string uri)
        {
            var response = await _httpClient.GetAsync(uri.TrimStart('/'));
            return new RequestResponse(response);
        }
        
        public async Task<RequestResponse> Get(Uri uri)
        {
            var response = await _httpClient.GetAsync(uri);
            return new RequestResponse(response);
        }
    } 
}