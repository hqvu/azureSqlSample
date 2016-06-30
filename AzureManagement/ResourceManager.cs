using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;

namespace AzureManagement
{
    public class ResourceManager
    {
        private string _subscriptionId;
        private string _accessToken;

        public ResourceManager(string subscriptionId, string accessToken)
        {
            this._subscriptionId = subscriptionId;
            this._accessToken = accessToken;
        }

        public ResourceGroup CreateResourceGroup(
            string location,
            string resourceGroupName)
        {
            var creds = Authentication.CreateCredentials(this._accessToken);

            // Create a resource management client
            ResourceManagementClient resourceClient = new ResourceManagementClient(creds);

            // Resource group parameters
            ResourceGroup resourceGroupParameters = new ResourceGroup()
            {
                Location = location
            };

            //Create a resource group
            resourceClient.SubscriptionId = this._subscriptionId;
            var resourceGroupResponse = resourceClient.ResourceGroups.CreateOrUpdate(
                resourceGroupName, resourceGroupParameters);

            return resourceGroupResponse;
        }
    }
}
