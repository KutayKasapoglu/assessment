﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 17.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace ConsoleApp
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using ConsoleApp.Utilities;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class CloudStorageArmTemplate : RuntimeTextTemplate
    {
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
/* 
Author: Kutay Kasapoglu
The "ConsoleApp.Utilities" namespace is imported to provide access to utility classes and methods that are used within this template. 
Some of the processes have been transferred to this helper class in order to keep template file clean and focused on its main purpose.
    - TemplateHelper.GetEnumDescription()
    - TemplateHelper.GenerateBlobContainersJson()
    - EnumHelper.GetEnumDescription()
*/
            this.Write("\r\n");
            this.Write("{\r\n    \"$schema\": \"https://schema.management.azure.com/schemas/2019-04-01/deploym" +
                    "entTemplate.json#\",\r\n    \"contentVersion\": \"1.0.0.0\",\r\n    \"parameters\": { },\r\n " +
                    "   \"variables\": {\r\n        \"storageAccountName\": \"");
            this.Write(this.ToStringHelper.ToStringWithCulture(StorageAccountName));
            this.Write("\",\r\n        \"tierName\": \"");
            this.Write(this.ToStringHelper.ToStringWithCulture(EnumHelper.GetEnumDescription(Tier)));
            this.Write("\",\r\n        \"blobContainers\": ");
            this.Write(this.ToStringHelper.ToStringWithCulture(TemplateHelper.WriteJsonArray(BlobContainers)));
            this.Write(@",
        ""location"": ""[resourceGroup().location]""
    },
    ""resources"": [
        {
            ""type"": ""Microsoft.Storage/storageAccounts"",
            ""name"": ""[variables('storageAccountName')]"",
            ""location"": ""[variables('location')]"",
            ""apiVersion"": ""2021-04-01"",
            ""sku"": {
                ""name"": ""[variables('tierName')]""
            },
            ""kind"": ""StorageV2"",
            ""properties"": {
                ""accessTier"": ""Hot""
            }
        },
        ");
            this.Write(this.ToStringHelper.ToStringWithCulture(TemplateHelper.GenerateBlobContainersJson(BlobContainers)));
            this.Write("\r\n    ],\r\n    \"outputs\": {\r\n        \"storageAccountId\": {\r\n            \"type\": \"s" +
                    "tring\",\r\n            \"value\": \"[resourceId(\'Microsoft.Storage/storageAccounts\', " +
                    "variables(\'storageAccountName\'))]\"\r\n        }\r\n    }\r\n}\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }
}