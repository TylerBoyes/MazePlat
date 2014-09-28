using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(MenuVariables))]
public class GameMenu : MonoBehaviour 
{
    public GameObject sceneVariablesObject;
    public string SceneToLoad;
    public bool QuitOnClick = false;

    public string Arguments;
    public Vector2 location;

    Color color;
    TextMesh textMesh;
    MenuVariables sceneVariables;

	// Use this for initialization
	void Start () 
    {
        textMesh = GetComponent<TextMesh>();
        color = textMesh.color;
        sceneVariables = sceneVariablesObject.GetComponent<MenuVariables>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseEnter()
    {
        SetActive();
    }

    public void SetActive()
    {
        textMesh.color = Color.yellow;
    }

    void OnMouseExit()
    {
        textMesh.color = color;
    }

    public void SetInactive()
    {
        textMesh.color = color;
    }

    public void Press()
    {
        if (QuitOnClick)
            Application.Quit();
        else
        {
            gameObject.SendMessage("Activate");
            Application.LoadLevel(SceneToLoad);
            GameWideVariables.playerArguments = Arguments;
            GameWideVariables.location = location;   

        }
    }

    void OnMouseUpAsButton()
    {
        Press();
    }
}
