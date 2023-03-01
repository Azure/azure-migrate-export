using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Identity.Client;
using System.Threading.Tasks;

namespace Azure.Migrate.Export.Authentication
{
    public static class AzureAuthenticationHandler
    {
        private static readonly List<string> scopes = new List<string>()
        {
            "https://management.azure.com/.default"
        };
        public static async Task<AuthenticationResult> CommonLogin()
        {
            return await Login();
        }

        public static async Task<AuthenticationResult> TenantLogin(string tenantID)
        {
            Program.InitializeTenantAuthentication(tenantID);
            return await Login();
        }

        public static async Task<AuthenticationResult> Login()
        {
            AuthenticationResult authResult = null;
            var accounts = await Program.PublicClientApp.GetAccountsAsync();
            var firstAccount = accounts.FirstOrDefault();

            try
            {
                authResult = await Program.PublicClientApp.AcquireTokenSilent(scopes, firstAccount)
                                                          .ExecuteAsync();
            }
            catch (MsalUiRequiredException)
            {
                // A MsalUiRequiredException happened on AcquireTokenSilent.
                // This indicates you need to call AcquireTokenInteractive to acquire a token
                try
                {
                    authResult = await Program.PublicClientApp.AcquireTokenInteractive(scopes)
                                                              .ExecuteAsync();
                }
                catch (MsalException exInteractiveLogin)
                {
                    throw new Exception($"Error during interactive login: {exInteractiveLogin.Message}");
                }
            }
            catch (Exception exLogin)
            {
                throw new Exception($"Error during login: {exLogin.Message}");
            }
#if DEBUG
            Console.WriteLine(authResult.AccessToken);
            Console.WriteLine(authResult.Account.Username);
#endif
            return authResult;
        }

        public static async Task Logout()
        {
            var accounts = (await Program.PublicClientApp.GetAccountsAsync()).ToList();
            while (accounts.Any())
            {
                try
                {
                    await Program.PublicClientApp.RemoveAsync(accounts.First());
                    accounts = (await Program.PublicClientApp.GetAccountsAsync()).ToList();
                }
                catch (MsalException ex)
                {
                    throw new Exception($"Error during user logout: {ex.Message}");
                }
            }
        }

        public static async Task<AuthenticationResult> RetrieveAuthenticationToken()
        {
            AuthenticationResult authResult = null;
            var accounts = await Program.PublicClientApp.GetAccountsAsync();
            var firstAccount = accounts.FirstOrDefault();

            try
            {
                // AcquireTokenSilent - Retrieves token from the encrypted cache. It auto-refreshes token based on AuthenticationResult.ExpiresOn
                authResult = await Program.PublicClientApp.AcquireTokenSilent(scopes, firstAccount)
                                                          .ExecuteAsync();
            }
            catch (Exception exRetrieveAuthenticationToken)
            {
                throw new Exception($"Error during cached token retrieval: {exRetrieveAuthenticationToken.Message}");
            }
            return authResult;
        }
    }
}