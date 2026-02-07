namespace DurableBackend.AI.Graph;

public class StateGraph<T>
{
    private readonly Dictionary<string, Func<T, Task<T>>> _nodes = new();
    private readonly Dictionary<string, string> _edges = new();
    private string? _entryPoint;
    private string? _finishPoint;

    public void AddNode(string name, Func<T, Task<T>> node)
    {
        _nodes[name] = node;
    }

    public void SetEntryPoint(string name)
    {
        _entryPoint = name;
    }

    public void AddEdge(string from, string to)
    {
        _edges[from] = to;
    }

    public void SetFinishPoint(string name)
    {
        _finishPoint = name;
    }

    public CompiledGraph<T> Compile()
    {
        return new CompiledGraph<T>(_nodes, _edges, _entryPoint, _finishPoint);
    }
}

public class CompiledGraph<T>
{
    private readonly Dictionary<string, Func<T, Task<T>>> _nodes;
    private readonly Dictionary<string, string> _edges;
    private readonly string _entryPoint;
    private readonly string? _finishPoint;

    public CompiledGraph(
        Dictionary<string, Func<T, Task<T>>> nodes,
        Dictionary<string, string> edges,
        string? entryPoint,
        string? finishPoint)
    {
        _nodes = nodes;
        _edges = edges;
        _entryPoint = entryPoint ?? throw new InvalidOperationException("Entry point must be set.");
        _finishPoint = finishPoint;
    }

    public async Task<T> InvokeAsync(T initialState)
    {
        var currentState = initialState;
        var currentNodeName = _entryPoint;

        while (true)
        {
            if (!_nodes.ContainsKey(currentNodeName))
            {
                throw new InvalidOperationException($"Node '{currentNodeName}' not found.");
            }

            var node = _nodes[currentNodeName];
            currentState = await node(currentState);

            if (currentNodeName == _finishPoint)
            {
                break;
            }

            if (_edges.ContainsKey(currentNodeName))
            {
                currentNodeName = _edges[currentNodeName];
            }
            else
            {
                // Implicit finish if no edge? Or error?
                // For this simple implementation, if we hit a node with no outgoing edge, we stop.
                break; 
            }
        }

        return currentState;
    }
}
