using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public BirdEnemy birdPrefab;
    public float numEnemies;
    private float startEnemies;
    List<BirdEnemy> enemies;

    Transform enemyContainer;

    public GameObject ground;

    private Mesh groundMesh;

    public static EnemyManager Instance;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        startEnemies = numEnemies;
    }

    // Start is called before the first frame update
    void Start()
    {
        groundMesh = ground.GetComponent<MeshFilter>().mesh;
        ClearEnemies();
        InstantiateEnemies();
        PlaceEnemies();
    }


    //TODO: Eneemy type parameter if we ever have more enemies
    public void AddEnemies(int count)
    {

        for(int i = 0; i < count; i++)
        {
            BirdEnemy birdEnemy = Instantiate<BirdEnemy>(birdPrefab);
            birdEnemy.transform.SetParent(enemyContainer, false);
            enemies.Add(birdEnemy);
        }

        numEnemies = enemies.Count;

        PlaceEnemies();
    }

    void PlaceEnemies()
    {
        Vector3 groundSize = Vector3.Scale(ground.transform.localScale, groundMesh.bounds.size);


        int enemyRows = (int)Mathf.Sqrt(numEnemies);
        float delta = (groundSize.x / enemyRows);

        Vector3 birdPosition = new Vector3(-groundSize.x, 10, -groundSize.z);
        int e = 0;
        for (int i = 0; i < enemyRows; i++)
        {
            birdPosition.x = (-groundSize.x / 2) + (delta * i);
            for (int j = 0; j < enemyRows; j++, e++)
            {
                birdPosition.z = delta *  j;
                enemies[e].transform.position = birdPosition;
            }
        }
    }


    public void ClearEnemies()
    {
        if (enemyContainer)
        {
            Destroy(enemyContainer.gameObject);
        }
        enemyContainer = new GameObject("Enemies Container").transform;
        enemyContainer.SetParent(transform, false);
        enemies = new List<BirdEnemy>();
    }

    private void InstantiateEnemies()
    {
        
        for (int i = 0; i < numEnemies; i++)
        {
            BirdEnemy birdEnemy = Instantiate<BirdEnemy>(birdPrefab);
            birdEnemy.transform.SetParent(enemyContainer, false);
            enemies.Add(birdEnemy);
        }

    }

    public void Reset()
    {
        Debug.Log("RESETING ENEMIES");
        numEnemies = startEnemies;
        ClearEnemies();
        InstantiateEnemies();
        PlaceEnemies();
    }
}
