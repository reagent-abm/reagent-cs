using Microsoft.Extensions.Logging;
using Reagent.Messages;
using Reagent.Properties;

namespace Reagent.Social.Messages;

/// <summary>
/// The specification of a <c>SocialMessage</c>.
/// </summary>
public class SocialMessageSpecification : IGuidD
{
    /// <summary>
    /// The <c>Guid</c> of the <c>SocialMessageSpecification</c>.
    /// </summary>
    public virtual Guid Guid { get; }

    /// <summary>
    /// The <c>Guid</c> of the <c>Agent</c> that sent the message.
    /// </summary>
    public virtual Guid Sender { get; }

    /// <summary>
    /// The payload of the message.
    /// </summary>
    public virtual IMessage Payload { get; }

    /// <summary>
    /// The logger.
    /// </summary>
    protected virtual ILogger<SocialMessageSpecification> Logger { get; }

    /// <summary>
    /// Create a new <c>SocialMessageSpecification</c>.
    /// </summary>
    /// <param name="guid">
    /// The <c>Guid</c> of the <c>SocialMessageSpecification</c>.
    /// </param>
    /// <param name="sender">
    /// The <c>Guid</c> of the <c>Agent</c> that sent the message.
    /// </param>
    /// <param name="payload">
    /// The payload of the message.
    /// </param>
    /// <param name="logger">
    /// The logger.
    /// </param>
    public SocialMessageSpecification(Guid guid, Guid sender, IMessage payload,
        ILogger<SocialMessageSpecification> logger)
    {
        Guid = guid;
        Sender = sender;
        Payload = payload;
        Logger = logger;
    }

    /// <summary>
    /// Create a new <c>SocialMessageSpecification</c> with a random <c>Guid</c>.
    /// </summary>
    /// <param name="sender">
    /// The <c>Guid</c> of the <c>Agent</c> that sent the message.
    /// </param>
    /// <param name="payload">
    /// The payload of the message.
    /// </param>
    /// <param name="logger">
    /// The logger.
    /// </param>
    public SocialMessageSpecification(Guid sender, IMessage payload, ILogger<SocialMessageSpecification> logger)
        : this(Guid.NewGuid(), sender, payload, logger)
    {
    }

    /// <summary>
    /// Converts this <c>SocialMessageSpecification</c> to a <c>SocialMessage</c> with the given <c>Guid</c>s as destinations.
    /// </summary>
    /// <param name="destinations">
    /// The <c>Guid</c>s of the <c>Agent</c>s that the message is sent to.
    /// </param>
    /// <returns>
    /// The <c>SocialMessage</c>s.
    /// </returns>
    public virtual IList<SocialMessage> ToSocialMessagesFromGuids(IList<Guid> destinations)
    {
        Logger.LogTrace(
            "Converting SocialMessageSpecification {SocialMessageSpecification} from Sender {Sender} to {Destinations} destinations",
            this, Sender, destinations.Count);
        var result = from destination in destinations
            select new SocialMessage(destination, Sender, Payload);
        return result.ToList();
    }

    /// <summary>
    /// Converts this <c>SocialMessageSpecification</c> to a <c>SocialMessage</c> with the given <c>Agent</c>s as destinations.
    /// </summary>
    /// <param name="agents">
    /// The <c>Agent</c>s that the message is sent to.
    /// </param>
    /// <returns>
    /// The <c>SocialMessage</c>s.
    /// </returns>
    public virtual IList<SocialMessage> ToSocialMessagesFromAgents(IList<Agent.Agent> agents)
    {
        Logger.LogTrace(
            "Converting SocialMessageSpecification {SocialMessageSpecification} from Sender {Sender} to {Agents} agents",
            this, Sender, agents.Count);
        var result = from agent in agents
            select new SocialMessage(agent.Guid, Sender, Payload);
        return result.ToList();
    }

    /// <summary>
    /// Converts this <c>SocialMessageSpecification</c> to a <c>SocialMessage</c> with the given <c>Guid</c>s as destinations and weights.
    /// </summary>
    /// <param name="destinationsAndWeights">
    /// The <c>Guid</c>s of the <c>Agent</c>s that the message is sent to and their weights.    
    /// </param>
    /// <returns>
    /// The <c>SocialMessage</c>s.
    /// </returns>
    public virtual IList<SocialMessage> ToSocialMessagesFromGuidsAndWeights(
        IList<Tuple<Guid, double>> destinationsAndWeights)
    {
        Logger.LogTrace(
            "Converting SocialMessageSpecification {SocialMessageSpecification} from Sender {Sender} to {DestinationsAndWeights} destinations and weights",
            this, Sender, destinationsAndWeights.Count);
        var result = from destinationAndWeight in destinationsAndWeights
            select new SocialMessage(destinationAndWeight.Item1, Sender, Payload, destinationAndWeight.Item2);
        return result.ToList();
    }
    
    /// <summary>
    /// Converts this <c>SocialMessageSpecification</c> to a <c>SocialMessage</c> with the given <c>Agent</c>s as destinations and weights.
    /// </summary>
    /// <param name="agentsAndWeights">
    /// The <c>Agent</c>s that the message is sent to and their weights.
    /// </param>
    /// <returns>
    /// The <c>SocialMessage</c>s.
    /// </returns>
    public virtual IList<SocialMessage> ToSocialMessagesFromAgentsAndWeights(
        IList<Tuple<Agent.Agent, double>> agentsAndWeights)
    {
        Logger.LogTrace(
            "Converting SocialMessageSpecification {SocialMessageSpecification} from Sender {Sender} to {AgentsAndWeights} agents and weights",
            this, Sender, agentsAndWeights.Count);
        var result = from agentAndWeight in agentsAndWeights
            select new SocialMessage(agentAndWeight.Item1.Guid, Sender, Payload, agentAndWeight.Item2);
        return result.ToList();
    }

    /// <summary>
    /// Gets a string representation of this <c>SocialMessageSpecification</c>.
    /// </summary>
    /// <returns>
    /// The string representation.
    /// </returns>
    public override string ToString()
    {
        return $"SocialMessageSpecification(Guid={Guid}, Sender={Sender}, Payload={Payload})";
    }
}