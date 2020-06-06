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
	Transform poolPrefab;

	[SerializeField]
	Transform[] barnaclePrefabs;

	[SerializeField]
	private float[] obstacleYScales;

	[Range(0.5f, 4.0f)]
	public float obstacleDensity = 0.5f; //Obstacle per square unit

	private const int unitToScaleFactor = 10;

    public float obstacleTypeProbability = 0.5f; //Disctribution of Blocking vs Hiding obstacles
	public float poolProbability = 0.01f; //Distribution of pools vs obstacles

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


	public static Texture2D waterMap;
	private static float waterScale = 0.03f;


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

		int failedCount = 0;
		for (int  i = 0; i < numObstacles; i++)
        {
            Vector3? position = GetOpenPosition(minDistance, 0.5f, i); //Todo: will neeed to sample mesh for y value if non-flat ground
            if (position != null)
            {
				bool placePool = Random.value <= poolProbability;
                if(placePool)
                {
					Vector3 poolPos = (Vector3)position;
					poolPos.y = 0.1f;
					Transform poolInstance = AddPool(poolPos, 4.0f, 6.0f, 1, 3);
					obstacles[i] = poolInstance;
					continue;
                }

				int obstacleType = Random.value >= obstacleTypeProbability ? BLOCKING : HIDING;
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

    public Transform AddPool(Vector3 position, float minScale, float maxScale, int barnacleMin, int barnacleMax)
    {
		float randScale = (maxScale - minScale) * Random.value + minScale;
		Transform instance = Instantiate(poolPrefab);
		instance.localScale = new Vector3(randScale, 1, randScale);
		instance.localPosition = position;
		instance.SetParent(obstacleContainer, true);

		int numBarnacles = Random.Range(barnacleMin, barnacleMax + 1);
		float poolRadius = randScale / 2;
        //Add barnacles for the pool
        for(int i = 0; i < numBarnacles; i++) //TODO: FIX BARNACLE POS
        {
			Vector3 dir = Utils.GetRandomDirection();
			Vector3 barnaclePos = position + (poolRadius * dir);
			float barnacleScale = (2.0f) * Random.value + 1.5f;
			Transform barnalce = Instantiate(barnaclePrefabs[Random.Range(0, barnaclePrefabs.Length)]);
			barnalce.position = barnaclePos;
			barnalce.localRotation = Quaternion.Euler(45, 0, 0);
			instance.localScale = new Vector3(barnacleScale, barnacleScale, barnacleScale);
			barnalce.SetParent(instance, true);
		}

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



	public static Vector4 SampleWater(Vector3 position)
	{
		return waterMap.GetPixelBilinear(
			position.x * waterScale,
			position.z * waterScale
		);
	}
}
