using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter, IKitchenObjectHolder
{
    [SerializeField] private KitchenObjectSO kitchenObject;

    public override void Interact(Player player) {
        Debug.Log("Interacted with clear counter: " + this);
        KitchenObject counterObject = GetHeldObject();
        KitchenObject playersObject = player.GetHeldObject();
        Debug.Log("Players object: " + playersObject + ", counter object: " + counterObject);

        if (playersObject != null) {
            // player holds something
            if (playersObject is PlateKitchenObject) {
                // player holds the plate
                if (counterObject != null) {
                    if (AddIngredientToPlate((PlateKitchenObject) playersObject, counterObject.GetKitchenObjectSO())) {
                        counterObject.DestroySelf(); 
                    }
                } else {
                    playersObject.ChangeHolder(this);
                }
            } else {
                // player holds the product
                if (counterObject != null) {
                    if (counterObject is PlateKitchenObject) {
                        if (AddIngredientToPlate((PlateKitchenObject) counterObject, playersObject.GetKitchenObjectSO())) {
                            playersObject.DestroySelf();
                        }
                    } else {
                        SwapObjects(playersObject, counterObject, player, this);
                    }
                } else {
                    playersObject.ChangeHolder(this);
                }
            }
        } else if (counterObject != null) {
            counterObject.ChangeHolder(player);
        }
    }

    private bool AddIngredientToPlate(PlateKitchenObject plate, KitchenObjectSO ingredient) {
        Debug.Log("Trying to add ingredient to plate");
        if (!plate.CanAddIngredient(ingredient)) {
            Debug.Log("Ingredient is not allowed");
            return false;
        }

        return plate.AddIngredient(ingredient);
    }

    private void SwapObjects(KitchenObject playersObject, KitchenObject counterObject, Player player, ClearCounter counter) {
        playersObject.SetHolder(counter);
        counterObject.SetHolder(player);
    }
}
