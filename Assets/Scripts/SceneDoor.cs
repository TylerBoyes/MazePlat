using UnityEngine;
using System.Collections;

public class SceneDoor : MonoBehaviour
{
    public string LockType = "Lock";
    public string RequiredKey = "";
    public bool TouchActivated = false;
    public string NextLevel;
    public string Arguments;
    public Vector2 location;
    public Color LockColour = Color.white;
    bool doorActive = true;
    string hashKey;
    

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

        if (!unlocked && RequiredKey.Length > 0)
        {
            if (!player.GetComponent<PlayerInput>().UseItem(RequiredKey))
                return;
            unlocked = true;
            DestroyObject(lockObject);
            GameState.Instance.Replace(hashKey, true);  
        }

        GameWideVariables.arguments = Arguments;
        GameWideVariables.location = location;        

        Application.LoadLevel(NextLevel);
        doorActive = false;
    }
}
