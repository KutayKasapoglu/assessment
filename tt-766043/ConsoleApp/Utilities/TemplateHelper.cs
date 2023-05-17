using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace ConsoleApp.Utilities
{
    /// <remarks>
    /// Author: Kutay Kasapoglu
    /// </remarks>
    /// 
    /// <summary>
    /// In order to keep template file clean and focused on its main purpose, TemplateHelper class created.
    /// This separation promotes code reusability, maintainability, and testability.
    /// </summary>
    public static class TemplateHelper
    {
        /// <remarks>
        /// Author: Kutay Kasapoglu
        /// </remarks>
        /// 
        /// <summary>
        /// The GenerateBlobContainersJson method generates JSON representations of blob containers for an Azure ARM template.
        /// This method dynamically creates the necessary JSON structure for each container in the provided collection, 
        /// simplifying the process of defining blob containers in the ARM template.
        /// By separating the generation of blob container JSON from the main template, this method improves code modularity and reusability.
        /// </summary>
        public static string GenerateBlobContainersJson(IEnumerable<string> blobContainers)
        {
            var sb = new StringBuilder();

            foreach (var container in blobContainers)
            {
                sb.AppendLine(@"
        {
            ""type"": ""Microsoft.Storage/storageAccounts/blobServices/containers"",
            ""apiVersion"": ""2021-04-01"",
            ""name"": ""[concat(variables('storageAccountName'), '/default/" + container + @")]"",
            ""properties"": {
                ""publicAccess"": ""None""
            }
        },");
            }

            return sb.ToString().TrimEnd(',');
        }

        /// <remarks>
        /// Author: Kutay Kasapoglu
        /// </remarks>
        /// 
        /// <summary>
        /// The WriteJsonArray method aims to generate a JSON array representation from a collection of strings.
        /// This method constructs a JSON array by iterating over the input collection 
        /// and converting each string element into a JSON string format in order dosplay containers in variables of Template.
        /// </summary>
        public static string WriteJsonArray(IEnumerable<string> array)
        {
            var jsonBuilder = new StringBuilder();
            jsonBuilder.Append("[");

            if (array != null && array.Any())
            {
                jsonBuilder.Append(string.Join(", ", array.Select(item => $"\"{item}\"")));
            }

            jsonBuilder.Append("]");

            return jsonBuilder.ToString();
        }
    }
}
