using UnityEngine;
using System.Collections;

public class Flashlight : MonoBehaviour 
{
    public GameObject Player;

    PlayerInput playerInput;

    float x = 0;

	// Use this for initialization
	void Start () 
    {
        playerInput = Player.GetComponent<PlayerInput>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Player != null)
        {
            transform.position = new Vector3(
                Player.transform.position.x,
                Player.transform.position.y,
                transform.position.z);
            
        }
        if (playerInput != null)
        {
            if (playerInput.Direction == Direction.East)
            {
                transform.rotation = Quaternion.EulerAngles(0, Mathf.PI / 8, 0);
            }
            if (playerInput.Direction == Direction.West)
            {
                transform.rotation = Quaternion.EulerAngles(0, -Mathf.PI / 8, 0);
            }
            else if (playerInput.Direction == Direction.North)
            {
                transform.rotation = Quaternion.EulerAngles(-Mathf.PI / 8, 0, 0);
            }
            else if (playerInput.Direction == Direction.South)
            {
                transform.rotation = Quaternion.EulerAngles(Mathf.PI / 8, 0, 0);
            }
        }
	}
}
