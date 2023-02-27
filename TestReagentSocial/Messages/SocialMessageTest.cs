using TestReagent.SimulationManager;

namespace TestReagent.Social.Messages;

using Reagent.Social.Messages;

public class SocialMessageTest
{
    [Fact]
    public void Guid_Always_ReturnsGuid()
    {
        var guid = Guid.NewGuid();
        var socialMessage = new SocialMessage(guid, Guid.NewGuid(), Guid.NewGuid(),
            new SimulationManagerTest.MessageImpl(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()));
        Assert.Equal(guid, socialMessage.Guid);
    }
    
    [Fact]
    public void Destination_Always_ReturnsDestination()
    {
        var destination = Guid.NewGuid();
        var socialMessage = new SocialMessage(Guid.NewGuid(), destination, Guid.NewGuid(),
            new SimulationManagerTest.MessageImpl(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()));
        Assert.Equal(destination, socialMessage.Destination);
    }
    
    [Fact]
    public void Sender_Always_ReturnsSender()
    {
        var sender = Guid.NewGuid();
        var socialMessage = new SocialMessage(Guid.NewGuid(), Guid.NewGuid(), sender,
            new SimulationManagerTest.MessageImpl(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()));
        Assert.Equal(sender, socialMessage.Sender);
    }
    
    [Fact]
    public void Payload_Always_ReturnsPayload()
    {
        var payload = new SimulationManagerTest.MessageImpl(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        var socialMessage = new SocialMessage(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), payload);
        Assert.Equal(payload, socialMessage.Payload);
    }
    
    [Fact]
    public void Weight_Always_ReturnsWeight()
    {
        const double weight = 1.0;
        var socialMessage = new SocialMessage(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            new SimulationManagerTest.MessageImpl(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()), weight);
        Assert.Equal(weight, socialMessage.Weight);
    }
    
    [Fact]
    public void Constructor_NoGuid_AssignsRandomGuid()
    {
        var socialMessage1 = new SocialMessage(Guid.NewGuid(), Guid.NewGuid(),
            new SimulationManagerTest.MessageImpl(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()));
        var socialMessage2 = new SocialMessage(Guid.NewGuid(), Guid.NewGuid(),
            new SimulationManagerTest.MessageImpl(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()));
        Assert.NotEqual(socialMessage1.Guid, socialMessage2.Guid);
    }
    
    [Fact]
    public void ToString_Always_ReturnsCorrectRepresentation()
    {
        var guid = Guid.NewGuid();
        var destination = Guid.NewGuid();
        var sender = Guid.NewGuid();
        var payload = new SimulationManagerTest.MessageImpl(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        const double weight = 1.0;
        var socialMessage = new SocialMessage(guid, destination, sender, payload, weight);
        Assert.Equal($"SocialMessage(Guid={guid}, Destination={destination}, Sender={sender}, Payload={payload}, Weight={weight})", socialMessage.ToString());
    }
}