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
    public void ConstructorAssignsRandomGuid()
    {
        var a1 = new AgentTestImpl();
        var a2 = new AgentTestImpl();
        Assert.NotEqual(a1.Guid, a2.Guid);
    }

    [Fact]
    public void ConstructorAssignsFixedGuid()
    {
        var g = Guid.NewGuid();
        var a = new AgentTestImpl(g);
        Assert.Equal(g, a.Guid);
    }
}