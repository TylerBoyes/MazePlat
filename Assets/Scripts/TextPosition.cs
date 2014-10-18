using UnityEngine;
using System.Collections;

public class TextPosition : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        GetComponent<Renderer>().sortingLayerName = "OnGrassInFront";
        GetComponent<Renderer>().sortingOrder = 10;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
