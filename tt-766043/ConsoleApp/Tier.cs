using System.ComponentModel;

namespace ConsoleApp;

/// <summary>
/// Represents the different tiers or stages of a system, application, or service.
/// </summary>

/// <remarks>
/// Author: Kutay Kasapoglu
/// </remarks>
/// 
/// <summary>
/// The Description attributes are added to provide a better readable description while using the values in template.
/// Each enum value has a Description attribute that specifies the corresponding Azure Storage Account tier name.
/// </summary>
public enum Tier
{
    /// <summary>
    /// Represents the development tier or stage.
    /// </summary>
    [Description("Standard_LRS")]
    Development = 0,

    /// <summary>
    /// Represents the testing tier or stage.
    /// </summary>
    [Description("Standard_ZRS")]
    Test = 1,

    /// <summary>
    /// Represents the production tier or stage.
    /// </summary>
    [Description("Premium_LRS")]
    Production = 2
}