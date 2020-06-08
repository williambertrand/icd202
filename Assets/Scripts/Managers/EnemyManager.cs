using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public BirdEnemy birdPrefab;
    public float numEnemies;
    List<BirdEnemy> enemies;

    public GameObject ground;

    private Mesh groundMesh;

    public static EnemyManager Instance;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Place enemies
        enemies = new List<BirdEnemy>();
        for(int i = 0; i < numEnemies; i++)
        {
            BirdEnemy birdEnemy = Instantiate<BirdEnemy>(birdPrefab);
            enemies.Add(birdEnemy);
        }

        groundMesh = ground.GetComponent<MeshFilter>().mesh;

        PlaceEnemies();
    }


    //TODO: Eneemy type parameter if we ever have more enemies
    public void AddEnemies(int count)
    {

        for(int i = 0; i < count; i++)
        {
            BirdEnemy birdEnemy = Instantiate<BirdEnemy>(birdPrefab);
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
}
