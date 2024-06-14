using Microsoft.Identity.Client;
using System;
using System.Threading;
using System.Windows.Forms;

using Azure.Migrate.Export.Authentication;
using Azure.Migrate.Export.Forms;

namespace Azure.Migrate.Export
{
    static class Program
    {
        public static string PowerShellClientId = "1950a258-227b-4e31-a9cf-717495945fc2";
        public static string CommonAuthorityEndpoint = "https://login.microsoftonline.com/common/oauth2/authorize";
        public static string TenantAuthorityEndpoint = "https://login.microsoftonline.com/_tenantID/oauth2/authorize";
        public static IPublicClientApplication clientApp;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string appGuid = Application.StartupPath.Replace('\\', '_').Replace(':', '_');
            string mutexId = $"Global\\{appGuid}";
            using (Mutex mutex = new Mutex(false, mutexId))
            {
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show("Another instance of the application is already running.", "Azure Migrate Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

            InitializeCommonAuthentication();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AzureMigrateExportMainForm());
            }
        }

        public static IPublicClientApplication PublicClientApp { get { return clientApp; } }

        public static void InitializeCommonAuthentication()
        {
            clientApp = PublicClientApplicationBuilder.Create(PowerShellClientId)
                                                      .WithAuthority(new Uri(CommonAuthorityEndpoint))
                                                      .Build();
            TokenCacheHelper.EnableSerialization(clientApp.UserTokenCache);
        }

        public static void InitializeTenantAuthentication(string tenantID)
        {
            string finalAuthorityEndpoint = TenantAuthorityEndpoint.Replace("_tenantID", tenantID);
            clientApp = PublicClientApplicationBuilder.Create(PowerShellClientId)
                                                      .WithAuthority(new Uri(finalAuthorityEndpoint))
                                                      .Build();
            TokenCacheHelper.EnableSerialization(clientApp.UserTokenCache);
        }
    }
}
