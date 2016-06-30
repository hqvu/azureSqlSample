using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using System;

namespace AzureManagement
{
    public class Authentication
    {
        public static AuthenticationResult GetAccessToken(string domainName, string clientId, string redirectUri)
        {
            AuthenticationContext authContext = new AuthenticationContext
                ("https://login.windows.net/" + domainName /* Tenant ID or AAD domain */);

            AuthenticationResult token = authContext.AcquireToken(
                "https://management.azure.com/" /* the Azure Resource Management endpoint */,
                clientId /* client ID */,
                new Uri(redirectUri) /* redirect URI */,
                PromptBehavior.Never /* with Auto user will not be prompted if an unexpired token is cached */);

            return token;
        }

        public static TokenCredentials CreateCredentials(string accessToken)
        {
            return new TokenCredentials(accessToken);
        }
    }
}
