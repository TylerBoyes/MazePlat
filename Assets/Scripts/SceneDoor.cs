using UnityEngine;
using System.Collections;

public class SceneDoor : MonoBehaviour
{
    public bool TouchActivated = false;
    public string NextLevel;
    public string Arguments;
    public Vector2 location;
    bool active = true;

    public void Activate()
    {
        if (!active) return;

        GameWideVariables.arguments = Arguments;
        GameWideVariables.location = location;

        

        Application.LoadLevel(NextLevel);
        active = false;
    }
}
