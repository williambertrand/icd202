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

    private Rigidbody rigidBody;

    public float MoveSpeed = 10.0f;
    private Vector3 moveDir;

    public BirdState state;
    public bool hasTarget;

    void Start()
    {
        startingPositon = transform.position;
        roamPosition = GetRoamingPosition();
        rigidBody = GetComponent<Rigidbody>();
        moveDir = Vector3.zero;
    }

    private void Update()
    {

        switch (state)
        {
            default:
            case BirdState.ROAMING:
                if (Vector3.Distance(transform.position, roamPosition) > 0.05f)
                {
                    transform.LookAt(roamPosition);
                    moveDir = (roamPosition - transform.position).normalized;
                }
                else
                {
                    roamPosition = GetRoamingPosition();
                }
                break;
            case BirdState.DIVING:
                if (Vector3.Distance(transform.position, divePosition) > 0.05f)
                {
                    transform.LookAt(roamPosition);
                    moveDir = (roamPosition - transform.position).normalized;
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
        rigidBody.velocity = moveDir * MoveSpeed;
    }

    private void FindTarget()
    {
        float targetRange = GetRangeByState();
        if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) <= targetRange)
        {
            hasTarget = true;
        }
    }


    private float GetRangeByState()
    {
        switch (state)
        {
            case BirdState.ROAMING:
                return 50.0f;
            case BirdState.GROUNDED:
                return 10.0f;
            default:
                return 0.0f;
        }
    }

}
