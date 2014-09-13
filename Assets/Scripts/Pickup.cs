using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour 
{
    public LayerMask collisionMask;
    SpriteRenderer renderer;
    BoxCollider collider;

    Vector2 position;
    Ray ray;

	// Use this for initialization
	void Start () 
    {
        position = Vector2.zero;
        ray = new Ray(Vector2.zero, new Vector2(0, 1f));
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        position.x = transform.position.x;
        position.y = transform.position.y-1.1f;

        ray.origin = position;
        Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 0.75f, collisionMask))
        {

            hit.collider.gameObject.SendMessage("HitTreasure");
            audio.Play();
            Destroy(renderer);
            Destroy(this);
        }
	}
}
