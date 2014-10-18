using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerPhysics))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerInput : MonoBehaviour
{
    public float Gravity = 22;
    public float Speed = 8;
    public float Acceleration = 0.5f;
    public float JumpPower = 12;

    public float framesPerSecond = 10;
    public Sprite[] walkingUpSprites;
    public Sprite[] walkingDownSprites;
    public Sprite[] walkingRightSprites;
    public Sprite[] walkingLeftSprites;

    Direction direction = Direction.South;
    public Direction Direction { get { return direction; } set { direction = value; } }

    int activeSide = 0;

    public int items = 0;

    Vector2 targetSpeed;

    float jumpTime = 0;

    float minAnimationSpeed = 0.5f;

    Vector2 currentSpeed;
    Vector2 amountToMove;
    PlayerPhysics playerPhysics;

    SpriteRenderer spriteRenderer;
    List<Pickup> inventory;
    Hashtable resources;
    string inventoryHashKey;
    string resourcesHashKey;

    NPCController activeNPCDialog;

    public List<Pickup> Inventory
    {
        get { return inventory; }
    }

    // Use this for initialization
    void Start()
    {
        inventoryHashKey = "PlayerInventory";

        object value = GameState.Instance.GetValue(inventoryHashKey);
        if (value != null)
        {
            inventory = (List<Pickup>)value;
        }
        else
        {
            inventory = new List<Pickup>();
            GameState.Instance.AddObject(inventoryHashKey, inventory);
        }

        resourcesHashKey = "PlayerResources";
        value = GameState.Instance.GetValue(resourcesHashKey);
        if (value != null)
        {
            resources = (Hashtable)value;
        }
        else
        {
            resources = new Hashtable();
            GameState.Instance.AddObject(resourcesHashKey, resources);
        }



        playerPhysics = GetComponent<PlayerPhysics>();

        spriteRenderer = renderer as SpriteRenderer;// GetComponent<SpriteRenderer>();
//        spriteRenderer.animation.playAutomatically = true;

        amountToMove = Vector2.zero;
    }

    bool setNpcAction = false;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (activeNPCDialog != null)
        {
            if (activeNPCDialog.DialogEnabled())
                return;
        }

        //Get input for starting NPC dialogs:
        if (setNpcAction)
        {
            setNpcAction = false;
            activeNPCDialog.Action();
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");

            //get closes npc:
            if (npcs.Length > 0)
            {
                GameObject closestNPC = null;
                float closest = 2.00f; // min distance
                foreach (GameObject npc in npcs)
                {
                    float distance = Vector3.Distance(transform.position, npc.transform.position);
                    if (distance < closest)
                    {
                        closestNPC = npc;
                        closest = distance;
                    }
                }
                if (closestNPC != null)
                {
                    activeNPCDialog = closestNPC.GetComponent<NPCController>();
                    setNpcAction = true;
                }
            }
        }

        targetSpeed = Vector2.zero;

        targetSpeed.x = Input.GetAxisRaw("Horizontal");
        if (playerPhysics.onSolidGround)
            targetSpeed.y = Input.GetAxisRaw("Vertical");

        amountToMove = targetSpeed;

        if (jumpTime <= 0)
        {
            if (playerPhysics.grounded && !playerPhysics.onSolidGround)
            {
                if (Input.GetButton("Jump"))
                {
                    playerPhysics.Jump(JumpPower);
                    jumpTime = 0.2f;
                }
            }
        }
        else
            jumpTime -= Time.deltaTime;

        if (Mathf.Abs(playerPhysics.rigidbody2D.velocity.x) >= Speed)
            amountToMove.x = 0;
        if (Mathf.Abs(playerPhysics.rigidbody2D.velocity.y) >= Speed)
            amountToMove.y = 0;

        playerPhysics.Move(amountToMove * 1000 * Time.deltaTime * Acceleration);

        currentSpeed = playerPhysics.rigidbody2D.velocity;
    }

    void Update()
    {
        SetupAnimation();
    }

    public void AddItem(object item)
    {
        items++;

        if (typeof(Pickup) == item.GetType())
        {
            Pickup pickup = (Pickup)item;

            if (pickup.IsResource)
            {
                if (resources.Contains(pickup.Item))
                {
                    string key = pickup.Item;
                    int value = ((int)resources[key]) + pickup.amount;                   

                    resources.Remove(key);
                    resources.Add(key, value);
                }
                else
                {
                    resources.Add(pickup.Item, pickup.amount);
                }
            }
            else
            {
                DontDestroyOnLoad(pickup.gameObject);
                inventory.Add(pickup);
            }
        }
    }

    public bool UseItem(string item)
    {
        Pickup usedItem = null;
        foreach (Pickup pickup in Inventory)
        {
            if (pickup.Item == item)
            {
                usedItem = pickup;
                break;
            }
        }
        if (usedItem != null)
        {
            inventory.Remove(usedItem);
            DestroyObject(usedItem.gameObject);
            return true;
        }
        return false;
    }

    void SetupAnimation()
    {
        bool animationActive = playerPhysics.onSolidGround || playerPhysics.grounded; //so basically if the player isn:t jumping or falling
        if (!animationActive)
            return;

        bool inDialog = (activeNPCDialog != null && activeNPCDialog.DialogEnabled());
        if (currentSpeed.x > minAnimationSpeed && !inDialog) //if the player is moving to the right
        {
            int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
            index = index % walkingUpSprites.Length;
            spriteRenderer.sprite = walkingRightSprites[index];
            activeSide = 2;
            direction = Direction.East;
        }
        else if (-currentSpeed.x > minAnimationSpeed && !inDialog) // if the player is moving to the left
        {
            int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
            index = index % walkingUpSprites.Length;
            spriteRenderer.sprite = walkingLeftSprites[index];
            activeSide = 3;
            direction = Direction.West;
        }
        else if (currentSpeed.y > minAnimationSpeed && !inDialog) // if the player is moving up
        {
            int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
            index = index % walkingUpSprites.Length;
            spriteRenderer.sprite = walkingUpSprites[index];
            activeSide = 1;
            direction = Direction.North;
        }
        else if (-currentSpeed.y > minAnimationSpeed && !inDialog) //if the player is moving down
        {
            int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
            index = index % walkingUpSprites.Length;
            spriteRenderer.sprite = walkingDownSprites[index];
            activeSide = 0;
            direction = Direction.South;
        }
        else // if the player is not moving, show the first frame of the animation. its the resting frame
        {
            switch (activeSide)
            {
                case 0:
                    spriteRenderer.sprite = walkingDownSprites[0];
                    break;
                case 1:
                    spriteRenderer.sprite = walkingUpSprites[0];
                    break;
                case 2:
                    spriteRenderer.sprite = walkingRightSprites[0];
                    break;
                case 3:
                    spriteRenderer.sprite = walkingLeftSprites[0];
                    break;

            }
        }
    }
}

public enum Direction
{
    North, 
    East,
    South,
    West
}
