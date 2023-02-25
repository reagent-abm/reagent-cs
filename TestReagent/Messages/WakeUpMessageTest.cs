using Microsoft.Extensions.Logging;
using Reagent.Messages;
using TestReagent.Agent;
using TestReagent.SimulationManager;

namespace TestReagent.Messages;

public class WakeUpMessageTest
{
    [Fact]
    public void Constructor_FixedGuid_AssignsAndSchedules()
    {
        var startTime = DateTime.Now;
        var endTime = startTime.AddYears(1);
        var loggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
        var logger = loggerFactory.CreateLogger<SimulationManagerTest.SimulationManagerImpl>();
        var simulationManager = new SimulationManagerTest.SimulationManagerImpl(logger, startTime, endTime);
        Reagent.SimulationManager.SimulationManager.Instance = simulationManager;
        
        var g = Guid.NewGuid();
        var agent = new AgentTest.AgentTestImpl();
        var wakeTime = startTime.AddMonths(6);
        var m = new WakeUpMessage(agent, wakeTime, g);
        Assert.Equal(g, m.Guid);
        Assert.Equal(agent.Guid, m.Destination);
        Assert.Equal(agent.Guid, m.Sender);
        Assert.Equal(wakeTime, m.WakeTime);
        
        var messageQueue = simulationManager.GetMessageQueue;
        Assert.True(messageQueue.ContainsKey(wakeTime));
        var queue = messageQueue[wakeTime];
        Assert.Single(queue);
        Assert.Equal(m, queue.Dequeue());
        SimulationManager.SimulationManagerTest.UnsetInstance();
    }

    [Fact]
    public void Constructor_NoGuid_AssignsRandomGuidAndSchedules()
    {
        var startTime = DateTime.Now;
        var endTime = startTime.AddYears(1);
        var loggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
        var logger = loggerFactory.CreateLogger<SimulationManagerTest.SimulationManagerImpl>();
        var simulationManager = new SimulationManagerTest.SimulationManagerImpl(logger, startTime, endTime);
        Reagent.SimulationManager.SimulationManager.Instance = simulationManager;
        
        var agent = new AgentTest.AgentTestImpl();
        var wakeTime = startTime.AddMonths(6);
        var m1 = new WakeUpMessage(agent, wakeTime);
        var m2 = new WakeUpMessage(agent, wakeTime);
        Assert.NotEqual(m1.Guid, m2.Guid);
        Assert.Equal(agent.Guid, m1.Destination);
        Assert.Equal(agent.Guid, m1.Sender);
        Assert.Equal(agent.Guid, m2.Destination);
        Assert.Equal(agent.Guid, m2.Sender);
        Assert.Equal(wakeTime, m1.WakeTime);
        Assert.Equal(wakeTime, m2.WakeTime);
        
        var messageQueue = simulationManager.GetMessageQueue;
        Assert.True(messageQueue.ContainsKey(wakeTime));
        var queue = messageQueue[wakeTime];
        Assert.Equal(2, queue.Count);
        Assert.Equal(m1, queue.Dequeue());
        Assert.Equal(m2, queue.Dequeue());
        
        SimulationManager.SimulationManagerTest.UnsetInstance();
    }
    
    [Fact]
    public void ToString_Always_RepresentationIsCorrect()
    {
        var startTime = DateTime.Now;
        var endTime = startTime.AddYears(1);
        var loggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
        var logger = loggerFactory.CreateLogger<SimulationManagerTest.SimulationManagerImpl>();
        var simulationManager = new SimulationManagerTest.SimulationManagerImpl(logger, startTime, endTime);
        Reagent.SimulationManager.SimulationManager.Instance = simulationManager;
        
        var agent = new AgentTest.AgentTestImpl();
        var wakeTime = DateTime.Now;
        var m = new WakeUpMessage(agent, wakeTime);
        Assert.Equal($"WakeUpMessage(Sender={m.Sender}, Destination={m.Destination}, Guid={m.Guid}, WakeTime={m.WakeTime})",
            m.ToString());
        
        SimulationManager.SimulationManagerTest.UnsetInstance();
    }
}