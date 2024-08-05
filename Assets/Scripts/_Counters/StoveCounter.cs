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
        var playerProduct = player.GetHeldObject();
        var counterProduct = GetHeldObject();

        Debug.Log("Player product: " + playerProduct + ", counter product: " + counterProduct);
        
        if (!CanInteract()) {
            Debug.Log("Frying in progress. No actions allowed");
            return;
        }

        if (playerProduct != null) {
            var recipe = GetRecipeForProduct(playerProduct);
            if (recipe != null) {
                playerProduct.ChangeHolder(this);
                StartCooking(recipe);
            } else {
                Debug.Log("Can not process product, no matching recipe");
            }
        } 
        
        if (counterProduct != null) {
            Debug.Log("Passing counter product: " + counterProduct);
            counterProduct.ChangeHolder(player);
            StopCooking();
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
}
