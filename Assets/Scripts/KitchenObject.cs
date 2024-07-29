using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] KitchenObjectSO kitchenObjectSO;

    private ClearCounter clearCounter;

    public KitchenObjectSO GetKitchenObjectSO() {
        return kitchenObjectSO;
    }

    public void SetClearCounter(ClearCounter newCounter) {
        if (this.clearCounter != null) {
            this.clearCounter.ClearPlacedObject();
        }

        if (newCounter.HasPlacedObject()) {
            Debug.LogWarning("New counter already has kitchen object");
            return;
        }

        this.clearCounter = newCounter;
        this.clearCounter.SetPlacedObject(this);

        transform.parent = newCounter.GetPlacementPoint().transform;
        transform.localPosition = Vector3.zero;
    }

    public ClearCounter GetClearCounter() {
        return this.clearCounter;
    }
}
