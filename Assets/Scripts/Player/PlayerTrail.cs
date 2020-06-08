using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrail : MonoBehaviour
{
    Transform trailContainer;


    [SerializeField]
    Transform[] nodePrefabs;
    int numNodePrefabs;

    float nodeSpacing = 1.0f;
    public float lastNodeDist = 0.0f;

    private int leg = 0;
    private const int numLegs = 6;


    public static PlayerTrail Instance;

    private void Awake()
    {
        trailContainer = new GameObject("Trail Container").transform;
        trailContainer.SetParent(transform, false);

        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerController.Instance.currentTripDist - lastNodeDist > nodeSpacing)
        {

            Vector3 nodePos = PlayerController.Instance.transform.position;
            nodePos.y = 0.1f;
            if(leg < 3)
            {
                nodePos.x -= leg * 0.15f;
            }
            else
            {
                nodePos.x += leg * 0.1f;
            }
            AddNode(nodePos, Vector3.zero, 0.0f);
            leg = (leg + 1) % numLegs;
            lastNodeDist = PlayerController.Instance.currentTripDist;
        }
    }


    public void ClearTrail()
    {
        if (trailContainer)
        {
            Destroy(trailContainer.gameObject);
        }
        trailContainer = new GameObject("Trail Container").transform;
        trailContainer.SetParent(transform, false);
        PlayerController.Instance.currentTripDist = 0;
        lastNodeDist = 0;
    }

    /* Place a node at the current location
     * Future improvements may invovle changing sprite based on direction or
     * velocity (like stretching it out when going faster or something)
    */

    public Transform AddNode(Vector3 position, Vector3 direction, float velocity)
    {
        int randNode = Random.Range(0, numNodePrefabs);
        float randScale = (1.75f) * Random.value + 2.0f;

        Transform instance = Instantiate(nodePrefabs[randNode]);


        instance.localPosition = position;
        instance.localScale = new Vector3(randScale, 1.0f, randScale);
        instance.rotation = Quaternion.Euler(90, 0, 0);
        instance.SetParent(trailContainer, false);
        return instance;
    }
}
