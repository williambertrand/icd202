using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BirdState
{
    ROAMING,
    DIVING,
    GROUNDED,
    CLIMBING
}

[RequireComponent(typeof(Rigidbody))]
public class BirdEnemy : MonoBehaviour
{
    private Vector3 startingPositon;
    private Vector3 roamPosition;
    private Vector3 divePosition;

    private const float REACH_DIST = 1.0f;
    private const int BIRD_DAMAGE = 1;

    //For testing
    public float DistToPlayer = 0.0f;

    private Rigidbody rigidBody;

    public const float FlySpeed = 15.0f;
    public const float GroundSpeed = 7.5f;
    public const float DiveSpeed = 25.0f;




    private float moveSpeed;

    private Vector3 moveDir;

    public BirdState state;
    public bool hasTarget;
    private Vector3 targetPosition; 

    void Start()
    {
        startingPositon = transform.position;
        roamPosition = GetRoamingPosition();
        rigidBody = GetComponent<Rigidbody>();
        moveDir = Vector3.zero;
        moveSpeed = FlySpeed;

        //Start us off as roaming birds
        transform.LookAt(roamPosition);
        moveDir = (roamPosition - transform.position).normalized;
    }

    private void Update()
    {

        CheckForTarget();

        switch (state)
        {
            default:
            case BirdState.CLIMBING:
                if (Vector3.Distance(transform.position, roamPosition) < REACH_DIST)
                {
                    state = BirdState.ROAMING;
                }
                break;
            case BirdState.ROAMING:
                if(hasTarget)
                {
                    state = BirdState.DIVING;
                    divePosition = PlayerController.Instance.transform.position;
                    moveSpeed = DiveSpeed;
                    transform.LookAt(divePosition);
                    moveDir = (divePosition - transform.position).normalized;
                    break;
                }
                if (Vector3.Distance(transform.position, roamPosition) < REACH_DIST)
                {
                    roamPosition = GetRoamingPosition();
                }
                transform.LookAt(roamPosition);
                moveDir = (roamPosition - transform.position).normalized;
                break;
            case BirdState.DIVING:
                if (Vector3.Distance(transform.position, divePosition) < REACH_DIST)
                {
                    //Reached dive position
                    state = BirdState.GROUNDED;
                    moveSpeed = GroundSpeed;
                    state = BirdState.GROUNDED;
                    transform.LookAt(targetPosition);
                    moveDir = (targetPosition - transform.position).normalized;
                }
                break;
            case BirdState.GROUNDED:
                if (Vector3.Distance(transform.position, targetPosition) < REACH_DIST)
                {
                    //Reached target position
                }
                break;


        }
    }

    private Vector3 GetRoamingPosition()
    {
        return startingPositon + Utils.GetRandomDirection() * Random.Range(30f, 60f);
    }

    private void FixedUpdate()
    {
        rigidBody.velocity = moveDir * moveSpeed;
    }

    private void CheckForTarget()
    {
        DistToPlayer = Vector3.Distance(transform.position, targetPosition);
        float targetRange = GetRangeByState();
        if (DistToPlayer <= targetRange && PlayerController.Instance.playerstate == PlayerState.IN_OPEN)
        {
            hasTarget = true;
            targetPosition = PlayerController.Instance.transform.position;
        }
        else
        {
            hasTarget = false;
            //Lost target
            TransitionToLostTargetState();
        }
    }


    private float GetRangeByState()
    {
        switch (state)
        {
            case BirdState.ROAMING:
                return 24.0f;
            case BirdState.DIVING:
                return 15.0f;
            case BirdState.GROUNDED:
                return 4.0f;
            default:
                return 0.0f;
        }
    }

    private void TransitionToLostTargetState()
    {
        switch (state)
        {
            case BirdState.DIVING:
                state = BirdState.ROAMING;
                return;
            case BirdState.GROUNDED:
                state = BirdState.CLIMBING;
                return;
            default:
                return;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.TakeDamage(BIRD_DAMAGE);
        }
    }


}
