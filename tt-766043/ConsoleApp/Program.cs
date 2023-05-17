using ConsoleApp;

/*
This task is designed to assess your backend development skills. 
Your objective is to fix code that generates an ARM template for Azure Storage. 

Currently, the code is unable to control the storage sku based on the Tier-value and 
lacks blob container provisioning. This issue requires your attention. To successfully 
complete this task, you will need to:

- Ensure the generated template includes Blob Container provisioning.
- Control the storage sku according to the Tier-value:
  - Development -> Standard_LRS
  - Test -> Standard_ZRS
  - Production -> Premium_LRS

Please provide a clear description of your approach to fixing the code and any relevant experience you have in your solution.
 */

var script = new CloudStorageArmTemplate()
{
    StorageAccountName = "store",
    BlobContainers = new[]{ "images", "music", "documents" },
    Tier = Tier.Test
};

Console.WriteLine(script);

/// <remarks>
/// Author: Kutay Kasapoglu
/// </remarks>
/// 
/// <summary>
/// The asked features were made by changing the structure in a minimal way.
/// For all changes made, an explanation is written in the relevant fields.
/// 
/// Thank you for giving me this opportunity:)
/// Kind regards!
/// </summary>
