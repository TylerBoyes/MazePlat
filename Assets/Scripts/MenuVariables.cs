using UnityEngine;
using System.Collections;

public class MenuVariables : MonoBehaviour 
{
    public GameObject[] menuEntries;

    int activeMenu = -1;
    bool buttonPressed = false;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetAxis("Mouse X") < 0)
        {
            //Code for action on mouse moving left
            activeMenu = -1;
        }
        if (Input.GetAxis("Mouse X") > 0)
        {
            //Code for action on mouse moving right
            activeMenu = -1;
        }

        float y = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(y) > 0.1)
        {
            if (!buttonPressed)
            {
                buttonPressed = true;
                if (activeMenu >= 0)
                    menuEntries[activeMenu].GetComponent<GameMenu>().SendMessage("SetInactive");

                if (y > 0.1)
                    activeMenu++;
                else if (y < -0.1)
                    activeMenu--;

                if (activeMenu >= menuEntries.Length)
                    activeMenu = 0;
                else if (activeMenu < 0)
                    activeMenu = menuEntries.Length - 1;

                menuEntries[activeMenu].GetComponent<GameMenu>().SendMessage("SetActive");
            }
        }
        else
            buttonPressed = false;


        if (Input.GetButtonUp("Select"))
        {
            menuEntries[activeMenu].GetComponent<GameMenu>().SendMessage("Press");
        }
	}
}
