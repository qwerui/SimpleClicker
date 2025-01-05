using Microsoft.IdentityModel.Tokens;
using System.Collections.Immutable;

namespace ClickerBackend.Config
{
    public class JwksManager
    {
        public static ImmutableList<JsonWebKey> signingKey;

        public static async void UpdateJwks()
        {
            HttpClient client = new HttpClient();
            var message = await client.GetAsync("https://player-auth.services.api.unity.com/.well-known/jwks.json");
            var jwksString = await message.Content.ReadAsStringAsync();
            var jwks = new JsonWebKeySet(jwksString);
            signingKey = ImmutableList.Create([.. jwks.Keys]);
        }
    }
}
