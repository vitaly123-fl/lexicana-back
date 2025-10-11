namespace lexicana.Common.ApiRequest
{
    public class RequestResponse
    {
        private readonly HttpResponseMessage _responseMessage;
        
        public RequestResponse(HttpResponseMessage responseMessage)
        {
            _responseMessage = responseMessage;
        }
        
        public async Task<string> GetString()
        {
            if (_responseMessage.IsSuccessStatusCode)
                return await _responseMessage.Content.ReadAsStringAsync();

            return String.Empty;
        }
       
        public async Task<T> GetObject<T>()
        {
            var temp=await _responseMessage.Content.ReadAsStringAsync();
            if (_responseMessage.IsSuccessStatusCode)
                return await _responseMessage.Content.ReadFromJsonAsync<T>();

            return default;
        }
    }
}