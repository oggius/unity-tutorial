using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CuttingCounter : BaseCounter, IProgressible
{
    public event EventHandler cuttingStarted;
    public event EventHandler cuttingStopped;
    public event EventHandler<IProgressible.ProgressChangedEventArgs> progressChanged;

    private bool isCutting = false;
    private float cuttingProgressSeconds;
    private float cuttingMaxSeconds;
    private KitchenObject cuttingKitchenObject;

    public override void Interact(Player player) {
        if (IsCutting()) {
            Debug.Log("Cutting in progress. No actions allowed");
            return;
        }
        KitchenObject counterObject = GetHeldObject();
        KitchenObject playerObject = player.GetHeldObject();
        Debug.Log("Players object: " + playerObject + ", counter object: " + counterObject);

        if (playerObject != null && playerObject.IsCuttable()) {
            Debug.Log("Cutting counter obtains: " + playerObject);
            playerObject.ChangeHolder(this);

            StartCutting(playerObject);
        }

        if (counterObject != null) {
            if (playerObject is PlateKitchenObject plate) {
                Debug.Log("Player holds the plate, trying to add cut ingredient: " + counterObject);
                if (TryAddIngredientToPlate(plate, counterObject)) {
                    Debug.Log("Ingredient added, destroying object");
                    counterObject.DestroySelf();
                }
            } else {
                Debug.Log("Player grabs: " + counterObject);
                counterObject.ChangeHolder(player);
            }
        }
    }

    public bool IsCutting() {
        return isCutting;
    }

    private void StartCutting(KitchenObject kitchenObject) {
        Debug.Log("Starting cutting");
        isCutting = true;
        cuttingKitchenObject = kitchenObject;
        cuttingMaxSeconds = kitchenObject.GetKitchenObjectSO().cuttingSeconds;
        cuttingProgressSeconds = 0f;

        cuttingStarted?.Invoke(this, EventArgs.Empty);
    }

    private void StopCutting() {
        Debug.Log("Stopping cutting");
        isCutting = false;
        cuttingProgressSeconds = 0f;
        cuttingMaxSeconds = 0f;

        cuttingKitchenObject.DestroySelf();
        KitchenObject.SpawnKitchenObject(cuttingKitchenObject.GetCutsInto(), this);

        cuttingStopped?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        if (isCutting) {
            cuttingProgressSeconds += Time.deltaTime;
            progressChanged?.Invoke(this, new IProgressible.ProgressChangedEventArgs {
                currentProgress = cuttingProgressSeconds, maxProgress = cuttingMaxSeconds
            });

            if (cuttingProgressSeconds > cuttingMaxSeconds) {
                StopCutting();
            }
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
}
