using UnityEngine;
using System.Collections;
[RequireComponent(typeof(PlayerInput))]

public class TwoPairObjectMover : MonoBehaviour
{

    public GameObject[] SwitchableObjects;

    public float TimeBetweenSwitching = 0.2f;
    private float lastTimeSinceSwitch = 0;
    public float speed = 40;
    private SwitchState state;
    public int NumberOfSwitching = 10;
    public float TimeStartMoving = 5;

    int objectToSwitch1a = 0;
    int objectToSwitch1b = 1;
    int objectToSwitch2a = 2;
    int objectToSwitch2b = 3;
    private Vector3 objOneATarget;
    private Vector3 objOneBTarget;
    private Vector3 objTwoATarget;
    private Vector3 objTwoBTarget;
    private PlayerInput playerInput;

    // Use this for initialization
    void Start()
    {
        lastTimeSinceSwitch = TimeStartMoving;
        state = SwitchState.CountingDown;
        playerInput = GetComponent<PlayerInput>();
        playerInput.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        // if we have only have one object, it cant really trade places with anything..
        if (SwitchableObjects.Length <= 3)
            return;

        if (state == SwitchState.GettingObjects)
        {
            SwitchableObjects = shuffleArray(SwitchableObjects);

            //set vector3 target positions for two objects
            objOneATarget = SwitchableObjects[objectToSwitch1b].transform.position;
            objOneBTarget = SwitchableObjects[objectToSwitch1a].transform.position;
            objTwoATarget = SwitchableObjects[objectToSwitch2b].transform.position;
            objTwoBTarget = SwitchableObjects[objectToSwitch2a].transform.position;

            state = SwitchState.Switching;
        }
        else if (state == SwitchState.Switching)
        {
            setNewPosition(objectToSwitch1a, objOneATarget);
            setNewPosition(objectToSwitch1b, objOneBTarget);
            setNewPosition(objectToSwitch2a, objTwoATarget);
            setNewPosition(objectToSwitch2b, objTwoBTarget);
            if (getCuurentPosition(objectToSwitch1a) == objOneATarget
                && getCuurentPosition(objectToSwitch2a) == objTwoATarget)
            {
                NumberOfSwitching--;
                if (NumberOfSwitching > 0)
                {
                    state = SwitchState.CountingDown;
                    lastTimeSinceSwitch = TimeBetweenSwitching;
                }
                else
                {
                    state = SwitchState.Done;
                    playerInput.enabled = true;
                }
            }
        }
        else if (state == SwitchState.CountingDown)
        {
            if (lastTimeSinceSwitch < 0)
            {
                lastTimeSinceSwitch = TimeBetweenSwitching;
                state = SwitchState.GettingObjects;
            }
            else
            {
                lastTimeSinceSwitch -= Time.deltaTime;
            }
        }
        else
        {
            return;
        }
    }

    private void setNewPosition(int objectToMove, Vector3 target)
    {
        Vector3 currentPosition = getCuurentPosition(objectToMove);
        SwitchableObjects[objectToMove].transform.position = Vector3.MoveTowards(currentPosition, target, speed * Time.deltaTime);
    }

    private Vector3 getCuurentPosition(int objectIndex)
    {
        return SwitchableObjects[objectIndex].transform.position;
    }

    private GameObject[] shuffleArray(GameObject[] array)
    {
        System.Random random = new System.Random();
        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            GameObject tmp = array[k];
            array[k] = array[n];
            array[n] = tmp;
        }
        return array;
    }

}
