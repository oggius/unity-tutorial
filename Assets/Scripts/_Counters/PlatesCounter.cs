using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlatesCounter : BaseCounter
{

    public event EventHandler plateSpawned;
    public event EventHandler platePicked;

    [SerializeField] private KitchenObjectSO plateSO;
    private float spawnPlateTimer = 0f;
    private float spawnPlateMaxTime = 4f;
    private int platesSpawnedAmount = 0;
    private int platesMaxAmount = 4;

    private void Update() {
        if (platesSpawnedAmount >= platesMaxAmount) {
            return;
        }
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateMaxTime) {
            spawnPlateTimer = 0f;
            plateSpawned?.Invoke(this, EventArgs.Empty);
            // KitchenObject.SpawnKitchenObject(plateSO, this);
            platesSpawnedAmount++;
        }
    }
    public override void Interact(Player player)
    {
        Debug.Log("Interacted with PlatesCounter");
        if (!player.HasHeldObject() && platesSpawnedAmount > 0) {
            KitchenObject.SpawnKitchenObject(plateSO, player);
            platesSpawnedAmount--;
            platePicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
