using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellItem : Item
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //This is probably unnecesarry as only collison will be with player?
        if (other.gameObject.CompareTag("Player"))
        {
            player.CarryShells = player.CarryShells + 1;
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("Collided with non player?!");
        }
    }



}
