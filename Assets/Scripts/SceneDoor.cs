using UnityEngine;
using System.Collections;

public class SceneDoor : MonoBehaviour
{
    public float FadeTime = 0.5f;
    public bool FreezePlayer = true;
    public string LockType = "Lock";
    public string RequiredKey = "";
    public string NextLevel;
    public bool TouchActivated = false;
    public string Arguments;
    public Vector2 location;
    public Color LockColour = Color.white;
    bool doorActive = true;
    string hashKey;

    public string MazeArguments = "";
    public Vector2 EntryDoorLocation;
    public string EntryDoorDestination = "";
    

    bool unlocked = false;
    GameObject lockObject;

    public void Start()
    {
        hashKey = Application.loadedLevelName + NextLevel + RequiredKey + transform.position;
        
        object value = GameState.Instance.GetValue(hashKey);
        if (value != null)
        {
            if ((bool)value)
            {
                unlocked = true;
            }
        }
        else
        {
            GameState.Instance.AddObject(hashKey, false);
        }

        //make a lock object if the door needs  akey
        if (RequiredKey.Length > 0 && !unlocked)
        {
            lockObject = (GameObject)Instantiate(Resources.Load(LockType));
            lockObject.transform.position = transform.position;
            lockObject.GetComponent<SpriteRenderer>().color = LockColour;
        }
    }

    public void Activate(GameObject player)
    {
        if (!doorActive) return;

        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null)
        {
            audio.Play();
        }

        if (!unlocked && RequiredKey.Length > 0)
        {
            if (!player.GetComponent<PlayerInput>().UseItem(RequiredKey))
                return;
            unlocked = true;
            DestroyObject(lockObject);
            GameState.Instance.Replace(hashKey, true);  
        }

        GameWideVariables.playerArguments = Arguments;
        GameWideVariables.location = location;       
 
        GameWideVariables.mazeArguments = MazeArguments;
        GameWideVariables.entryDoorLocation = EntryDoorLocation;
        GameWideVariables.entryDoorDestination = EntryDoorDestination;

        if (FreezePlayer)
        {
            player.GetComponent<PlayerInput>().enabled = false;
        }

        Fader.Instance.LoadLevel(NextLevel, FadeTime);
		doorActive = false;
            
    }
}
