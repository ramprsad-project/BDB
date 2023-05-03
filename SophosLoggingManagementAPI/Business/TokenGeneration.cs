using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using SophosLoggingManagementAPI.Data;
using SophosLoggingManagementAPI.Models;
using SophosLoggingManagementAPI.Dto;
using System.Linq;

namespace SophosLoggingManagementAPI.Business
{
    public static class TokenGeneration
    {

        /// The client information used to get the OAuth Access Token from the server.
        static string clientId = "6dffa7b0-780b-4363-bdb8-18e37c734651";
        static string clientSecret = "baf44b13673f5d10c367291aa6d0375109713c5fc4e8ff660125b65b256518d5296f2ea4dafa332b5f836c34caa27c000db5";

        // The server base address
        static string baseUrl = "https://id.sophos.com/api/v2/oauth2/token";

        // this will hold the Access Token returned from the server.
        static string accessToken = null;

        /// <summary>
        /// This method uses the OAuth Client Credentials Flow to get an Access Token to provide
        /// Authorization to the APIs.
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetToken()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);

                // We want the response to be JSON.
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");

                // Build up the data to POST.
                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                postData.Add(new KeyValuePair<string, string>("client_id", clientId));
                postData.Add(new KeyValuePair<string, string>("client_secret", clientSecret));
                postData.Add(new KeyValuePair<string, string>("scope", "token"));

                FormUrlEncodedContent content = new FormUrlEncodedContent(postData);

                // Post to the Server and parse the response.
                HttpResponseMessage response = await client.PostAsync(baseUrl, content);
                string jsonString = await response.Content.ReadAsStringAsync();
                object responseData = JsonConvert.DeserializeObject(jsonString);

                // return the Access Token.
                return ((dynamic)responseData).access_token;
            }
        }

        /// <summary>
        /// GetPartnerID
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetPartnerID()
        {
            // Get the Access Token.
            string uri = "https://api.central.sophos.com/whoami/v1";
            accessToken = (accessToken == null) ? await GetToken() : accessToken;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Add the Authorization header with the AccessToken.
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

                // make the request
                HttpResponseMessage response = await client.GetAsync(uri);

                // parse the response and return the data.
                string jsonString = await response.Content.ReadAsStringAsync();
                object responseData = JsonConvert.DeserializeObject(jsonString);
                return ((dynamic)responseData).id;
            }
        }
        
        /// <summary>
        /// GetTenants
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetTenants()
        {
            // Get the Access Token.
            string url = "https://api.central.sophos.com/partner/v1/tenants/";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Add the Authorization header with the AccessToken.
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                client.DefaultRequestHeaders.Add("X-Partner-ID", await GetPartnerID());
                // make the request
                HttpResponseMessage response = await client.GetAsync(url);

                return response.Content.ReadAsStringAsync().Result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetTenantsAPI()
        {
            // Get the Access Token.
            string url = "https://api-us01.central.sophos.com/endpoint/v1/endpoints";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                // Add the Authorization header with the AccessToken.
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                client.DefaultRequestHeaders.Add("Accept", "*/*");
                client.DefaultRequestHeaders.Add("X-Tenant-ID", await ((dynamic)GetTenants()).items.id);
                // make the request
                HttpResponseMessage response = await client.GetAsync(url);
                return await response.Content.ReadAsStringAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetTenantEvents()
        {
            // Get the Access Token.
            string url = "https://api-us01.central.sophos.com/siem/v1/events";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                // Add the Authorization header with the AccessToken.
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                //client.DefaultRequestHeaders.Add("Accept", "*/*");
                var Id =  JsonConvert.DeserializeObject<dynamic>(await GetTenants())!;
                var val = ((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)Id).First()).First).First).First;
                client.DefaultRequestHeaders.Add("X-Tenant-ID", (string?)((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JContainer)(dynamic)val).First).Value);
                // make the request
                HttpResponseMessage response = await client.GetAsync(url);
                return await response.Content.ReadAsStringAsync();
            }
        }

        public static T Deserialize<T>(string json)
        {
            Newtonsoft.Json.JsonSerializer s = new JsonSerializer();
            return s.Deserialize<T>(new JsonTextReader(new StringReader(json)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task<string> SaveSystemEventsToDB()
        {
            string events = await GetTenantEvents();
            int status =0;
            Root root = Deserialize<Root>(events);
            for (int i= 0; i < root.items.Count - 1; i++)
            {
                 status = SaveSystemEvents.SavesystemEventsToDB(root.items[i].user_id, root.items[i].when, root.items[i].created_at, root.items[i].type, root.items[i].severity, root.items[i].location, root.items[i].name, root.items[i].severity, root.items[i].severity, root.items[i].source_info.ip, root.items[i].source, root.items[i].endpoint_type, root.items[i].id, null, null, null, null, null, null, root.items[i].type, null, root.items[i].endpoint_id);
            }

            return (status == 1) ? "success" : "failed";
        }
    }
}
