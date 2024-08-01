using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObject;

    public override void Interact(Player player) {
        KitchenObject counterObject = GetHeldObject();
        KitchenObject playersObject = player.GetHeldObject();
        Debug.Log("Players object: " + playersObject + ", counter object: " + counterObject);

        if (playersObject != null) {
            Debug.Log("Cutting counter obtains: " + playersObject);
            playersObject.ChangeHolder(this);
            playersObject.DestroySelf();

            GameObject kitchenObjectInstance = Instantiate(kitchenObject.prefab);
            kitchenObjectInstance.GetComponent<KitchenObject>().SetHolder(this);
        }

        if (counterObject != null) {
            Debug.Log("Player grabs: " + counterObject);
            counterObject.SetHolder(player);
        }
    }
    
}
