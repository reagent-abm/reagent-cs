namespace Reagent.Social.Messages;

using Reagent.Messages;

/// <summary>
/// A <c>SocialMessage</c> is a message that is sent between <c>Agent</c>s through the social network.
/// </summary>
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class SocialMessage : IMessage
{
    /// <summary>
    /// The <c>Guid</c> of the <c>SocialMessage</c>.
    /// </summary>
    public virtual Guid Guid { get; }

    /// <summary>
    /// The <c>Guid</c> of the <c>Agent</c> that the message is sent to.
    /// </summary>
    public virtual Guid Destination { get; }

    /// <summary>
    /// The <c>Guid</c> of the <c>Agent</c> that sent the message.
    /// </summary>
    public virtual Guid Sender { get; }

    /// <summary>
    /// The payload of the message.
    /// </summary>
    // ReSharper disable once MemberCanBeProtected.Global
    public virtual IMessage Payload { get; }

    /// <summary>
    /// The weight of the message.
    /// </summary>
    // ReSharper disable once MemberCanBeProtected.Global
    public virtual double Weight { get; }

    /// <summary>
    /// Create a new <c>SocialMessage</c>.
    /// </summary>
    /// <param name="guid">
    /// The <c>Guid</c> of the <c>SocialMessage</c>.
    /// </param>
    /// <param name="destination">
    /// The <c>Guid</c> of the <c>Agent</c> that the message is sent to.
    /// </param>
    /// <param name="sender">
    /// The <c>Guid</c> of the <c>Agent</c> that sent the message.
    /// </param>
    /// <param name="payload">
    /// The payload of the message.
    /// </param>
    /// <param name="weight">
    /// The weight of the message.
    /// </param>
    // ReSharper disable once MemberCanBePrivate.Global
    public SocialMessage(Guid guid, Guid destination, Guid sender, IMessage payload, double weight = 1.0)
    {
        Guid = guid;
        Destination = destination;
        Sender = sender;
        Payload = payload;
        Weight = weight;
    }

    /// <summary>
    /// Create a new <c>SocialMessage</c> with a random <c>Guid</c>.
    /// </summary>
    /// <param name="destination">
    /// The <c>Guid</c> of the <c>Agent</c> that the message is sent to.
    /// </param>
    /// <param name="sender">
    /// The <c>Guid</c> of the <c>Agent</c> that sent the message.
    /// </param>
    /// <param name="payload">
    /// The payload of the message.
    /// </param>
    /// <param name="weight">
    /// The weight of the message.
    /// </param>
    public SocialMessage(Guid destination, Guid sender, IMessage payload, double weight = 1.0) : this(Guid.NewGuid(),
        destination, sender, payload, weight)
    {
    }

    /// <summary>
    /// Get a string representation of the <c>SocialMessage</c>.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString()
    {
        return
            $"SocialMessage(Guid={Guid}, Destination={Destination}, Sender={Sender}, Payload={Payload}, Weight={Weight})";
    }
}