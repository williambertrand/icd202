using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{

	[SerializeField]
	Rect WorldBounds = new Rect(-5f, -5f, 10f, 10f);

    [SerializeField]
	Transform[] obstaclePrefabs;

	public float obstacleDensity = 1.0f; //Obstacle per square unit
    public float obstacleProbability = 0.5f;

    public const float minDistance = 0.1f;

	Transform obstacleContainer;

	int numObstacles;
	Transform[] obstacles;

	private const int BLOCKING = 0;
	private const int HIDING = 1;


	private void Awake()
    {
		numObstacles = (int)(
			WorldBounds.width *
			WorldBounds.height *
			obstacleDensity
		);
		obstacles = new Transform[numObstacles]; 
    }

    public void ClearObstacles()
	{
		if (obstacleContainer)
		{
			Destroy(obstacleContainer.gameObject);
		}
		obstacleContainer = new GameObject("Features Container").transform;
		obstacleContainer.SetParent(transform, false);
        obstacles = new Transform[numObstacles];
	}

	public void ApplyObstacles() {

        for(int  i = 0; i < numObstacles; i++)
        {
			Vector3 position = GetOpenPosition(minDistance, 0.0f, i); //Todo: will neeed to sample mesh for y value if non-flat ground
			int obstacleType = Random.value > obstacleProbability ? BLOCKING : HIDING;
			Transform instance = AddObstacle(position, obstacleType, 0.4f, 0.6f);
			obstacles[i] = instance;
        }

    }

	public Transform AddObstacle(Vector3 position, int obstacleType, float minScale, float maxScale) {
		Transform instance = Instantiate(obstaclePrefabs[obstacleType]);
		position.y += instance.transform.localScale.y * 0.5f;

		float randScale = (maxScale - minScale) * Random.value + minScale;
		Vector3 scale = new Vector3(randScale, 1.0f, randScale);

		//instance.localPosition = GroundMesh.SamplePoint(position); //IF we eever geenerate the greound mesh to have irregularities / noise then we'll need this

		instance.localScale = scale;
		instance.SetParent(obstacleContainer, false);
		return instance;
	}


	private Vector3 GetOpenPosition(float minDistance, float yValue, int index)
    {
		int retryCount = 0;
		Vector3 position = new Vector3(0,yValue,0);
		bool placed = false;

        while(!placed)
        {
			position.x = WorldBounds.xMin + (Random.value * WorldBounds.width);
            position.z = WorldBounds.yMin + (Random.value * WorldBounds.height);


			for (int i = 0; i < index; i ++)
			{
				Transform t = obstacles[i];
				if (Vector3.Distance(t.position, position) < minDistance)
				{
					retryCount++;
					break;
				}
                else {
                    if(i == index -1 )
                    {
						placed = true;
                    }
                }
			}

		}

        if(retryCount >= 5)
        {
			Debug.LogError("Had to retry GetOpenPositon more that 5 times: " + retryCount);
        }
		return position;

	}
}
