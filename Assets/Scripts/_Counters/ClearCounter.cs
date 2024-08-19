using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter, IKitchenObjectHolder {
    [SerializeField] private KitchenObjectSO kitchenObject;

    public override void Interact(Player player) {
        Debug.Log("Interacted with clear counter: " + this);

        KitchenObject counterObject = GetHeldObject();
        KitchenObject playersObject = player.GetHeldObject();
        
        Debug.Log("Interaction attempt. Players object: " + playersObject + ", counter object: " + counterObject);

        if (playersObject != null) {
            HandlePlayerObjectInteraction(playersObject, counterObject, player);
        } else if (counterObject != null) {
            counterObject.ChangeHolder(player);
        }
    }

    private void HandlePlayerObjectInteraction(KitchenObject playersObject, KitchenObject counterObject, Player player) {
        if (playersObject is PlateKitchenObject plate) {
            HandlePlateInteraction(plate, counterObject);
        } else {
            HandleNonPlateInteraction(playersObject, counterObject, player);
        }
    }

    private void HandlePlateInteraction(PlateKitchenObject plate, KitchenObject counterObject) {
        if (counterObject != null) {
            if (TryAddIngredientToPlate(plate, counterObject)) {
                counterObject.DestroySelf();
            }
        } else {
            plate.ChangeHolder(this);
        }
    }

    private void HandleNonPlateInteraction(KitchenObject playersObject, KitchenObject counterObject, Player player) {
        if (counterObject != null) {
            if (counterObject is PlateKitchenObject plate) {
                if (TryAddIngredientToPlate(plate, playersObject)) {
                    playersObject.DestroySelf();
                }
            } else {
                SwapObjects(playersObject, counterObject, player);
            }
        } else {
            playersObject.ChangeHolder(this);
        }
    }

    private bool TryAddIngredientToPlate(PlateKitchenObject plate, KitchenObject ingredientObject) {
        Debug.Log("Trying to add ingredient to plate");

        if (AddIngredientToPlate(plate, ingredientObject.GetKitchenObjectSO())) 
        {
            return true;
        }

        Debug.Log("Ingredient is not allowed");
        return false;
    }

    private bool AddIngredientToPlate(PlateKitchenObject plate, KitchenObjectSO ingredient) {
        return plate.CanAddIngredient(ingredient) && plate.AddIngredient(ingredient);
    }

    private void SwapObjects(KitchenObject playersObject, KitchenObject counterObject, Player player) {
        playersObject.SetHolder(this);
        counterObject.SetHolder(player);
    }
}
