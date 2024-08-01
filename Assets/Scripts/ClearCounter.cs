using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter, IKitchenObjectHolder
{
    [SerializeField] private KitchenObjectSO kitchenObject;

    public override void Interact(Player player) {
        KitchenObject counterObject = GetHeldObject();
        KitchenObject playersObject = player.GetHeldObject();
        Debug.Log("Players object: " + playersObject + ", counter object: " + counterObject);

        if (counterObject != null && playersObject != null) {
            // swap objects
            counterObject.SetHolder(player);
            playersObject.SetHolder(this);
        } else {
            if (counterObject != null) {
                Debug.Log("Player obtains: " + counterObject);
                counterObject.ChangeHolder(player);
            }

            if (playersObject != null) {
                Debug.Log("Counter obtains: " + playersObject);
                playersObject.ChangeHolder(this);
            }
        }
    }
}
