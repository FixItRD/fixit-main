using fixit_main.Models;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Identity.Client;


public class TokenService : ITokenService
{

    private readonly HttpClient _httpClient;
    public TokenService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<string> GenerateToken(int userID, string name, Role role)
    {
        string roleString = role.ToString().ToLower();

        try
        {
            var result = await _httpClient.PostAsJsonAsync("https://fixit-token-handler20240711122232.azurewebsites.net/api/v1/Token", new UserClaimsModel()
            {
                UserId = userID,
                Name = name,
                Role = roleString
            });

            string token = await result.Content.ReadAsStringAsync();

            return token;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return "";
        }
    }
}