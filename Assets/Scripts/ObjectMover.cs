using UnityEngine;
using System.Collections;
[RequireComponent(typeof(PlayerInput))]

public class ObjectMover : MonoBehaviour
{

    public GameObject[] SwitchableObjects;

    public float TimeBetweenSwitching = 0.2f;
    private float lastTimeSinceSwitch = 0;
    public float speed = 40;
    private SwitchState state;
    public int NumberOfSwitching = 10;
    public float TimeStartMoving = 5;

    int objectToSwitch1;
    int objectToSwitch2;
    private Vector3 objOneTarget;
    private Vector3 objTwoTarget;
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
        if (SwitchableObjects.Length <= 1)
            return;

        if (state == SwitchState.GettingObjects)
        {
            
            int numberOfObjects = SwitchableObjects.Length;
            objectToSwitch1 = Random.Range(0, SwitchableObjects.Length);
            objectToSwitch2 = objectToSwitch1;
            while (objectToSwitch2 == objectToSwitch1)
            {
                objectToSwitch2 = Random.Range(0, SwitchableObjects.Length);
            }

            //set vector3 target positions for tw objects
            objOneTarget = SwitchableObjects[objectToSwitch2].transform.position;
            objTwoTarget = SwitchableObjects[objectToSwitch1].transform.position;

            state = SwitchState.Switching;
        }
        else if (state == SwitchState.Switching)
        {
            setNewPosition(objectToSwitch1, objOneTarget);
            setNewPosition(objectToSwitch2, objTwoTarget);
            if (getCuurentPosition(objectToSwitch1) == objOneTarget)
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

}



enum SwitchState
{
    CountingDown,
    Switching,
    GettingObjects,
    Done
}