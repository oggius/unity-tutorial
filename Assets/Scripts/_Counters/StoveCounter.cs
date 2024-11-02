using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class StoveCounter : BaseCounter, IProgressible
{
    public event EventHandler<IProgressible.ProgressChangedEventArgs> progressChanged;
    public event EventHandler<FryingStateChangedEventArgs> fryingStateChanged;

    public class FryingStateChangedEventArgs : EventArgs {
        public FryingState state;
    }

    private FryingState currentState;
    private FryingRecipeSO fryingRecipe;
    private float fryingProgressSeconds;
    private float burningProgressSeconds;

    public enum FryingState {
        Idle,
        Frying,
        Burning,
        Burned
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipes;

    public override void Interact(Player player)
    {
        var playerObject = player.GetHeldObject();
        var counterObject = GetHeldObject();

        Debug.Log("Player object: " + playerObject + ", counter object: " + counterObject);
        
        if (!CanInteract()) {
            Debug.Log("Frying in progress. No actions allowed");
            return;
        }

        if (playerObject != null) {
            var recipe = GetRecipeForProduct(playerObject);
            if (recipe != null) {
                playerObject.ChangeHolder(this);
                StartCooking(recipe);
            } else {
                Debug.Log("Can not process product, no matching recipe");
            }
        } 
        
        if (counterObject != null) {
            if (playerObject is PlateKitchenObject plate) {
                if (TryAddIngredientToPlate(plate, counterObject)) {
                    counterObject.DestroySelf();
                    StopCooking();
                }
            } else {
                Debug.Log("Passing counter product: " + counterObject);
                counterObject.ChangeHolder(player);
                StopCooking();
            }
        }
    }

    private void Update() {
        ProcessCooking();
    }

    private void StartCooking(FryingRecipeSO recipe) {
        fryingRecipe = recipe;
        StartFrying();
    }

    private void ProcessCooking() {
        switch (currentState) {
            case FryingState.Idle:
                break;
            case FryingState.Frying:
                fryingProgressSeconds += Time.deltaTime;
                progressChanged?.Invoke(this, new IProgressible.ProgressChangedEventArgs{
                    currentProgress = fryingProgressSeconds,
                    maxProgress = fryingRecipe.fryingTime
                });

                if (fryingProgressSeconds > fryingRecipe.fryingTime) {
                    Debug.Log("Frying completed");
                    FinishFrying();
                }
                break;
            case FryingState.Burning:
                burningProgressSeconds += Time.deltaTime;
                progressChanged?.Invoke(this, new IProgressible.ProgressChangedEventArgs{
                    currentProgress = burningProgressSeconds,
                    maxProgress = fryingRecipe.burningTime
                });

                if (burningProgressSeconds > fryingRecipe.burningTime) {
                    Debug.Log("Burning completed");
                    FinishBurning();
                }
                break;
            case FryingState.Burned:
                break;
        }
    }

    private void StopCooking() {
        currentState = FryingState.Idle;
        fryingStateChanged?.Invoke(this, new FryingStateChangedEventArgs { state = currentState } );
        progressChanged?.Invoke(this, new IProgressible.ProgressChangedEventArgs{ currentProgress = 0f, maxProgress = 0f } );
    }

    private void StartFrying() {
        currentState = FryingState.Frying;
        fryingProgressSeconds = 0f;
        fryingStateChanged?.Invoke(this, new FryingStateChangedEventArgs{ state = FryingState.Frying });
    }

    private void FinishFrying() {
        // change uncooked product with cooked one
        GetHeldObject().DestroySelf();
        KitchenObject.SpawnKitchenObject(fryingRecipe.friedProduct, this);
        StartBurning();
    }

    private void StartBurning() {
        Debug.Log("Burning started");
        currentState = FryingState.Burning;
        burningProgressSeconds = 0f;
        fryingStateChanged?.Invoke(this, new FryingStateChangedEventArgs{ state = FryingState.Burning });
    }

    private void FinishBurning() {
        Debug.Log("Burning completed");
        GetHeldObject().DestroySelf();
        KitchenObject.SpawnKitchenObject(fryingRecipe.burnedProduct, this);

        currentState = FryingState.Burned;
        fryingStateChanged?.Invoke(this, new FryingStateChangedEventArgs{ state = FryingState.Burned });

        StopCooking();
    }

    private FryingRecipeSO GetRecipeForProduct(KitchenObject product) {
        KitchenObjectSO productScriptable = product.GetKitchenObjectSO();
        foreach (var recipe in fryingRecipes) {
            if (productScriptable == recipe.uncookedProduct) {
                return recipe;
            }
        }

        return null;
    }

    private bool CanInteract() {
        return currentState != FryingState.Frying;
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
