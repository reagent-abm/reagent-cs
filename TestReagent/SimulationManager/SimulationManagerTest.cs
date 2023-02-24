using Microsoft.Extensions.Logging;
using TestReagent.Agent;

namespace TestReagent.SimulationManager;

using Reagent.SimulationManager;

public class SimulationManagerTest
{
    private static readonly DateTime StartTime = new(2020, 1, 1);
    private static readonly DateTime EndTime = new(2022, 1, 1);

    private static readonly ILoggerFactory LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    });

    private static readonly ILogger<SimulationManager> Logger = LoggerFactory.CreateLogger<SimulationManager>();

    [Fact]
    public void TestAddAgentWhenAgentDoesNotExist()
    {
        var simulationManager = new SimulationManager(Logger, StartTime, EndTime);
        var agent = new AgentTest.AgentTestImpl();
        simulationManager.AddAgent(agent);
        var agents =
            Utils.GetInstanceProperty(typeof(SimulationManager), simulationManager, "Agents") as
                IDictionary<Guid, Reagent.Agent.Agent>;
        Assert.NotNull(agents);
        Assert.Single(agents);
        Assert.Equal(agent, agents[agent.Guid]);
    }

    [Fact]
    public void TestAddAgentWhenAgentExists()
    {
        var simulationManager = new SimulationManager(Logger, StartTime, EndTime);
        var agent1 = new AgentTest.AgentTestImpl();
        var agents = new Dictionary<Guid, Reagent.Agent.Agent>
        {
            [agent1.Guid] = agent1
        };
        Utils.SetInstanceProperty(typeof(SimulationManager), simulationManager, "Agents", agents);
        var agent2 = new AgentTest.AgentTestImpl(agent1.Guid);
        simulationManager.AddAgent(agent2);
        Assert.Single(agents);
        Assert.NotEqual(agent1, agents[agent1.Guid]);
        Assert.Equal(agent2, agents[agent2.Guid]);
    }

    [Fact]
    public void TestAddTwoAgents()
    {
        var simulationManager = new SimulationManager(Logger, StartTime, EndTime);
        var agent1 = new AgentTest.AgentTestImpl();
        var agent2 = new AgentTest.AgentTestImpl();
        simulationManager.AddAgent(agent1);
        simulationManager.AddAgent(agent2);
        var agents =
            Utils.GetInstanceProperty(typeof(SimulationManager), simulationManager, "Agents") as
                IDictionary<Guid, Reagent.Agent.Agent>;
        Assert.NotNull(agents);
        Assert.Equal(2, agents.Count);
        Assert.Equal(agent1, agents[agent1.Guid]);
        Assert.Equal(agent2, agents[agent2.Guid]);
    }
}