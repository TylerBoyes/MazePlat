using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour 
{
    public Vector2 Destination;
    public bool LoadNextLevel = false;
    public string NextLevel;

    public bool LeftOf = false;
    public GameObject DestinationDoor = null;

    bool active = true;

    public bool Activate()
    {
        if (!active) return false; 

        if (LoadNextLevel)
        {
            Application.LoadLevel(NextLevel);
            active = false;
            return false;
        }
        return true;
    }

    public Vector2 GetDestination()
    {
        if (DestinationDoor != null)
        {
            Vector2 dest = DestinationDoor.transform.position;
            dest.x += 1;
            dest.y += 0.2f;

            if (LeftOf)
                dest.x -= 2f;
            return dest;
        }
        else
            return Destination;
    }
}
