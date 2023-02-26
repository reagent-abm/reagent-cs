using Microsoft.Extensions.Logging;
using QuikGraph;
using Reagent.Social.Messages;

namespace Reagent.Social.SocialNetwork;

public class DirectedWeightedSocialNetwork : ISocialNetwork
{
    protected virtual AdjacencyGraph<Guid, STaggedEdge<Guid, double>> Network { get; } = new();
    
    protected virtual ILogger<DirectedWeightedSocialNetwork> Logger { get; }

    public DirectedWeightedSocialNetwork(ILogger<DirectedWeightedSocialNetwork> logger)
    {
        Logger = logger;
    }

    public virtual void AddAgent(Agent.Agent agent)
    {
        Logger.LogDebug("Adding agent {Agent} to the social network", agent);
        Network.AddVertex(agent.Guid);
    }

    public virtual void AddEdge(Agent.Agent source, Agent.Agent target, double weight = 1)
    {
        Logger.LogDebug("Adding edge between {Source} and {Target} with weight {Weight} to the social network", source, target, weight);
        Network.AddEdge(new STaggedEdge<Guid, double>(source.Guid, target.Guid, weight));
    }

    public virtual void RemoveEdge(Agent.Agent source, Agent.Agent target)
    {
        Logger.LogDebug("Removing edge between {Source} and {Target} from the social network", source, target);
        if (Network.TryGetEdge(source.Guid, target.Guid, out var edge))
        {
            Network.RemoveEdge(edge);
        }
    }

    public virtual double? GetWeight(Agent.Agent source, Agent.Agent target)
    {
        Logger.LogTrace("Getting weight between {Source} and {Target} from the social network", source, target);
        if (Network.TryGetEdge(source.Guid, target.Guid, out var edge))
        {
            return edge.Tag;
        }

        return null;
    }

    public virtual void SetWeight(Agent.Agent source, Agent.Agent target, double weight = 1)
    {
        Logger.LogDebug("Setting weight between {Source} and {Target} to {Weight} in the social network", source, target, weight);
        RemoveEdge(source, target);
        AddEdge(source, target, weight);
    }

    public virtual void SendSocialMessages(SocialMessageSpecification specification)
    {
        var tags = from outgoingEdges in Network.OutEdges(specification.Sender)
            select new Tuple<Guid, double>(outgoingEdges.Target, outgoingEdges.Tag);
        var messages = specification.ToSocialMessagesFromGuidsAndWeights(tags.ToList());

        foreach (var message in messages)
        {
            SimulationManager.SimulationManager.Instance!.SendMessageNow(message);
        }
    }
    
    public override string ToString()
    {
        return $"DirectedWeightedSocialNetwork(Network={Network})";
    }
}