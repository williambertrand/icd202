﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellItem : Item
{
    private void OnTriggerEnter(Collider other)
    {
        //This is probably unnecesarry as only collison will be with player?
        if (other.gameObject.CompareTag("Player"))
        {
            bool pickedUp = player.OnPickup();
            if (pickedUp)
            {
                Destroy(gameObject);
            }
        }
    }



}
