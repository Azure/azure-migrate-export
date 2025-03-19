using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Azure.Migrate.Export.Tests
{
    [TestClass]
    public class ProgramTests
    {
        private readonly IntPtr _mockHandle;
        private readonly string _powershellClientId = "1950a258-227b-4e31-a9cf-717495945fc2";

        public ProgramTests()
        {
            _mockHandle = new IntPtr(76);
        }

        [TestMethod]
        public void InitializeCommonAuthentication_ShouldCreateClientAppWithCorrectSettings()
        {
            // Arrange
            Program.InitializeCommonAuthentication();

            // Act
            var clientApp = Program.PublicClientApp;

            // Assert
            Assert.IsNotNull(clientApp);
            Assert.AreEqual(this._powershellClientId, clientApp.AppConfig.ClientId);
        }

        [TestMethod]
        public void InitializeTenantAuthentication_ShouldCreateClientAppWithCorrectSettings()
        {
            // Arrange
            var tenantId = new Guid().ToString();
            Program.InitializeTenantAuthentication(tenantId);

            // Act
            var clientApp = Program.PublicClientApp;

            // Assert
            Assert.IsNotNull(clientApp);
            Assert.AreEqual(this._powershellClientId, clientApp.AppConfig.ClientId);
        }
    }
}
