namespace Reagent.Social.SocialNetwork;

/// <summary>
/// Something that has a <c>SocialNetwork</c>.
/// </summary>
public interface IHasSocialNetwork
{
    /// <summary>
    /// The <c>SocialNetwork</c>.
    /// </summary>
    public ISocialNetwork SocialNetwork { get; }
}