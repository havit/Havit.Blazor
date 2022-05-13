// In a class library there must be present ResourceLocationAttribute otherwise developer can change where resources should be located.
// To understand this well see comment in ResourceManagerStringLocalizerFactory.GetResourcePath (https://github.com/aspnet/Localization/blob/c283dfb56cd9620edfae644f1229d8231e9cfc0e/src/Microsoft.Extensions.Localization/ResourceManagerStringLocalizerFactory.cs#L249).
// We would like to share resources near classes, unfortunatelly we cannot use ResourceLocationAttribute with an empty string (https://github.com/aspnet/Localization/blob/43b974482c7b703c92085c6f68b3b23d8fe32720/src/Microsoft.Extensions.Localization/ResourceLocationAttribute.cs#L22).
// Thus we are forced to use some namespace.
[assembly: Microsoft.Extensions.Localization.ResourceLocation("Resources")]
