using System;
using System.ComponentModel;

namespace multiple_Agents.Memory;

public class ConversationMemory
{
    private readonly List<string> _history = new();

    public void Add(string message)
    {
        _history.Add(message);
    }

    public IReadOnlyList<string> GetAll()
    {
        return _history.AsReadOnly();
    }
}
//What this class does(simple)

//_history → stores messages

//Add() → write memory

//GetAll() → read memory

//This is exactly how memory starts in real systems.