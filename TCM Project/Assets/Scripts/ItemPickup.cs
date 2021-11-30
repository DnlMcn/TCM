using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    PlayerController player;
    public event System.Action OnItemPickup;

    public bool HasItem; // Substituindo "Item" pelo nome do item (e.g. HasGun, HasPhone, etc.)

    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        player.OnItemPickup += Pickup;
      
    }

    public void Pickup()
    {
        HasItem = true;
        Debug.Log("Item was picked up.");

    }


}
