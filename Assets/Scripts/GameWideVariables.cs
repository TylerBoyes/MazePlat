using UnityEngine;
using System.Collections;

public class GameWideVariables : MonoBehaviour
{
    [HideInInspector]
    public static string playerArguments;
    [HideInInspector]
    public static string mazeArguments;
    [HideInInspector]
    public static Vector2 location;
    [HideInInspector]
    public static Vector2 entryDoorLocation;
    [HideInInspector]
    public static string entryDoorDestination;
}
