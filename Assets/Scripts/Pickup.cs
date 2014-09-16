using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour 
{
    public LayerMask collisionMask;
    SpriteRenderer renderer;
    BoxCollider collider;

    Vector2 position;
    Ray2D ray;

	// Use this for initialization
	void Start () 
    {
        position = Vector2.zero;
        ray = new Ray2D(Vector2.zero, new Vector2(0, 1f));
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider>();
	}

    void Activate()
    {
        audio.Play();
        Destroy(renderer);
        Destroy(this);
       // Destroy(gameObject);
    }

	// Update is called once per frame
	void Update () 
    {
        position.x = transform.position.x;
        position.y = transform.position.y-0.4f;

        ray.origin = position;
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 0.75f, collisionMask);
        if (hit.collider != null)
        {
            hit.collider.gameObject.SendMessage("HitTreasure");
            Activate();
            Debug.DrawRay(ray.origin, ray.direction, Color.red);
        }
        else
            Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
	}
}
