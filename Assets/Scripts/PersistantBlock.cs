using UnityEngine;
using System.Collections;

public class PersistantBlock : MonoBehaviour 
{
    public bool Enabled = true;
    public string UniqueObjectName;

    string hashKey;

	// Use this for initialization
	void Start ()
    {
        hashKey = UniqueObjectName;

        object value = GameState.Instance.GetValue(hashKey);
        if (value != null)
        {
            Enabled = (bool)value;
        }
        else
        {
            GameState.Instance.AddObject(hashKey, Enabled);
        }

        gameObject.SetActive(Enabled);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
