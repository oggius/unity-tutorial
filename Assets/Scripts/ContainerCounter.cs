using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler playerGrabbedObject;

    [SerializeField] private KitchenObjectSO kitchenObject;

    public override void Interact(Player player) {
        if (!player.HasHeldObject()) {
            Debug.Log("Spawn object " + kitchenObject);
            // instantiate product
            KitchenObject.SpawnKitchenObject(kitchenObject, player);
            playerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
    