using UnityEngine;
using System.Collections;
using System;

public class LevelLoader : MonoBehaviour
{
    GameObject player; //It needs the player! (lol, it is the player!)

	// Use this for initialization
	void Start () 
    {
        player = gameObject;
        
        if (GameWideVariables.playerArguments == "SpawnAt")
        {
            player.transform.position = GameWideVariables.location;
            player.rigidbody2D.position = GameWideVariables.location;
        }

        GameWideVariables.playerArguments = "";
        GameWideVariables.location = Vector2.zero;
	}

    bool Contains(string[] arr, string it)
    {
        foreach (string str in arr)
        {
            if (str == it)
                return true;
        }
        return false;
    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}