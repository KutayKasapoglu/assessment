
namespace ConsoleApp;

/// <summary>
/// Represents an ARM template for cloud storage configuration.
/// </summary>
public partial class CloudStorageArmTemplate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CloudStorageArmTemplate"/> class with default values.
    /// </summary>
    public CloudStorageArmTemplate()
    {
        StorageAccountName = "store";
        Tier = Tier.Test;
    }

    /// <summary>
    /// Gets the name of the storage account.
    /// </summary>
    public string StorageAccountName { get; init; }

    /// <summary>
    /// Gets the tier or stage of the target environment.
    /// </summary>
    public Tier Tier { get; set; }

    /// <summary>
    /// Gets the list of blob container names in the storage account.
    /// </summary>
    public string[] BlobContainers { get; init; } = Array.Empty<string>();

}
