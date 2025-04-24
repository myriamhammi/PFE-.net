using Newtonsoft.Json;

namespace TEST_PFE.Ser
{
    public class OAuthHelper
    {
        public static async Task<string> GetAccessTokenAsync()
        {
            var tenantId = "common"; // Utilise "common" si tu n'as pas ton tenant ID
            var clientId = "51f81489-12ee-4a9e-aaae-a2591f45987d"; // Public client ID par défaut (XrmToolBox, etc.)
            var resource = "https://org97d7668c.api.crm4.dynamics.com/";
            var username = "myriam.hammi@esprit.tn";
            var password = "223JFT0927m";

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
                throw new Exception($"OAuth error: {response.StatusCode} - {result}");

            dynamic json = JsonConvert.DeserializeObject(result);
            return json.access_token;
        }
    }
}
