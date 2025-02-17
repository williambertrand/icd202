﻿using UnityEngine;

public class MovingSphere : MonoBehaviour
{

	[SerializeField, Range(0f, 100f)]
	float maxSpeed = 10f;

	[SerializeField, Range(0f, 100f)]
	float minSpeed = 5.0f;

	[SerializeField, Range(0f, 100f)]
	float maxAcceleration = 14f;

	Vector3 velocity, desiredVelocity, lastPosition;

	Rigidbody body;
	PlayerController player;

	public float EnergyUse = 10.0f;

	void Awake()
	{
		body = GetComponent<Rigidbody>();
		player = GetComponent<PlayerController>();
	}

    private void Start()
    {
		lastPosition = transform.position;
    }

    void Update()
	{
		Vector2 playerInput;
		playerInput.x = Input.GetAxis("Horizontal");
		playerInput.y = Input.GetAxis("Vertical");
		playerInput = Vector2.ClampMagnitude(playerInput, 1f);
		float energyFactor = (player.CurrentEnergy / PlayerController.MAX_ENERGY);
		Mathf.Lerp(minSpeed, maxSpeed, energyFactor);
		desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed * energyFactor;

		float travelDist = Vector3.Distance(transform.position, lastPosition);

		if (player.playerstate != PlayerState.SAFE)
        {
			player.currentTripDist += travelDist;

			float energy = EnergyUse * Time.deltaTime * travelDist;
			player.UseEnergy(energy);
		}
		lastPosition = transform.position;

	}

	void FixedUpdate()
	{

		velocity = body.velocity;
		float maxSpeedChange = maxAcceleration * Time.deltaTime;
		velocity.x =
			Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
		velocity.z =
			Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

		body.velocity = velocity;

	}
}