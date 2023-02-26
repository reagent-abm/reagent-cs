using Reagent.Social.Messages;

namespace Reagent.Social.SocialNetwork;

using Agent;

public interface ISocialNetwork
{
    public void AddAgent(Agent agent);

    public void AddEdge(Agent source, Agent target, double weight = 1.0);
    
    public void RemoveEdge(Agent source, Agent target);
    
    public double? GetWeight(Agent source, Agent target);
    
    public void SetWeight(Agent source, Agent target, double weight = 1.0);

    public void SendSocialMessages(SocialMessageSpecification specification);
}