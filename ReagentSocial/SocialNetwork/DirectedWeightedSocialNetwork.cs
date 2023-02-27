using Microsoft.Extensions.Logging;
using QuikGraph;
using Reagent.Social.Messages;

namespace Reagent.Social.SocialNetwork;

/// <summary>
/// A directed, weighted social network.
/// </summary>
public class DirectedWeightedSocialNetwork : ISocialNetwork
{
    /// <summary>
    /// The network from Guids to Guids with weights.
    /// </summary>
    protected virtual AdjacencyGraph<Guid, STaggedEdge<Guid, double>> Network { get; } = new();
    
    /// <summary>
    /// The logger.
    /// </summary>
    protected virtual ILogger<DirectedWeightedSocialNetwork> Logger { get; }
    
    /// <summary>
    /// The simulation manager.
    /// </summary>
    protected virtual Reagent.SimulationManager.SimulationManager SimulationManager { get; }

    /// <summary>
    /// Create a new <c>DirectedWeightedSocialNetwork</c>.
    /// </summary>
    /// <param name="simulationManager">
    /// The simulation manager.
    /// </param>
    /// <param name="logger">
    /// The logger.
    /// </param>
    public DirectedWeightedSocialNetwork(Reagent.SimulationManager.SimulationManager simulationManager, ILogger<DirectedWeightedSocialNetwork> logger)
    {
        Logger = logger;
        SimulationManager = simulationManager;
    }

    /// <inheritdoc />
    public virtual void AddAgent(Agent.Agent agent)
    {
        Logger.LogDebug("Adding agent {Agent} to the social network", agent);
        Network.AddVertex(agent.Guid);
    }

    /// <inheritdoc />
    public virtual void AddEdge(Agent.Agent source, Agent.Agent target, double weight = 1)
    {
        Logger.LogDebug("Adding edge between {Source} and {Target} with weight {Weight} to the social network", source, target, weight);
        Network.AddEdge(new STaggedEdge<Guid, double>(source.Guid, target.Guid, weight));
    }

    /// <inheritdoc />
    public virtual void RemoveEdge(Agent.Agent source, Agent.Agent target)
    {
        Logger.LogDebug("Removing edge between {Source} and {Target} from the social network", source, target);
        if (Network.TryGetEdge(source.Guid, target.Guid, out var edge))
        {
            Network.RemoveEdge(edge);
        }
    }

    /// <inheritdoc />
    public virtual double? GetWeight(Agent.Agent source, Agent.Agent target)
    {
        Logger.LogTrace("Getting weight between {Source} and {Target} from the social network", source, target);
        if (Network.TryGetEdge(source.Guid, target.Guid, out var edge))
        {
            return edge.Tag;
        }

        return null;
    }

    /// <inheritdoc />
    public virtual void SetWeight(Agent.Agent source, Agent.Agent target, double weight = 1)
    {
        Logger.LogDebug("Setting weight between {Source} and {Target} to {Weight} in the social network", source, target, weight);
        RemoveEdge(source, target);
        AddEdge(source, target, weight);
    }

    /// <inheritdoc />
    public virtual void SendSocialMessages(SocialMessageSpecification specification)
    {
        var tags = from outgoingEdges in Network.OutEdges(specification.Sender)
            select new Tuple<Guid, double>(outgoingEdges.Target, outgoingEdges.Tag);
        var messages = specification.ToSocialMessagesFromGuidsAndWeights(tags.ToList());

        foreach (var message in messages)
        {
            SimulationManager.SendMessageNow(message);
        }
    }
    
    /// <summary>
    /// Get a string representation of the <c>DirectedWeightedSocialNetwork</c>.
    /// </summary>
    /// <returns>
    /// The string representation of the <c>DirectedWeightedSocialNetwork</c>.
    /// </returns>
    public override string ToString()
    {
        return $"DirectedWeightedSocialNetwork(Network={Network})";
    }
}