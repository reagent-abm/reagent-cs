using Microsoft.Extensions.Logging;
using Reagent.Social.SocialNetwork;

namespace Reagent.Social.SimulationManager;

// ReSharper disable once UnusedType.Global
/// <summary>
/// A <c>SimulationManager</c> that has a <c>SocialNetwork</c>.
/// </summary>
public class SocialSimulationManager : Reagent.SimulationManager.SimulationManager, IHasSocialNetwork
{
    /// <summary>
    /// The <c>SocialNetwork</c>.
    /// </summary>
    public ISocialNetwork SocialNetwork { get; }
    
    /// <summary>
    /// Create a new <c>SocialSimulationManager</c>.
    /// </summary>
    /// <param name="logger">
    /// The logger.
    /// </param>
    /// <param name="startTime">
    /// The start time.
    /// </param>
    /// <param name="endTime">
    /// The end time.
    /// </param>
    /// <param name="socialNetwork">
    /// The <c>SocialNetwork</c>.
    /// </param>
    public SocialSimulationManager(ILogger<Reagent.SimulationManager.SimulationManager> logger, DateTime startTime,
        DateTime endTime, ISocialNetwork socialNetwork) : base(logger, startTime, endTime)
    {
        SocialNetwork = socialNetwork;
    }

    /// <summary>
    /// Add an <c>Agent</c> to the <c>SocialNetwork</c> and simulation.
    /// </summary>
    /// <param name="agent">The <c>Agent</c>.</param>
    public override void AddAgent(Agent.Agent agent)
    {
        base.AddAgent(agent);
        SocialNetwork.AddAgent(agent);
    }

    /// <summary>
    /// Get a string representation of the <c>SocialSimulationManager</c>.
    /// </summary>
    /// <returns>
    /// The string representation.
    /// </returns>
    public override string ToString()
    {
        return $"SocialSimulationManager(StartTime={StartTime}, EndTime={EndTime}, CurrentTime={CurrentTime})";
    }
}