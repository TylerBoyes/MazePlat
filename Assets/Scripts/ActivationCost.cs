using UnityEngine;
using System.Collections;

public class ActivationCost : MonoBehaviour 
{
    public string Resource = "";
    public int Cost = 0;

    void Activate()
    {
        Hashtable resources = (Hashtable)GameState.Instance.GetValue("PlayerResources");

        if (resources.Contains(Resource))
        {
            if (((int)resources[Resource]) >= Cost)
            {
                resources[Resource] = (int)resources[Resource] - Cost;
            }
        }
	
	}
}
