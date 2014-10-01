using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnableRequirement : MonoBehaviour 
{
    public string Resource = "";
    public int Amount = 0;
    public bool DisableAtAmount = false;

    public string Item = "";

    // Use this for initialization
    void Start()
    {
        gameObject.SetActive(DisableAtAmount);

        Hashtable resources = (Hashtable)GameState.Instance.GetValue("PlayerResources");
        List<Pickup> inventory = (List<Pickup>)GameState.Instance.GetValue("PlayerInventory");

        bool hasResource = Resource == "" || (resources.Contains(Resource) && ((int)resources[Resource]) >= Amount);
        bool hasItem = Item == "";
        foreach (Pickup p in inventory)
            if (p.Item == Item)
                hasItem = true;


        if (hasResource && hasItem)
        {
            gameObject.SetActive(!DisableAtAmount);
        }
    }
}
