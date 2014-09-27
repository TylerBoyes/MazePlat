using UnityEngine;
using System.Collections;

public class PersistantBlockToggler : MonoBehaviour 
{
    public string HashKeyToToggle;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void Activate()
    {
        object value = GameState.Instance.GetValue(HashKeyToToggle);
        if (value != null)
        {
            GameState.Instance.Replace(HashKeyToToggle, !(bool)value);
        }
    }
}
