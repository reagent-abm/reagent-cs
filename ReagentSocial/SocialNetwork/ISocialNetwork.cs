using Reagent.Social.Messages;

namespace Reagent.Social.SocialNetwork;

using Agent;

/// <summary>
/// A <c>SocialNetwork</c> is a network of <c>Agent</c>s.
/// </summary>
public interface ISocialNetwork
{
    /// <summary>
    /// Add an <c>Agent</c> to the <c>SocialNetwork</c>.
    /// </summary>
    /// <param name="agent">
    /// The <c>Agent</c>.
    /// </param>
    public void AddAgent(Agent agent);

    /// <summary>
    /// Add an edge to the <c>SocialNetwork</c> between two <c>Agent</c>s.
    /// </summary>
    /// <param name="source">
    /// The source <c>Agent</c>.
    /// </param>
    /// <param name="target">
    /// The target <c>Agent</c>.
    /// </param>
    /// <param name="weight">
    /// The weight of the edge.
    /// </param>
    public void AddEdge(Agent source, Agent target, double weight = 1.0);
    
    /// <summary>
    /// Remove an edge from the <c>SocialNetwork</c>.
    /// </summary>
    /// <param name="source">
    /// The source <c>Agent</c>.
    /// </param>
    /// <param name="target">
    /// The target <c>Agent</c>.
    /// </param>
    public void RemoveEdge(Agent source, Agent target);
    
    /// <summary>
    /// Get the weight of an edge.
    /// </summary>
    /// <param name="source">
    /// The source <c>Agent</c>.
    /// </param>
    /// <param name="target">
    /// The target <c>Agent</c>.
    /// </param>
    /// <returns>
    /// The weight of the edge, if found.
    /// </returns>
    public double? GetWeight(Agent source, Agent target);
    
    /// <summary>
    /// Set the weight of an edge.
    /// </summary>
    /// <param name="source">
    /// The source <c>Agent</c>.
    /// </param>
    /// <param name="target">
    /// The target <c>Agent</c>.
    /// </param>
    /// <param name="weight">
    /// The new weight of the edge.
    /// </param>
    public void SetWeight(Agent source, Agent target, double weight = 1.0);

    /// <summary>
    /// Send <c>SocialMessage</c>s to the <c>Agent</c>s that are connected to the <c>Agent</c> specified in the
    /// <c>SocialMessageSpecification</c>.
    /// </summary>
    /// <param name="specification">
    /// The <c>SocialMessageSpecification</c>.
    /// </param>
    public void SendSocialMessages(SocialMessageSpecification specification);
}