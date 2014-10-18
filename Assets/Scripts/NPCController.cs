using UnityEngine;
using System.Collections;

public class NPCController : MonoBehaviour 
{
  //  public bool ReverseEnable = false;
    public bool Enabled = true;
    public string UniqueObjectName;

    public DialogueInstance EnabledDialogue;
    public DialogueInstance DisabledDialogue;

	// Use this for initialization
	void Start ()
    {
        EnabledDialogue.enabled = false;
        DisabledDialogue.enabled = false;

        string hashKey = UniqueObjectName;

        object value = GameState.Instance.GetValue(hashKey);
        if (value != null)
        {
            Enabled = (bool)value;
        }
        else
        {
            GameState.Instance.AddObject(hashKey, Enabled);
        }


      //  if (ReverseEnable)
      //      Enabled = !Enabled;
    }

    public bool DialogEnabled()
    {
        bool dialogEnabled = false;
        if (EnabledDialogue != null)
        {
            dialogEnabled |= EnabledDialogue.enabled;
        }
        if (DisabledDialogue != null)
        {
            dialogEnabled |= DisabledDialogue.enabled;
        }
        return dialogEnabled;
    }

    public void Action()
    {
        if (Enabled && EnabledDialogue != null)
        {
            EnabledDialogue.enabled = true;
        }
        else if (!Enabled && DisabledDialogue != null)
        {
            DisabledDialogue.enabled = true;
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
