using UnityEngine;
using System.Collections;

public class GameState 
{
    private Hashtable lookupTable;
    private static GameState instance;

    public static GameState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameState();
            }

            return instance;
        }
    }

    public GameState()
    {
        lookupTable = new Hashtable();
    }

    public void AddObject(object key, object value)
    {
        lookupTable.Add(key, value);
    }

    public bool ContainsKey(object key)
    {
        return lookupTable.ContainsKey(key);
    }

    public object GetValue(object key)
    {
        if (lookupTable.ContainsKey(key))
            return lookupTable[key];
        return null;
    }

    public void Replace(object key, object value)
    {
        lookupTable.Remove(key);
        lookupTable.Add(key, value);
    }
}
