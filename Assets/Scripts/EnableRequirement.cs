using UnityEngine;
using System.Collections;

public class EnableRequirement : MonoBehaviour 
{
    public string Resource = "";
    public int Amount = 0;
    public bool DisableAtAmount = false;


	// Use this for initialization
	void Start () 
    {
        Hashtable resources = (Hashtable)GameState.Instance.GetValue("PlayerResources");

        gameObject.SetActive(DisableAtAmount);
        if (resources.Contains(Resource))
        {
            if (((int)resources[Resource]) >= Amount)
            {
                gameObject.SetActive(!DisableAtAmount);
            }
        }
	}
}
