using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour
{
    public string message = "AddItem";
    public string Item = "";
    public int amount = 1;
    public bool IsResource = false;

    string hashKey;

	// Use this for initialization
	void Start () 
    {
        hashKey = Application.loadedLevelName + Item + transform.position;

        object value = GameState.Instance.GetValue(hashKey);
        if (value != null)
        {
            if ((bool)value)
            {
                DestroyObject(gameObject);
            }
        }
        else
        {
            GameState.Instance.AddObject(hashKey, false);
        }
	}

    void Activate()
    {
        GameState.Instance.Replace(hashKey, true);

        if (audio)
            audio.Play();
        GetComponent<SpriteRenderer>().enabled = false;
        DestroyObject(GetComponent<BoxCollider2D>());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.SendMessage(message, this);
            Activate();
        }
    }
    /*
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
     */
}
