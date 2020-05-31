using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    const int MAX_SHELLS = 10;

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
            Debug.Log("I picked up a shell!");
        }
    }

    private int carryShells;

    public int HungerLevel { get; set; }
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
