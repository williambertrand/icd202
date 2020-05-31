using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    SAFE,
    HIDING,
    IN_OPEN
}

public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance;

    const int MAX_SHELLS = 10;

    public Text carryValueText;
    public int CarryShells
    {
        get
        {
            return carryShells;
        }
        set
        {
            if (carryShells == MAX_SHELLS)
            {
                carryShells = MAX_SHELLS;
            }
            else
            {
                carryShells = value;
            }
            carryValueText.text = "" + carryShells;
        }
    }

    private int carryShells;

    public int HungerLevel { get; set; }

    public PlayerState playerstate;
    public float currentTripDist;

    private Ray safeRay;
    private Renderer renderer;

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
        currentTripDist = 0;
        playerstate = PlayerState.SAFE;

        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if(playerstate != PlayerState.SAFE)
        {
            CheckPlayerState();
        }

    }


    void CheckPlayerState()
    {
        int layerMask = 1 << 8;
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, Vector3.up, out hit, 2.5f, layerMask))
        {
            Debug.DrawRay(transform.position, Vector3.up * hit.distance, Color.green);
            //Get the Renderer component from the new cube

            //Call SetColor using the shader property name "_Color" and setting the color to red
            renderer.material.SetColor("_EmissionColor", Color.green);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * 1000, Color.white);
            renderer.material.SetColor("_EmissionColor", Color.blue);
        }
    }
}
