﻿using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace $safeprojectname$
{
    public class ApiClient
    {
        private readonly Config _config;

        public ApiClient(Config config)
        {
            _config = config;
        }
        private HttpClient GetHttpClient(string url)
        {
            var client = new HttpClient { BaseAddress = new Uri(url) };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
        private async Task<HttpResponseMessage> Fetch(HttpClient client, ObjRequest request)
        {
            switch (request.Type)
            {
                case ERequestType.Get:
                    var url = $"{_config.Url}/{request.Method}{_config.Permissions}{request.Parameters}";
                    return await client.GetAsync(url);
                case ERequestType.Post:
                    var objPost = new StringContent(JsonConvert.SerializeObject(request.Parameters));
                    return await client.PostAsync($"{_config.Url}/{request.Method}{_config.Permissions}", objPost);
                case ERequestType.Put:
                    var objPut = new StringContent(JsonConvert.SerializeObject(request.Parameters));
                    return await client.PutAsync($"{_config.Url}/{request.Method}{_config.Permissions}", objPut);
                default:
                    throw new Exception("Invalid Request type");
            }
        }
        public async Task<T> SendAsync<T>(ObjRequest request)
        {
            try
            {
                using (var client = GetHttpClient(_config.Url))
                {
                    var response = await Fetch(client, request);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<T>(json);

                        return result;
                    }

                    return default(T);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("System could not connect to the API");
            }
        }
    }
}