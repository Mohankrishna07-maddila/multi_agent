namespace multiple_Agents.Memory;

public class ConversationMemory
{
    private readonly Dictionary<string, List<string>> _sessions = new();

    public void Add(string sessionId, string message)
    {
        if (!_sessions.ContainsKey(sessionId))
        {
            _sessions[sessionId] = new List<string>();
        }

        _sessions[sessionId].Add(message);
    }

    public IReadOnlyList<string> GetAll(string sessionId)
    {
        if (!_sessions.ContainsKey(sessionId))
        {
            return Array.Empty<string>();
        }

        return _sessions[sessionId].AsReadOnly();
    }
}
