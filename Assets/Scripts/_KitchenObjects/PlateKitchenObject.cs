using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<IngredientAddedToPlateEventArgs> ingredientAddedToPlate;
    public class IngredientAddedToPlateEventArgs : EventArgs {
        public KitchenObjectSO ingredient;
    }
    [SerializeField] private List<KitchenObjectSO> allowedIngredients;

    private List<KitchenObjectSO> addedIngredients = new List<KitchenObjectSO>();

    public bool CanAddIngredient(KitchenObjectSO ingredient) {
        return allowedIngredients.Contains(ingredient);
    }

    public bool AddIngredient(KitchenObjectSO ingredient) {
        if (!allowedIngredients.Contains(ingredient)) {
            throw new Exception("Ingredient " + ingredient + " is not allowed on the plate");
        }

        if (addedIngredients.Contains(ingredient)) {
            Debug.Log("Ingredient " + ingredient + " is already added");
            return false;
        }

        addedIngredients.Add(ingredient);
        ingredientAddedToPlate?.Invoke(this, new IngredientAddedToPlateEventArgs { ingredient = ingredient });
        Debug.Log("Ingredient " + ingredient + " added");
        return true;
    }
}
