using System;
using System.Threading;
using System.Windows.Forms;

using Azure.Migrate.Export.Authentication;
using Azure.Migrate.Export.Forms;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Broker;

namespace Azure.Migrate.Export
{
    public static class Program
    {
        public static string PowerShellClientId = "1950a258-227b-4e31-a9cf-717495945fc2";
        public static string CommonAuthorityEndpoint = "https://login.microsoftonline.com/common/oauth2/authorize";
        public static string TenantAuthorityEndpoint = "https://login.microsoftonline.com/_tenantID/oauth2/authorize";
        public static IPublicClientApplication clientApp;
        private static BrokerOptions brokerOptions;
        private static IntPtr mainFormHandle;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string appGuid = Application.StartupPath.Replace('\\', '_').Replace(':', '_');
            string mutexId = $"Global\\{appGuid}";

            SetBrokerOptions();

            using (Mutex mutex = new Mutex(false, mutexId))
            {
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show("Another instance of the application is already running.", "Azure Migrate Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Load the form before authentication.
                var mainForm = new AzureMigrateExportMainForm();
                mainForm.Show();
                SetMainFormHandle(mainForm.Handle);

                // Begin authentication using MSAL after the form is loaded.
                mainForm.BeginAzureAuthentication();
                Application.Run(mainForm);
            }
        }

        public static IPublicClientApplication PublicClientApp { get { return clientApp; } }

        public static void InitializeCommonAuthentication()
        {
            clientApp = PublicClientApplicationBuilder.Create(PowerShellClientId)
                                                      .WithAuthority(new Uri(CommonAuthorityEndpoint))
                                                      .WithParentActivityOrWindow(GetMainFormHandle)
                                                      .WithBroker(brokerOptions)
                                                      .Build();
            TokenCacheHelper.EnableSerialization(clientApp.UserTokenCache);
        }

        public static void InitializeTenantAuthentication(string tenantID)
        {


            string finalAuthorityEndpoint = TenantAuthorityEndpoint.Replace("_tenantID", tenantID);
            clientApp = PublicClientApplicationBuilder.Create(PowerShellClientId)
                                                      .WithAuthority(new Uri(finalAuthorityEndpoint))
                                                      .WithParentActivityOrWindow(GetMainFormHandle)
                                                      .WithBroker(brokerOptions)
                                                      .Build();
            TokenCacheHelper.EnableSerialization(clientApp.UserTokenCache);
        }

        /// <summary>
        /// Sets the main form handle.
        /// </summary>
        /// <param name="handle"></param>
        public static void SetMainFormHandle(IntPtr handle)
        {
            mainFormHandle = handle;
        }

        /// <summary>
        /// Sets the broker options.
        /// </summary>

        public static void SetBrokerOptions()
        {
            brokerOptions = new BrokerOptions(BrokerOptions.OperatingSystems.Windows)
            {
                Title = "Azure Migrate Export",
            };
        }

        /// <summary>
        /// Gets the main forms handle.
        /// </summary>
        /// <returns>The main form HWND.</returns>
        private static IntPtr GetMainFormHandle()
        {
            return mainFormHandle;
        }
    }
}