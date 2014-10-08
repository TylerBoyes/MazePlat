using UnityEngine;
using System.Collections;

public class Lightflicker : MonoBehaviour 
{
    public float FlickerAmount = 0.1f;
    float intensity;
    Light light;

	// Use this for initialization
	void Start () 
    {
        light =  GetComponent<Light>();
        intensity = light.intensity;
	}
	
	// Update is called once per frame
	void Update ()
    {
        light.intensity = intensity + (Random.value - 0.5f) * FlickerAmount;
        
    }
}
