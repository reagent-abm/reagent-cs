using System.Collections;
using Microsoft.Extensions.Logging;
using Reagent.Messages;
using TestReagent.Agent;

namespace TestReagent.SimulationManager;

using Reagent.SimulationManager;

[Collection("Uses SimulationManager")]
public class SimulationManagerTest
{
    public class MessageImpl : IMessage
    {
        public Guid Guid { get; }
        public Guid Destination { get; }
        public Guid Sender { get; }

        public MessageImpl(Guid guid, Guid destination, Guid sender)
        {
            Guid = guid;
            Destination = destination;
            Sender = sender;
        }
    }
    
    public class SimulationManagerImpl : SimulationManager
    {
        public SimulationManagerImpl(ILogger<SimulationManager> logger, DateTime startTime, DateTime endTime) : base(
            logger, startTime, endTime)
        {
        }

        public SortedDictionary<DateTime, Queue<IMessage>> GetMessageQueue => MessageQueue;
        
        public void SetMessageQueue(SortedDictionary<DateTime, Queue<IMessage>> messageQueue)
        {
            MessageQueue = messageQueue;
        }
        
        public void SetCurrentTime(DateTime currentTime)
        {
            CurrentTime = currentTime;
        }
        
        public IDictionary<Guid, Reagent.Agent.Agent> GetAgents => Agents;
        
        public void SetAgents(IDictionary<Guid, Reagent.Agent.Agent> agents)
        {
            Agents = agents;
        }
    }
    
    private static readonly DateTime StartTime = new(2020, 1, 1);
    private static readonly DateTime EndTime = new(2022, 1, 1);

    private static readonly ILoggerFactory LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    });

    private static readonly ILogger<SimulationManager> Logger = LoggerFactory.CreateLogger<SimulationManager>();

    [Fact]
    public void AddAgent_NotExists_IsAdded()
    {
        var simulationManager = new SimulationManagerImpl(Logger, StartTime, EndTime);
        var agent = new AgentTest.AgentTestImpl();
        simulationManager.AddAgent(agent);
        var agents = simulationManager.GetAgents;
        Assert.NotNull(agents);
        Assert.Single(agents);
        Assert.Equal(agent, agents[agent.Guid]);
    }

    [Fact]
    public void AddAgent_Exists_Replaces()
    {
        var simulationManager = new SimulationManagerImpl(Logger, StartTime, EndTime);
        var agent1 = new AgentTest.AgentTestImpl();
        var agents = new Dictionary<Guid, Reagent.Agent.Agent>
        {
            [agent1.Guid] = agent1
        };
        simulationManager.SetAgents(agents);
        var agent2 = new AgentTest.AgentTestImpl(agent1.Guid);
        simulationManager.AddAgent(agent2);
        Assert.Single(agents);
        Assert.NotEqual(agent1, agents[agent1.Guid]);
        Assert.Equal(agent2, agents[agent2.Guid]);
    }

    [Fact]
    public void AddAgent_AddTwo_BothAdded()
    {
        var simulationManager = new SimulationManagerImpl(Logger, StartTime, EndTime);
        var agent1 = new AgentTest.AgentTestImpl();
        var agent2 = new AgentTest.AgentTestImpl();
        simulationManager.AddAgent(agent1);
        simulationManager.AddAgent(agent2);
        var agents = simulationManager.GetAgents;
        Assert.NotNull(agents);
        Assert.Equal(2, agents.Count);
        Assert.Equal(agent1, agents[agent1.Guid]);
        Assert.Equal(agent2, agents[agent2.Guid]);
    }

    [Fact]
    public void GetInstance_NotSet_ReturnsNull()
    {
        Assert.Null(SimulationManager.Instance);
    }

    internal static void UnsetInstance()
    {
        SimulationManager.Instance = null;
    }
    
    [Fact]
    public void GetInstance_Set_ReturnsInstance()
    {
        var simulationManager = new SimulationManager(Logger, StartTime, EndTime);
        SimulationManager.Instance = simulationManager;
        Assert.Equal(simulationManager, SimulationManager.Instance);
        UnsetInstance();
    }
    
    [Fact]
    public void GetStartTime_Always_ReturnsStartTime()
    {
        var simulationManager = new SimulationManager(Logger, StartTime, EndTime);
        Assert.Equal(StartTime, simulationManager.StartTime);
    }
    
    [Fact]
    public void GetEndTime_Always_ReturnsEndTime()
    {
        var simulationManager = new SimulationManager(Logger, StartTime, EndTime);
        Assert.Equal(EndTime, simulationManager.EndTime);
    }
    
    [Fact]
    public void GetCurrentTime_NotSet_ReturnsStartTime()
    {
        var simulationManager = new SimulationManager(Logger, StartTime, EndTime);
        Assert.Equal(StartTime, simulationManager.CurrentTime);
    }

    [Fact]
    public void GetCurrentTime_WhenUpdatedBySchedulingAndRun_ReturnsUpdatedTime()
    {
        var simulationManager = new SimulationManager(Logger, StartTime, EndTime);
        var agent = new AgentTest.AgentTestImpl();
        simulationManager.AddAgent(agent);
        var message = new MessageImpl(Guid.NewGuid(), agent.Guid, Guid.NewGuid());
        var messageTime = StartTime.AddMonths(1);
        simulationManager.ScheduleMessage(message, messageTime);
        simulationManager.Run();
        Assert.Equal(messageTime, simulationManager.CurrentTime);
    }
    
    [Fact]
    public void SetMessageQueue_Always_SetsMessageQueue()
    {
        var simulationManager = new SimulationManagerImpl(Logger, StartTime, EndTime);
        var messageQueue = new SortedDictionary<DateTime, Queue<IMessage>>();
        simulationManager.SetMessageQueue(messageQueue);
        Assert.Equal(messageQueue, simulationManager.GetMessageQueue);
    }
    
    [Fact]
    public void ScheduleMessage_TimeBeforeStartTime_ThrowsException()
    {
        var simulationManager = new SimulationManager(Logger, StartTime, EndTime);
        var agent = new AgentTest.AgentTestImpl();
        simulationManager.AddAgent(agent);
        var message = new MessageImpl(Guid.NewGuid(), agent.Guid, Guid.NewGuid());
        var messageTime = StartTime.AddMonths(-1);
        Assert.Throws<ArgumentOutOfRangeException>(() => simulationManager.ScheduleMessage(message, messageTime));
    }
    
    [Fact]
    public void ScheduleMessage_TimeBeforeCurrentTimeButAfterStartTime_ThrowsException()
    {
        var simulationManager = new SimulationManagerImpl(Logger, StartTime, EndTime);
        var agent = new AgentTest.AgentTestImpl();
        simulationManager.AddAgent(agent);
        var message = new MessageImpl(Guid.NewGuid(), agent.Guid, Guid.NewGuid());
        var currentTime = StartTime.AddMonths(2);
        var messageTime = StartTime.AddMonths(1);
        simulationManager.SetCurrentTime(currentTime);
        Assert.Throws<ArgumentOutOfRangeException>(() => simulationManager.ScheduleMessage(message, messageTime));
    }
    
    [Fact]
    public void ScheduleMessage_TimeAfterEndTime_ThrowsException()
    {
        var simulationManager = new SimulationManager(Logger, StartTime, EndTime);
        var agent = new AgentTest.AgentTestImpl();
        simulationManager.AddAgent(agent);
        var message = new MessageImpl(Guid.NewGuid(), agent.Guid, Guid.NewGuid());
        var messageTime = EndTime.AddMonths(1);
        Assert.Throws<ArgumentOutOfRangeException>(() => simulationManager.ScheduleMessage(message, messageTime));
    }

    [Fact]
    public void SendMessageNow_Always_AddsToMessageQueueCorrectly()
    {
        var simulationManager = new SimulationManagerImpl(Logger, StartTime, EndTime);
        var message = new MessageImpl(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        var currentTime = StartTime.AddMonths(1);
        simulationManager.SetCurrentTime(currentTime);
        simulationManager.SendMessageNow(message);
        var messageQueue = simulationManager.GetMessageQueue;
        Assert.Single(messageQueue);
        Assert.Equal(currentTime, messageQueue.Keys.First());
        Assert.Single(messageQueue[currentTime]);
        Assert.Equal(message, messageQueue[currentTime].Dequeue());
    }

    [Fact]
    public void Run_WithUnknownDestination_ThrowsException()
    {
        var simulationManager = new SimulationManager(Logger, StartTime, EndTime);
        var message = new MessageImpl(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        Assert.NotEqual(message.Destination, message.Sender);
        Assert.NotEqual(message.Guid, message.Destination);
        Assert.NotEqual(message.Guid, message.Sender);
        simulationManager.SendMessageNow(message);
        Assert.Throws<InvalidOperationException>(() => simulationManager.Run());
    }

    [Fact]
    public void ScheduleMessage_ExistingQueue_AddsToQueue()
    {
        var simulationManager = new SimulationManagerImpl(Logger, StartTime, EndTime);
        var messageTime = StartTime.AddMonths(1);
        var message1 = new MessageImpl(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        var queue = new Queue<IMessage>();
        queue.Enqueue(message1);
        var messageQueue = new SortedDictionary<DateTime, Queue<IMessage>>
        {
            [messageTime] = queue
        };
        simulationManager.SetMessageQueue(messageQueue);
        var message2 = new MessageImpl(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        simulationManager.ScheduleMessage(message2, messageTime);
        Assert.Equal(2, messageQueue[messageTime].Count);
        Assert.Equal(message1, messageQueue[messageTime].Dequeue());
        Assert.Equal(message2, messageQueue[messageTime].Dequeue());
    }
}