using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItem: Item
{
    public int ItemFoodValue = 1;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.EatItem(ItemFoodValue);
            Destroy(gameObject);
        }
    }
}
