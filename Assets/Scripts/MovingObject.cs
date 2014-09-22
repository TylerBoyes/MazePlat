using UnityEngine;
using System.Collections;
[RequireComponent(typeof(SpriteRenderer))]

public class MovingObject : MonoBehaviour {

    float minAnimationSpeed = 0.5f;

    public float framesPerSecond = 10;
    public Sprite[] walkingUpSprites;
    public Sprite[] walkingDownSprites;
    public Sprite[] walkingRightSprites;
    public Sprite[] walkingLeftSprites;
    SpriteRenderer spriteRenderer;
    Vector2 currentSpeed;
    int activeSide = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        SetupAnimation();
	}

    void SetupAnimation()
    {
        if (currentSpeed.x > minAnimationSpeed) //if the player is moving to the right
        {
            int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
            index = index % walkingUpSprites.Length;
            spriteRenderer.sprite = walkingRightSprites[index];
            activeSide = 2;
        }
        else if (-currentSpeed.x > minAnimationSpeed) // if the player is moving to the left
        {
            int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
            index = index % walkingUpSprites.Length;
            spriteRenderer.sprite = walkingLeftSprites[index];
            activeSide = 3;
        }
        else if (currentSpeed.y > minAnimationSpeed) // if the player is moving up
        {
            int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
            index = index % walkingUpSprites.Length;
            spriteRenderer.sprite = walkingUpSprites[index];
            activeSide = 1;
        }
        else if (-currentSpeed.y > minAnimationSpeed) //if the player is moving down
        {
            int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
            index = index % walkingUpSprites.Length;
            spriteRenderer.sprite = walkingDownSprites[index];
            activeSide = 0;
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
