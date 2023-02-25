using System.Reflection;
using System.Reflection.Emit;
using Reagent.Messages;

namespace TestReagent.Agent;

using Reagent.Agent;

public class AgentTest
{
    internal class AgentTestImpl : Agent
    {
        public AgentTestImpl(Guid guid) : base(guid)
        {
        }

        public AgentTestImpl()
        {
        }

        public override void HandleMessage(IMessage message)
        {
        }
    }
    
    [Fact]
    public void Constructor_NoGuid_AssignsRandomGuid()
    {
        var a1 = new AgentTestImpl();
        var a2 = new AgentTestImpl();
        Assert.NotEqual(a1.Guid, a2.Guid);
    }

    [Fact]
    public void Constructor_FixedGuid_Assigns()
    {
        var g = Guid.NewGuid();
        var a = new AgentTestImpl(g);
        Assert.Equal(g, a.Guid);
    }
    
    [Fact]
    public void ToString_Always_RepresentationIsCorrect()
    {
        var a = new AgentTestImpl();
        Assert.Equal($"Agent(Guid={a.Guid})", a.ToString());
    }
}