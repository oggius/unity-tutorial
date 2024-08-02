using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    public event EventHandler<CuttingProgressChangedEventArgs> cuttingProgressChanged;
    public class CuttingProgressChangedEventArgs : EventArgs {
        public float currentProgress;
        public float maxProgress;
    }

    public event EventHandler cuttingStarted;
    public event EventHandler cuttingStopped;

    [SerializeField] private KitchenObjectSO cutKitchenObject;

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
            //playerObject.DestroySelf();

            //KitchenObject.SpawnKitchenObject(cutKitchenObject, this);

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
            cuttingProgressChanged?.Invoke(this, new CuttingProgressChangedEventArgs {
                currentProgress = cuttingProgressSeconds, maxProgress = cuttingMaxSeconds
            });

            if (cuttingProgressSeconds > cuttingMaxSeconds) {
                StopCutting();
            }
        }
    }
}
