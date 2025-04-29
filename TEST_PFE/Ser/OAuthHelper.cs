//using Newtonsoft.Json;

//namespace TEST_PFE.Ser
//{
//    public class OAuthHelper
//    {
//        public static async Task<string> GetAccessTokenAsync()
//        {
//            var tenantId = "common"; // Utilise "common" si tu n'as pas ton tenant ID
//            var clientId = "51f81489-12ee-4a9e-aaae-a2591f45987d"; // Public client ID par défaut (XrmToolBox, etc.)
//            var resource = "https://org97d7668c.api.crm4.dynamics.com/";
//            var username = "myriam.hammi@esprit.tn";
//            var password = "223JFT0927m";

//            var client = new HttpClient();
//            var tokenUrl = $"https://login.microsoftonline.com/{tenantId}/oauth2/token";

//            var content = new FormUrlEncodedContent(new Dictionary<string, string>
//        {
//            {"grant_type", "password"},
//            {"client_id", clientId},
//            {"resource", resource},
//            {"username", username},
//            {"password", password}
//        });

//            var response = await client.PostAsync(tokenUrl, content);
//            var result = await response.Content.ReadAsStringAsync();

//            if (!response.IsSuccessStatusCode)
//                throw new Exception($"OAuth error: {response.StatusCode} - {result}");

//            dynamic json = JsonConvert.DeserializeObject(result);
//            return json.access_token;
//        }
//    }
//}
//using Microsoft.Identity.Client;
//namespace TEST_PFE.Ser
//{
//    public class OAuthHelper
//    {
//        public static async Task<string> GetAccessTokenAsync()
//        {
//            var clientId = "51f81489-12ee-4a9e-aaae-a2591f45987d";
//            var authority = "https://login.microsoftonline.com/common";
//            var scopes = new[] { "https://org97d7668c.api.crm4.dynamics.com/.default" };

//            var app = PublicClientApplicationBuilder
//                .Create(clientId)
//                .WithAuthority(authority)
//                .WithRedirectUri("http://localhost")
//                .Build();

//            var result = await app.AcquireTokenInteractive(scopes)
//                                  .ExecuteAsync();

//            return result.AccessToken;
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TEST_PFE.Ser
{
    public class OAuthHelper
    {
        public static async Task<string> GetAccessTokenAsync()
        {
            var tenantId = "common"; // Utilise "common" si tu n'as pas un tenant spécifique
            var clientId = "51f81489-12ee-4a9e-aaae-a2591f45987d"; // Client ID
            var resource = "https://org97d7668c.api.crm4.dynamics.com/"; // Ton service Dynamics 365
            var username = "myriam.hammi@esprit.tn";  // Ton nom d'utilisateur
            var password = "223JFT0927m";  // Ton mot de passe

            var client = new HttpClient();
            var tokenUrl = $"https://login.microsoftonline.com/{tenantId}/oauth2/token";

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"grant_type", "password"},
                {"client_id", clientId},
                {"resource", resource},
                {"username", username},
                {"password", password}
            });

            var response = await client.PostAsync(tokenUrl, content);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"OAuth error: {response.StatusCode} - {result}");
            }

            dynamic json = JsonConvert.DeserializeObject(result);
            return json.access_token;  // Récupère l'access_token
        }
    }
}

