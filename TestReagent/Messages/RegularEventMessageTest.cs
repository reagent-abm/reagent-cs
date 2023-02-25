using Microsoft.Extensions.Logging;
using Reagent.Messages;
using TestReagent.Agent;
using TestReagent.SimulationManager;

namespace TestReagent.Messages;

public class RegularEventMessageTest
{
    private static readonly ILoggerFactory LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    });

    private static readonly ILogger<RegularEventMessage> Logger = LoggerFactory.CreateLogger<RegularEventMessage>();

    [Fact]
    public void Constructor_FixedGuid_Assigns()
    {
        var g = Guid.NewGuid();
        var agent = new AgentTest.AgentTestImpl();
        var m = new RegularEventMessage(agent, g, Logger);
        Assert.Equal(g, m.Guid);
        Assert.Equal(agent.Guid, m.Destination);
        Assert.Equal(agent.Guid, m.Sender);
    }

    [Fact]
    public void Constructor_NoGuid_AssignsRandomGuid()
    {
        var agent = new AgentTest.AgentTestImpl();
        var m1 = new RegularEventMessage(agent, Logger);
        var m2 = new RegularEventMessage(agent, Logger);
        Assert.NotEqual(m1.Guid, m2.Guid);
        Assert.Equal(agent.Guid, m1.Destination);
        Assert.Equal(agent.Guid, m1.Sender);
        Assert.Equal(agent.Guid, m2.Destination);
        Assert.Equal(agent.Guid, m2.Sender);
    }

    [Fact]
    public void StaticLogger_GetAndSet_ReturnsAndSets()
    {
        Assert.Null(RegularEventMessage.StaticLogger);
        RegularEventMessage.StaticLogger = Logger;
        Assert.Equal(Logger, RegularEventMessage.StaticLogger);
        UnsetStaticLogger();
    }

    private static void UnsetStaticLogger()
    {
        RegularEventMessage.StaticLogger = null;
    }

    [Fact]
    public void ToString_Always_RepresentationIsCorrect()
    {
        var agent = new AgentTest.AgentTestImpl();
        var m = new RegularEventMessage(agent, Logger);
        Assert.Equal($"RegularEventMessage(Sender={m.Sender}, Destination={m.Destination}, Guid={m.Guid})",
            m.ToString());
    }

    [Fact]
    public void CreateAndScheduleMessages_StartTimeAfterEndTime_ThrowsException()
    {
        var agent = new AgentTest.AgentTestImpl();
        var m = new RegularEventMessage(agent, Logger);
        Assert.Throws<ArgumentException>(() =>
            RegularEventMessage.CreateAndScheduleMessages(m, TimeSpan.FromDays(1), DateTime.Now,
                DateTime.Now - TimeSpan.FromDays(1)));
    }
    
    [Fact]
    public void CreateAndScheduleMessages_TimeSpanIsZero_ThrowsException()
    {
        var agent = new AgentTest.AgentTestImpl();
        var m = new RegularEventMessage(agent, Logger);
        Assert.Throws<ArgumentException>(() =>
            RegularEventMessage.CreateAndScheduleMessages(m, TimeSpan.Zero, DateTime.Now,
                DateTime.Now + TimeSpan.FromDays(1)));
    }
    
    [Fact]
    public void CreateAndScheduleMessages_TimeSpanIsNegative_ThrowsException()
    {
        var agent = new AgentTest.AgentTestImpl();
        var m = new RegularEventMessage(agent, Logger);
        Assert.Throws<ArgumentException>(() =>
            RegularEventMessage.CreateAndScheduleMessages(m, TimeSpan.FromDays(-1), DateTime.Now,
                DateTime.Now + TimeSpan.FromDays(1)));
    }

    [Fact]
    public void CreateAndScheduleMessages_Correct_SchedulesCorrectly()
    {
        var simulationStartTime = new DateTime(2023, 1, 1);
        var simulationEndTime = new DateTime(2025, 1, 1);
        var loggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
        var logger = loggerFactory.CreateLogger<SimulationManagerTest.SimulationManagerImpl>();
        var simulationManager = new SimulationManagerTest.SimulationManagerImpl(logger, simulationStartTime, simulationEndTime);
        Reagent.SimulationManager.SimulationManager.Instance = simulationManager;

        var agent = new AgentTest.AgentTestImpl();
        var prototype = new RegularEventMessage(agent, Logger);
        var timeSpan = TimeSpan.FromDays(1);
        var scheduleStartTime = simulationStartTime + TimeSpan.FromDays(1);
        var scheduleEndTime = scheduleStartTime + TimeSpan.FromDays(7) + TimeSpan.FromMinutes(1);
        RegularEventMessage.StaticLogger = Logger;
        RegularEventMessage.CreateAndScheduleMessages(prototype, timeSpan, scheduleStartTime, scheduleEndTime);
        var messages = simulationManager.GetMessageQueue;
        Assert.Equal(8, messages.Count);
        Assert.Equal(scheduleStartTime, messages.Keys.First());
        Assert.Equal(scheduleStartTime + TimeSpan.FromDays(7), messages.Keys.Last());
        
        for (uint i = 0; i < 8; i++)
        {
            var message = messages.Values.ElementAt((int) i).First();
            Assert.NotEqual(prototype.Guid, message.Guid);
            Assert.Equal(prototype.Sender, message.Sender);
            Assert.Equal(prototype.Destination, message.Destination);
            Assert.Equal(scheduleStartTime + TimeSpan.FromDays(i), messages.Keys.ElementAt((int) i));
        }
        
        UnsetStaticLogger();
        SimulationManager.SimulationManagerTest.UnsetInstance();
    }
}