using UnityEngine;
using System.Collections;

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
    int activeSide = 0;

    Vector2 targetSpeed;

    float jumpTime = 0;

    float minAnimationSpeed = 0.5f;

    Vector2 currentSpeed;
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
    void FixedUpdate()
    {
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
