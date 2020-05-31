using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

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

    public float currentTripDist;


    // Start is called before the first frame update
    void Start()
    {
        currentTripDist = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
