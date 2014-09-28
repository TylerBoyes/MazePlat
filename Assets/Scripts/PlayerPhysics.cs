using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerPhysics : MonoBehaviour
{
    public LayerMask collisionMask;
    public LayerMask groundMask;
    public LayerMask teleporterMask;

    public EdgeCollider2D topEdge;
    public EdgeCollider2D rightEdge;
    public EdgeCollider2D leftEdge;

    public bool HalfHeight = false;

    private float skin = 0.005f;

    [HideInInspector]
    public bool grounded = false;
    [HideInInspector]
    public bool onSolidGround = false;
    [HideInInspector]
    public bool Teleported = true;

    private BoxCollider2D collider;
    private Rigidbody2D body;
    Vector2 size;
    Vector2 center;

    Vector2 forceToApply = Vector2.zero;

    bool jump;
    float jumpAmount;

    Ray2D ray;

    bool sceneStarting = true;
    float sceneTime = 0;

    // Use this for initialization
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        size = new Vector2(0.31f, 0.48f);// collider.size;
        center = collider.center;
        body = GetComponent<Rigidbody2D>();

        if (HalfHeight)
        {
            Vector2[] newPoints = new Vector2[topEdge.points.Length];
            for (int i = 0; i < topEdge.points.Length; i++)
            {
                newPoints[i] = new Vector2(topEdge.points[i].x, topEdge.points[i].y - 0.25f);
            }
            topEdge.points = newPoints;
            
            leftEdge.points = LowerTopPoint(leftEdge.points, 0.25f);
            rightEdge.points = LowerTopPoint(rightEdge.points, 0.25f);
            
        }
    }

    Vector2[] LowerTopPoint(Vector2[] points, float amount)
    {
        Vector2[] newPoints = new Vector2[points.Length];
        int topPoint = 0;
        float topY = -10;
        //copy points, while finding top point
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].y > topY)
            {
                topY = points[i].y;
                topPoint = i;
            }
            newPoints[i] = new Vector2(points[i].x, points[i].y);
        }

        //lower top point
        newPoints[topPoint] = new Vector2(newPoints[topPoint].x, newPoints[topPoint].y - amount);

        return newPoints;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        //this is to prevent the user from switching scenes while holding up real fast
        sceneTime += Time.deltaTime;
        if (sceneTime > 1.25f)
            sceneStarting = false;

        onSolidGround = false;

        LandedDetection(transform.position);
        FlatMapGroundDetection(transform.position);
        TeleporterDetection(transform.position);

        if (jump)
        {
            body.velocity = new Vector2(body.velocity.x, jumpAmount);
            jump = false;
            jumpAmount = 0;
        }
        else if (Teleported)
        {
            body.velocity = Vector2.zero;
        }
        else
        {
            body.AddForce(forceToApply);
        }
        forceToApply = Vector2.zero;

        //we manually bleed off velocity because we want horizontal friction but not veritcal
        float frictionX = 0.1f;
        float frictionY = 0.0f;
        if (onSolidGround)
        {
            frictionX = 0.2f;
            frictionY = 0.2f;
        }
        Vector2 bledVelocity = new Vector2(
            Mathf.MoveTowards(body.velocity.x, 0, frictionX),
            Mathf.MoveTowards(body.velocity.y, 0, frictionY));
        body.velocity = bledVelocity;

        if (onSolidGround)
            body.gravityScale = 0;
        else
            body.gravityScale = 1;
    }

    public void Jump(float amount)
    {
        jump = true;
        jumpAmount = amount;
    }
    public void Move(Vector2 moveAmount)
    {
        forceToApply = moveAmount;
        //Vector2 playerPosition = transform.position;

    }

    private void TeleporterDetection(Vector2 playerPosition)
    {
        Teleported = false;

        float x = playerPosition.x + center.x; 
        float y = playerPosition.y + center.y;

        float distance = 0.3f;// + Mathf.Max(0, body.velocity.magnitude - 1);
        ray = new Ray2D(new Vector2(x, y-size.y), new Vector2(0, 1));

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction,
            distance, teleporterMask);

        if (hit.collider == null)
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.green);

            Teleporter teleporter = hit.collider.gameObject.GetComponent<Teleporter>();
            bool pressing = Input.GetAxis("Vertical") != 0;

            if (teleporter != null && pressing)
            {
                bool teleport = teleporter.Activate();
                if (teleport)
                {
                    playerPosition = teleporter.GetDestination();
                    transform.position = playerPosition;
                    Teleported = true;
                }
            }
            else
            {
                SceneDoor sceneDoor = hit.collider.gameObject.GetComponent<SceneDoor>();
                if (sceneDoor && (pressing || sceneDoor.TouchActivated))
                    sceneDoor.Activate(gameObject);
            }
        }

        /*
        float x = (playerPosition.x + center.x);
        float y = (playerPosition.y + center.y);
        ray = new Ray(new Vector2(x, y - 1), new Vector2(0, 1f));
        Debug.DrawRay(ray.origin, ray.direction, Color.white);
        if (Physics.Raycast(ray, out hit, 1, teleporterMask))
        {
            bool pressing = (Input.GetAxisRaw("Vertical") != 0 || Input.GetButton("Jump"));
            Teleporter teleporter = hit.collider.gameObject.GetComponent<Teleporter>();
            if (teleporter && pressing)
            {
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
            {
                SceneDoor sceneDoor = hit.collider.gameObject.GetComponent<SceneDoor>();
                if (sceneDoor && (pressing || sceneDoor.TouchActivated))
                    sceneDoor.Activate();
            }
        }
        else
            Teleported = false;
         */
    }

    private void FlatMapGroundDetection(Vector2 playerPosition)
    {
        //groundCollision
        float x = (playerPosition.x + center.x);
        float y = (playerPosition.y + center.y) - 1; //minus 1 block
        ray = new Ray2D(new Vector2(x, y), new Vector2(0, 1f));
        
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1.05f, groundMask);
        if (hit.collider != null)
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.green);
            onSolidGround = true;
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.blue);
        }
    }

    private void LandedDetection(Vector2 playerPosition)
    {
        grounded = false;
        for (int i = 0; i < 3; i++)
        {
            float x = playerPosition.x + center.x - size.x + (size.x)*i; //left/center/right
            float y = playerPosition.y + center.y - size.y - 0.15f; //bottom of collider

            float distance = 0.2f;
            ray = new Ray2D(new Vector2(x, y), new Vector2(0, -1 * distance));

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction,
                distance, collisionMask);

            if (hit.collider == null)
            {
                Debug.DrawRay(ray.origin, ray.direction, Color.blue);
            }
            else
            {
                grounded = true;
                Debug.DrawRay(ray.origin, ray.direction, Color.red);
            }
        }
    }
}
