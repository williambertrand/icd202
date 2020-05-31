using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{

	[SerializeField]
	Rect WorldBounds = new Rect(-5f, -5f, 10f, 10f);

    [SerializeField]
	Transform[] obstaclePrefabs;

    [SerializeField]
	private float[] obstacleYScales;

	[Range(0.5f, 4.0f)]
	public float obstacleDensity = 0.5f; //Obstacle per square unit

	private const int unitToScaleFactor = 10;

    public float obstacleProbability = 0.5f; //Disctribution of Blocking vs Hiding obstacles

    public const float minDistance = 5.0f;

	Transform obstacleContainer;

	int numObstacles;
	Transform[] obstacles;

	private const int BLOCKING = 0;
	private const int HIDING = 1;

	// SHELLS
	public float shellUnderHidingProbability = 0.3f;
	public ShellItem shellPrefab;
	Transform shellContainer;
	public PlayerController playerReference;


	private void Awake()
    {
		numObstacles = (int)(
			WorldBounds.width *
			WorldBounds.height *
			obstacleDensity /
            unitToScaleFactor
		);
		ClearObstacles();
		ClearShells();
		obstacles = new Transform[numObstacles];
		ApplyObstacles();
    }

    public void ClearObstacles()
	{
		if (obstacleContainer)
		{
			Destroy(obstacleContainer.gameObject);
		}
		obstacleContainer = new GameObject("Obstacles Container").transform;
		obstacleContainer.SetParent(transform, false);
        obstacles = new Transform[numObstacles];
	}

    public void ClearShells()
	{
		if (shellContainer)
		{
			Destroy(shellContainer.gameObject);
		}
		shellContainer = new GameObject("Shells Container").transform;
		shellContainer.SetParent(transform, false);
	}

	public void ApplyObstacles() {

		Debug.Log("Applying World Obstacles: " + numObstacles);
		int failedCount = 0;
		for (int  i = 0; i < numObstacles; i++)
        {
            Vector3? position = GetOpenPosition(minDistance, 0.5f, i); //Todo: will neeed to sample mesh for y value if non-flat ground
            if (position != null)
            {
				int obstacleType = Random.value >= obstacleProbability ? BLOCKING : HIDING;
				Transform instance = AddObstacle((Vector3)position, obstacleType, 1.5f, 5.0f);
				obstacles[i] = instance;

                if(obstacleType == HIDING)
                {
					bool placeShell = Random.value >= shellUnderHidingProbability;
                    if (placeShell)
                    {
						AddShell((Vector3)position);
                    }
                }
			}
            else
            {
				failedCount++;
			}
		}

	}

    public void Refresh()
    {
		ClearObstacles();
		ClearShells();
		ApplyObstacles();
    }

	public Transform AddObstacle(Vector3 position, int obstacleType, float minScale, float maxScale) {
		Transform instance = Instantiate(obstaclePrefabs[obstacleType]);

		float randScale = (maxScale - minScale) * Random.value + minScale;
		Vector3 scale = new Vector3(randScale, 1.0f * obstacleYScales[obstacleType], randScale);


		//instance.localPosition = GroundMesh.SamplePoint(position); //IF we ever generate the greound mesh to have irregularities / noise then we'll need this
		instance.localScale = scale;
		position.y += obstacleType;
		instance.localPosition = position;
		instance.SetParent(obstacleContainer, false);
		return instance;
	}

    //Return null on failing to place down object
	private Vector3? GetOpenPosition(float minDistance, float yValue, int index)
    {
		Vector3 position = new Vector3(0, yValue ,0);
		position.x = WorldBounds.xMin + (Random.value * WorldBounds.width);
        position.z = WorldBounds.yMin + (Random.value * WorldBounds.height);


		for (int i = 0; i < index - 1; i++)
		{
			Transform t = obstacles[i];
			if (t != null && Vector3.Distance(t.position, position) < minDistance)
			{
				return null;
			}
		}
	
		return position;
	}

    private void AddShell(Vector3 position)
    {
		ShellItem instance = Instantiate(shellPrefab);
		position.y += instance.transform.localScale.y * 0.5f;
		instance.transform.localPosition = position;
		instance.transform.SetParent(shellContainer, false);
		instance.player = playerReference;
	}


    private void preventSpawnOverlap()
    {

    }
}
