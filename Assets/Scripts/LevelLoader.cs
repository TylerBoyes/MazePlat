using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour 
{
    public GameObject player; //It needs the player!
	// Use this for initialization
	void Start () 
    {
        if (GameWideVariables.arguments == "SpawnAt")
        {
            player.transform.position = GameWideVariables.location;
            player.rigidbody2D.position = GameWideVariables.location;
        }

        GameWideVariables.arguments = "";
        GameWideVariables.location = Vector2.zero;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
