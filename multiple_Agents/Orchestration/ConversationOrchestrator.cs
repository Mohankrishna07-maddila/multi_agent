using multiple_Agents.Agents;
using multiple_Agents.Memory;

namespace multiple_Agents.Orchestration;

public class ConversationOrchestrator
{
    private readonly ReaderAgent _reader;
    private readonly ResponderAgent _responder;
    private readonly ConversationMemory _memory;

    public ConversationOrchestrator(
        ReaderAgent reader,
        ResponderAgent responder,
        ConversationMemory memory)
    {
        _reader = reader;
        _responder = responder;
        _memory = memory;
    }

    public object Run(string sessionId, string input)
    {
        // Step 1: Save state
        _memory.Add(sessionId, input);

        // Step 2: Activity 1
        var context = _reader.Read(input);

        // Step 3: Activity 2
        var result = _responder.Respond(context);

        // Step 4: Return durable state
        return new
        {
            sessionId,
            result,
            memory = string.Join(" | ", _memory.GetAll(sessionId))
        };
    }
}
