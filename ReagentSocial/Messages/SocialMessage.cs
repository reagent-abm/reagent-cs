namespace Reagent.Social.Messages;

using Reagent.Messages;

/// <summary>
/// A <c>SocialMessage</c> is a message that is sent between <c>Agent</c>s through the social network.
/// </summary>
public class SocialMessage : IMessage
{
    /// <summary>
    /// The <c>Guid</c> of the <c>SocialMessage</c>.
    /// </summary>
    public Guid Guid { get; }

    /// <summary>
    /// The <c>Guid</c> of the <c>Agent</c> that the message is sent to.
    /// </summary>
    public Guid Destination { get; }

    /// <summary>
    /// The <c>Guid</c> of the <c>Agent</c> that sent the message.
    /// </summary>
    public Guid Sender { get; }

    /// <summary>
    /// The payload of the message.
    /// </summary>
    public IMessage Payload { get; }

    /// <summary>
    /// The weight of the message.
    /// </summary>
    public double Weight { get; }

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
    public SocialMessage(Guid guid, Guid destination, Guid sender, IMessage payload, double weight = 1.0)
    {
        Guid = guid;
        Destination = destination;
        Sender = sender;
        Payload = payload;
        Weight = weight;
    }
}