using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class PlayerPhysics : MonoBehaviour
{
    public LayerMask collisionMask;
    public LayerMask groundMask;
    public LayerMask teleporterMask;


    private float skin = 0.005f;

    [HideInInspector]
    public bool grounded;
    [HideInInspector]
    public bool hitRoof;
    [HideInInspector]
    public bool hitHorizontal;
    [HideInInspector]
    public bool hitVerticle;
    [HideInInspector]
    public bool onSolidGround;
    [HideInInspector]
    public bool Teleported = true;

    private BoxCollider collider;
    private Vector2 size;
    private float sizeLength;
    private Vector2 center;

    Ray ray;
    RaycastHit hit;

    // Use this for initialization
    void Start()
    {
        collider = GetComponent<BoxCollider>();
        size = collider.size;
        sizeLength = size.magnitude;
        center = collider.center;
        onSolidGround = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move(Vector2 moveAmount)
    {
        float deltaX = moveAmount.x;
        float deltaY = moveAmount.y;

        Vector2 playerPosition = transform.position;

        grounded = false;
        hitRoof = false;
        onSolidGround = false;
        hitVerticle = false;

        GroundCollision(playerPosition);
        VerticalCollision(ref deltaY, ref playerPosition);
        HorizontalCollision(ref deltaX, ref playerPosition);

        
        //corner Collision
        float x = (playerPosition.x + center.x); //left/center/right
        float y = (playerPosition.y + center.y); //bottom of collider        
        Vector2 delta = new Vector2(deltaX, deltaY);
        ray = new Ray(new Vector2(x, y), delta);
        Debug.DrawRay(ray.origin, ray.direction, Color.white);
        if (Physics.Raycast(ray, out hit, size.x/2, collisionMask))
        {
            deltaX = 0;
            deltaY = 0;
        }

        //groundCollision
        x = (playerPosition.x + center.x);
        y = (playerPosition.y + center.y);
        ray = new Ray(new Vector2(x, y - 1), new Vector2(0, 1f));
        Debug.DrawRay(ray.origin, ray.direction, Color.white);
        if (Physics.Raycast(ray, out hit, 1, teleporterMask))
        {
            Teleporter teleporter = hit.collider.gameObject.GetComponent<Teleporter>();
            bool teleport = teleporter.Activate();
            if (teleport)
            {
                playerPosition = teleporter.GetDestination();
                transform.position = playerPosition;
                deltaX = 0; deltaY = 0;
                Teleported = true;
            }
        }
        else
            Teleported = false;

        Vector2 finalTransform = new Vector2(deltaX, deltaY);
        transform.Translate(finalTransform);
    }

    private void GroundCollision(Vector2 playerPosition)
    {
        //groundCollision
        float x = (playerPosition.x + center.x);
        float y = (playerPosition.y + center.y) - 1; //minus 1 block
        ray = new Ray(new Vector2(x, y), new Vector2(0, 1f));
        Debug.DrawRay(ray.origin, ray.direction, Color.white);
        if (Physics.Raycast(ray, out hit, 1.05f, groundMask))
        {
            onSolidGround = true;
        }
    }

    private void VerticalCollision(ref float deltaY, ref Vector2 playerPosition)
    {
        float x, y;
        float verticleDirection = Mathf.Sign(deltaY);

        //verticle collision
        for (int i = 0; i < 3 && !hitVerticle; i++)
        {
            x = (playerPosition.x + center.x - size.x / 2f) + size.x / 2f * i; //left/center/right
            y = playerPosition.y + center.y + size.y / 2f * verticleDirection; //bottom of collider

            ray = new Ray(new Vector2(x, y), new Vector2(0, verticleDirection));
            Debug.DrawRay(ray.origin, ray.direction, Color.white);

            if (Physics.Raycast(ray, out hit, Mathf.Abs(deltaY), collisionMask))
            {
                float distance = Vector2.Distance(ray.origin, hit.point);

                if (distance > skin)
                {
                    deltaY = (distance - (skin / 1.01f)) * verticleDirection;
                }
                else
                {
                    bool falling = verticleDirection < 0;
                    if (falling)
                        grounded = true;
                    else
                        hitRoof = true;
                    deltaY = 0;

                    hitVerticle = true; //we dont need to test anymore, lets leave the loop
                }
            }
        }
    }

    private void HorizontalCollision(ref float deltaX, ref Vector2 playerPosition)
    {
        float x, y;

        float horizontalDirection = Mathf.Sign(deltaX);
        hitHorizontal = false;
        // horizontal collision
        for (int i = 0; i < 3 && !hitHorizontal; i++)
        {
            float collisionSize = size.y / 1.005f;
            x = playerPosition.x + center.x + size.x / 2f * horizontalDirection; //side of collider
            y = (playerPosition.y + center.y - collisionSize / 2f) + collisionSize / 2f * i;

            ray = new Ray(new Vector2(x, y), new Vector2(horizontalDirection, 0));

            float distance = Mathf.Abs(deltaX);
            if (distance != 0 && Physics.Raycast(ray, out hit, distance, collisionMask))
            {
                Debug.DrawRay(ray.origin, ray.direction, Color.white);
                hitHorizontal = true;
                deltaX = 0;
            }
        }
    }
}
