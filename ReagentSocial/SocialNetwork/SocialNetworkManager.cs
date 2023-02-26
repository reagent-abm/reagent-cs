namespace Reagent.Social.SocialNetwork;

/// <summary>
/// The <c>SocialNetworkManager</c> is a static class that manages the social network.
///
/// The <c>SocialNetwork</c> Should be set to the social network in use.
/// </summary>
public static class SocialNetworkManager
{
    /// <summary>
    /// The social network instance.
    /// </summary>
    public static ISocialNetwork? SocialNetwork { get; set; }
}