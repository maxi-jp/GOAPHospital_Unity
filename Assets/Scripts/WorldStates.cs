using System.Collections.Generic;

[System.Serializable]
public class WorldState
{
    public string key;
    public int value;
}

public class WorldStates
{
    public Dictionary<string, int> states;

    public WorldStates()
    {
        states = new Dictionary<string, int>();
    }

    public bool HasState(string key)
    {
        return states.ContainsKey(key);
    }

    public void AddState(string key, int value)
    {
        states.Add(key, value);
    }

    public Dictionary<string, int> GetStates()
    {
        return states;
    }

    public void SetState(string key, int value)
    {
        if (states.ContainsKey(key))
            states[key] = value;
        else
            AddState(key, value);
    }

    public void ModifyState(string key, int value)
    {
        if (states.ContainsKey(key))
        {
            states[key] += value;
            if (states[key] <= 0)
                RemoveState(key);
        }
        else
            AddState(key, value);
    }

    public bool RemoveState(string key)
    {
        if (states.ContainsKey(key))
        {
            states.Remove(key);
            return true;
        }
        else
            return false;
    }
}
