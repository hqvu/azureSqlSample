using Microsoft.Azure;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.Sql;
using Microsoft.Azure.Management.Sql.Models;

namespace AzureManagement
{
    public class SqlManager
    {
        private SqlManagementClient _sqlClient;

        public SqlManager(string subscriptionId, string accessToken)
        {
            this._sqlClient = new SqlManagementClient(
                new TokenCloudCredentials(subscriptionId, accessToken));
        }

        public ServerGetResponse CreateServer(
            string resourceGroupName,
            string location,
            string serverName,
            string version,
            string adminLogin,
            string adminPassword)
        {
            var serverParameters = new ServerCreateOrUpdateParameters()
            {
                Location = location,
                Properties = new ServerCreateOrUpdateProperties()
                {
                    AdministratorLogin = adminLogin,
                    AdministratorLoginPassword = adminPassword,
                    Version = version
                }
            };

            var serverResponse = this._sqlClient.Servers.CreateOrUpdate(
                resourceGroupName, serverName, serverParameters);

            return serverResponse;
        }

        public FirewallRuleGetResponse CreateFirewallRule(
            string resourceGroupName,
            string serverName,
            string firewallRuleName,
            string startIpAddress,
            string endIpAddress)
        {
            var firewallParameters = new FirewallRuleCreateOrUpdateParameters()
            {
                Properties = new FirewallRuleCreateOrUpdateProperties()
                {
                    StartIpAddress = startIpAddress,
                    EndIpAddress = endIpAddress
                }
            };

            var firewallResponse = this._sqlClient.FirewallRules.CreateOrUpdate(
                resourceGroupName, serverName, firewallRuleName, firewallParameters);

            return firewallResponse;
        }

        public DatabaseCreateOrUpdateResponse CreateDatabase(
            string resourceGroupName,
            string serverName,
            string databaseName,
            string databaseEdition,
            string databasePerfLevel)
        {
            // Retrieve the server on which the database will be created
            Server currentServer = this._sqlClient.Servers.Get(resourceGroupName, serverName).Server;

            var databaseParameters = new DatabaseCreateOrUpdateParameters()
            {
                Location = currentServer.Location,
                Properties = new DatabaseCreateOrUpdateProperties()
                {
                    Edition = databaseEdition,
                    RequestedServiceObjectiveName = databasePerfLevel
                }
            };

            var dbResponse = this._sqlClient.Databases.CreateOrUpdate(
                resourceGroupName, serverName, databaseName, databaseParameters);

            return dbResponse;
        }
    }
}
