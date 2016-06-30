using AzureManagement;
using Microsoft.Azure.Management.Sql.Models;
using System;

namespace TestApp
{
    class Program
    {
        static string subscriptionId = "<subscriptionId>";
        static string clientId = "<clientId>";
        static string redirectUri = "<redirectUri>";
        static string domainName = "<domainName>";

        // You create these values
        static string resourceGroupName = "<resourceGroupName>";
        static string location = "<location>";

        static string serverName = "<serverName>"; // Lower case characters and numbers only
        static string administratorLogin = "<adminLogin>";

        // Store your password securely!
        static string administratorPassword = "<adminPassword>";
        static string serverVersion = "12.0";

        static string firewallRuleName = "<firewallRuleName>";

        static string databaseName = "<dbname>";
        static string databaseEdition = "Basic"; // Basic, Standard, or Premium
        static string databasePerfLevel = "";

        static void Main(string[] args)
        {
            // ==================== Get access token =====================
            Console.WriteLine("Logging in...");
            var token = Authentication.GetAccessToken(domainName, clientId, redirectUri);
            Console.WriteLine("Logged in as: " + token.UserInfo.DisplayableId);

            // ================ Create resource group ====================
            Console.WriteLine("Creating resource group...");
            var resourceManager = new ResourceManager(subscriptionId, token.AccessToken);
            resourceManager.CreateResourceGroup(location, resourceGroupName);

            // =================== Create server =========================
            Console.WriteLine("Creating server...");
            var sqlManager = new SqlManager(subscriptionId, token.AccessToken);
            sqlManager.CreateServer(resourceGroupName, location,
                serverName, location, administratorLogin, administratorPassword);

            // ================ Create firewall rule =====================
            Console.WriteLine("Creating firewall rule...");

            // Replace with your client IP address
            string startIpAddress = "0.0.0.0";
            string endIpAddress = "255.255.255.255";

            sqlManager.CreateFirewallRule(
                resourceGroupName, serverName, firewallRuleName, startIpAddress, endIpAddress);

            // =================== Create database =======================
            Console.WriteLine("Creating database...");
            DatabaseCreateOrUpdateResponse dbResponse = sqlManager.CreateDatabase(
                resourceGroupName, serverName, databaseName, databaseEdition, databasePerfLevel);
            Console.WriteLine("Status: " + dbResponse.Status.ToString()
                + " Code: " + dbResponse.StatusCode.ToString());

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
