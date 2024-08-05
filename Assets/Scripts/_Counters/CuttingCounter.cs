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
            Debug.Log("Player grabs: " + counterObject);
            counterObject.ChangeHolder(player);
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
}
