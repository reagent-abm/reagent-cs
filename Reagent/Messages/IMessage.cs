using Reagent.Properties;

namespace Reagent.Messages;

/// <summary>
/// A message is a piece of data that is sent between <c>agent</c>s.
/// </summary>
public interface IMessage : IGuidD
{
    /// <summary>
    /// The <c>Guid</c> of the <c>Agent</c> that the message is sent to.
    /// </summary>
    Guid Destination { get; }
    
    /// <summary>
    /// The <c>Guid</c> of the <c>Agent</c> that sent the message.
    /// </summary>
    Guid Sender { get; }
}