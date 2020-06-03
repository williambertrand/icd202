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

    public int Health { get; set; }
    public int MaxHealth = 5;

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
        Health = MaxHealth;
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
        if (Physics.Raycast(transform.position, Vector3.up, out hit, 2.5f, layerMask))
        {
            playerstate = PlayerState.HIDING;
        }
        else
        {
            playerstate = PlayerState.IN_OPEN;
        }
    }

    public void EatItem(int value)
    {
        if (Health + value > MaxHealth)
        {
            Health = MaxHealth;
        }
        else
        {
            Health += value;
        }
    }
}
