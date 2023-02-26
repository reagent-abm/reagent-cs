using Microsoft.Extensions.Logging;
using Reagent.Social.SocialNetwork;

namespace Reagent.Social.SimulationManager;

public class SocialSimulationManager : Reagent.SimulationManager.SimulationManager, IHasSocialNetwork
{
    public ISocialNetwork SocialNetwork { get; }
    
    public SocialSimulationManager(ILogger<Reagent.SimulationManager.SimulationManager> logger, DateTime startTime,
        DateTime endTime, ISocialNetwork socialNetwork) : base(logger, startTime, endTime)
    {
        SocialNetwork = socialNetwork;
    }

    public override void AddAgent(Agent.Agent agent)
    {
        base.AddAgent(agent);
        SocialNetwork.AddAgent(agent);
    }

    public override string ToString()
    {
        return $"SocialSimulationManager(StartTime={StartTime}, EndTime={EndTime}, CurrentTime={CurrentTime})";
    }
}