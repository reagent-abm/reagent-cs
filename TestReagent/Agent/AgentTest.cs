using System.Reflection;
using System.Reflection.Emit;
using Reagent.Messages;
using TestReagent.SimulationManager;

namespace TestReagent.Agent;

using Reagent.Agent;

public class AgentTest
{
    public class AgentTestImpl : Agent
    {
        public AgentTestImpl(Guid guid) : base(guid)
        {
        }

        public AgentTestImpl()
        {
        }

        public override void HandleMessage(IMessage message)
        {
            throw new System.NotImplementedException();
        }
    }
    
    [Fact]
    public void HandleMessage_Always_ThrowsNotImplementedException()
    {
        var a = new AgentTestImpl();
        var m = new SimulationManagerTest.MessageImpl(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        Assert.Throws<NotImplementedException>(() => a.HandleMessage(m));
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