using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCounter : MonoBehaviour, IKitchenObjectHolder
{
    [SerializeField] private GameObject topCounterPoint;

    private KitchenObject placedObject;

    public abstract void Interact(Player player);

    public GameObject GetHoldingPoint() {
        return this.topCounterPoint;
    }

    public void ObtainKitchenObject(KitchenObject kitchenObject) {
        this.placedObject = kitchenObject;
    }

    public KitchenObject GetHeldObject() {
        return placedObject;
    }

    public void ReleaseHeldObject() {
        this.placedObject = null;
    }

    public bool HasHeldObject() {
        return placedObject != null;
    }
}
