using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerInput : MonoBehaviour
{
    public float gravity = 22;
    public float speed = 8;
    public float acceleration = 0.5f;
    public float jumpPower = 12;

    public float framesPerSecond = 10;
    public Sprite[] walkingUpSprites;
    public Sprite[] walkingDownSprites;
    public Sprite[] walkingRightSprites;
    public Sprite[] walkingLeftSprites;
    int activeSide = 0;

    Vector2 currentSpeed;
    Vector2 targetSpeed;

    float minAnimationSpeed = 0.5f;

    Vector2 amountToMove;
    PlayerPhysics playerPhysics;

    SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start()
    {
        playerPhysics = GetComponent<PlayerPhysics>();

        spriteRenderer = renderer as SpriteRenderer;// GetComponent<SpriteRenderer>();
//        spriteRenderer.animation.playAutomatically = true;

        amountToMove = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        targetSpeed = Vector2.zero;

        float effectiveSpeed = speed;
        targetSpeed.x = Input.GetAxisRaw("Horizontal") * effectiveSpeed;
        targetSpeed.y = Input.GetAxisRaw("Vertical") * effectiveSpeed;

        if (playerPhysics.hitVerticle)
        {
            amountToMove.y = 0;
        }

        if (playerPhysics.grounded)
        {
            if (Input.GetButton("Jump"))
            {
                amountToMove.y = jumpPower;
            }
        }

        if (playerPhysics.hitRoof)
        {
            amountToMove.y = 0;
        }

        if (playerPhysics.hitHorizontal)
        {
            currentSpeed.x = 0;
        }

        float effectiveAcceleration = acceleration;
        if (playerPhysics.onSolidGround)
            effectiveAcceleration *= 2;

        currentSpeed.x = Mathf.MoveTowards(currentSpeed.x, targetSpeed.x, effectiveAcceleration);
        currentSpeed.y = Mathf.MoveTowards(currentSpeed.y, targetSpeed.y, effectiveAcceleration);

        amountToMove.x = currentSpeed.x;

        if (!playerPhysics.onSolidGround)
        {
            amountToMove.y -= gravity * Time.deltaTime;
        }
        else
        {
            amountToMove.y = currentSpeed.y;
        }

        if (playerPhysics.Teleported)
        {
            targetSpeed = Vector2.zero;
            currentSpeed = Vector2.zero;
            amountToMove = Vector2.zero;
        }

        playerPhysics.Move(amountToMove * Time.deltaTime);

        

        SetupAnimation();

    }

    void SetupAnimation()
    {
        bool animationActive = playerPhysics.onSolidGround || playerPhysics.grounded;
        if (!animationActive)
            return;

        if (currentSpeed.x > minAnimationSpeed)
        {
            int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
            index = index % walkingUpSprites.Length;
            spriteRenderer.sprite = walkingRightSprites[index];
            activeSide = 2;
        }
        else if (-currentSpeed.x > minAnimationSpeed)
        {
            int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
            index = index % walkingUpSprites.Length;
            spriteRenderer.sprite = walkingLeftSprites[index];
            activeSide = 3;
        }
        else if (currentSpeed.y > minAnimationSpeed)
        {
            int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
            index = index % walkingUpSprites.Length;
            spriteRenderer.sprite = walkingUpSprites[index];
            activeSide = 1;
        }
        else if (-currentSpeed.y > minAnimationSpeed)
        {
            int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
            index = index % walkingUpSprites.Length;
            spriteRenderer.sprite = walkingDownSprites[index];
            activeSide = 0;
        }
        else
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
