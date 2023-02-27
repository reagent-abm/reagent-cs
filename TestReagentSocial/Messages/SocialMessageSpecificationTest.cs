using Microsoft.Extensions.Logging;
using Reagent.Social.Messages;
using TestReagent.Agent;
using TestReagent.SimulationManager;

namespace TestReagent.Social.Messages;

public class SocialMessageSpecificationTest
{
    [Fact]
    public void Guid_Always_ReturnsGuid()
    {
        var guid = Guid.NewGuid();
        var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
        var logger = loggerFactory.CreateLogger<SocialMessageSpecification>();
        var specification = new SocialMessageSpecification(guid, Guid.NewGuid(),
            new SimulationManagerTest.MessageImpl(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()), logger);
        Assert.Equal(guid, specification.Guid);
    }
    
    [Fact]
    public void Sender_Always_ReturnsSender()
    {
        var sender = Guid.NewGuid();
        var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
        var logger = loggerFactory.CreateLogger<SocialMessageSpecification>();
        var specification = new SocialMessageSpecification(Guid.NewGuid(), sender,
            new SimulationManagerTest.MessageImpl(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()), logger);
        Assert.Equal(sender, specification.Sender);
    }
    
    [Fact]
    public void Payload_Always_ReturnsPayload()
    {
        var payload = new SimulationManagerTest.MessageImpl(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
        var logger = loggerFactory.CreateLogger<SocialMessageSpecification>();
        var specification = new SocialMessageSpecification(Guid.NewGuid(), Guid.NewGuid(), payload, logger);
        Assert.Equal(payload, specification.Payload);
    }
    
    [Fact]
    public void Constructor_NoGuid_AssignsRandomGuid()
    {
        var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
        var logger = loggerFactory.CreateLogger<SocialMessageSpecification>();
        var specification1 = new SocialMessageSpecification(Guid.NewGuid(),
            new SimulationManagerTest.MessageImpl(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()), logger);
        var specification2 = new SocialMessageSpecification(Guid.NewGuid(),
            new SimulationManagerTest.MessageImpl(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()), logger);
        Assert.NotEqual(specification1.Guid, specification2.Guid);
    }
    
    [Fact]
    public void ToString_Always_ReturnsCorrectRepresentation()
    {
        var guid = Guid.NewGuid();
        var sender = Guid.NewGuid();
        var payload = new SimulationManagerTest.MessageImpl(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
        var logger = loggerFactory.CreateLogger<SocialMessageSpecification>();
        var specification = new SocialMessageSpecification(guid, sender, payload, logger);
        Assert.Equal($"SocialMessageSpecification(Guid={guid}, Sender={sender}, Payload={payload})", specification.ToString());
    }

    [Fact]
    public void ToSocialMessagesFromGuids_WhenEmpty_ReturnsEmpty()
    {
        var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
        var logger = loggerFactory.CreateLogger<SocialMessageSpecification>();
        var specification = new SocialMessageSpecification(Guid.NewGuid(), Guid.NewGuid(),
            new SimulationManagerTest.MessageImpl(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()), logger);
        var messages = specification.ToSocialMessagesFromGuids(new List<Guid>());
        Assert.Empty(messages);
    }

    [Fact]
    public void ToSocialMessagesFromGuids_WhenNotEmpty_ReturnsMessages()
    {
        var sender = Guid.NewGuid();
        var payload = new SimulationManagerTest.MessageImpl(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
        var logger = loggerFactory.CreateLogger<SocialMessageSpecification>();
        var specification = new SocialMessageSpecification(sender, payload, logger);
        var destinations = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        var messages = specification.ToSocialMessagesFromGuids(destinations).ToList();
        Assert.Equal(destinations.Count, messages.Count);
        foreach (var message in messages)
        {
            Assert.Equal(sender, message.Sender);
            Assert.Equal(payload, message.Payload);
        }
    }
    
    [Fact]
    public void ToSocialMessagesFromAgents_WhenEmpty_ReturnsEmpty()
    {
        var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
        var logger = loggerFactory.CreateLogger<SocialMessageSpecification>();
        var specification = new SocialMessageSpecification(Guid.NewGuid(), Guid.NewGuid(),
            new SimulationManagerTest.MessageImpl(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()), logger);
        var messages = specification.ToSocialMessagesFromAgents(new List<Reagent.Agent.Agent>());
        Assert.Empty(messages);
    }
    
    [Fact]
    public void ToSocialMessagesFromAgents_WhenNotEmpty_ReturnsMessages()
    {
        var sender = Guid.NewGuid();
        var payload = new SimulationManagerTest.MessageImpl(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
        var logger = loggerFactory.CreateLogger<SocialMessageSpecification>();
        var specification = new SocialMessageSpecification(sender, payload, logger);
        var destinations = new List<Reagent.Agent.Agent> { new AgentTest.AgentTestImpl(Guid.NewGuid()), new AgentTest.AgentTestImpl(Guid.NewGuid()), new AgentTest.AgentTestImpl(Guid.NewGuid()) };
        var messages = specification.ToSocialMessagesFromAgents(destinations).ToList();
        Assert.Equal(destinations.Count, messages.Count);
        foreach (var message in messages)
        {
            Assert.Equal(sender, message.Sender);
            Assert.Equal(payload, message.Payload);
        }
    }
    
    [Fact]
    public void ToSocialMessagesFromGuidsAndWeights_WhenEmpty_ReturnsEmpty()
    {
        var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
        var logger = loggerFactory.CreateLogger<SocialMessageSpecification>();
        var specification = new SocialMessageSpecification(Guid.NewGuid(), Guid.NewGuid(),
            new SimulationManagerTest.MessageImpl(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()), logger);
        var messages = specification.ToSocialMessagesFromGuidsAndWeights(new List<Tuple<Guid, double>>());
        Assert.Empty(messages);
    }
    
    [Fact]
    public void ToSocialMessagesFromGuidsAndWeights_WhenNotEmpty_ReturnsMessages()
    {
        var sender = Guid.NewGuid();
        var payload = new SimulationManagerTest.MessageImpl(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
        var logger = loggerFactory.CreateLogger<SocialMessageSpecification>();
        var specification = new SocialMessageSpecification(sender, payload, logger);
        var destinations = new List<Tuple<Guid, double>>
        {
            new(Guid.NewGuid(), 0.25),
            new(Guid.NewGuid(), 0.55),
            new(Guid.NewGuid(), 0.75)
        };
        var messages = specification.ToSocialMessagesFromGuidsAndWeights(destinations).ToList();
        Assert.Equal(destinations.Count, messages.Count);
        foreach (var message in messages)
        {
            Assert.Equal(sender, message.Sender);
            Assert.Equal(payload, message.Payload);
        }
    }
    
    [Fact]
    public void ToSocialMessagesFromAgentsAndWeights_WhenEmpty_ReturnsEmpty()
    {
        var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
        var logger = loggerFactory.CreateLogger<SocialMessageSpecification>();
        var specification = new SocialMessageSpecification(Guid.NewGuid(), Guid.NewGuid(),
            new SimulationManagerTest.MessageImpl(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()), logger);
        var messages = specification.ToSocialMessagesFromAgentsAndWeights(new List<Tuple<Reagent.Agent.Agent, double>>());
        Assert.Empty(messages);
    }
    
    [Fact]
    public void ToSocialMessagesFromAgentsAndWeights_WhenNotEmpty_ReturnsMessages()
    {
        var sender = Guid.NewGuid();
        var payload = new SimulationManagerTest.MessageImpl(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
        var logger = loggerFactory.CreateLogger<SocialMessageSpecification>();
        var specification = new SocialMessageSpecification(sender, payload, logger);
        var destinations = new List<Tuple<Reagent.Agent.Agent, double>>
        {
            new(new AgentTest.AgentTestImpl(Guid.NewGuid()), 0.25),
            new(new AgentTest.AgentTestImpl(Guid.NewGuid()), 0.55),
            new(new AgentTest.AgentTestImpl(Guid.NewGuid()), 0.75)
        };
        var messages = specification.ToSocialMessagesFromAgentsAndWeights(destinations).ToList();
        Assert.Equal(destinations.Count, messages.Count);
        foreach (var message in messages)
        {
            Assert.Equal(sender, message.Sender);
            Assert.Equal(payload, message.Payload);
        }
    }
}