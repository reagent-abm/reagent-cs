using Microsoft.Extensions.Logging;
using Reagent.Messages;
using Reagent.Properties;

namespace Reagent.Social.Messages;

public class SocialMessageSpecification : IGuidD
{
    public virtual Guid Guid { get; }

    public virtual Guid Sender { get; }

    public virtual IMessage Payload { get; }

    protected virtual ILogger<SocialMessageSpecification> Logger { get; }

    public SocialMessageSpecification(Guid guid, Guid sender, IMessage payload,
        ILogger<SocialMessageSpecification> logger)
    {
        Guid = guid;
        Sender = sender;
        Payload = payload;
        Logger = logger;
    }

    public SocialMessageSpecification(Guid sender, IMessage payload, ILogger<SocialMessageSpecification> logger)
        : this(Guid.NewGuid(), sender, payload, logger)
    {
    }

    public virtual IList<SocialMessage> ToSocialMessagesFromGuids(IList<Guid> destinations)
    {
        Logger.LogTrace(
            "Converting SocialMessageSpecification {SocialMessageSpecification} from Sender {Sender} to {Destinations} destinations",
            this, Sender, destinations.Count);
        var result = from destination in destinations
            select new SocialMessage(destination, Sender, Payload);
        return result.ToList();
    }

    public virtual IList<SocialMessage> ToSocialMessagesFromAgents(IList<Agent.Agent> agents)
    {
        Logger.LogTrace(
            "Converting SocialMessageSpecification {SocialMessageSpecification} from Sender {Sender} to {Agents} agents",
            this, Sender, agents.Count);
        var result = from agent in agents
            select new SocialMessage(agent.Guid, Sender, Payload);
        return result.ToList();
    }

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

    public override string ToString()
    {
        return $"SocialMessageSpecification(Guid={Guid}, Sender={Sender}, Payload={Payload})";
    }
}