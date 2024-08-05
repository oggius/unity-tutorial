using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCounter : MonoBehaviour, IKitchenObjectHolder
{
    [SerializeField] private GameObject topCounterPoint;

    protected KitchenObject heldObject;

    public abstract void Interact(Player player);

    public GameObject GetHoldingPoint() {
        return this.topCounterPoint;
    }

    public void ObtainKitchenObject(KitchenObject kitchenObject) {
        heldObject = kitchenObject;
    }

    public KitchenObject GetHeldObject() {
        return heldObject;
    }

    public void ReleaseHeldObject() {
        heldObject = null;
    }

    public bool HasHeldObject() {
        return heldObject != null;
    }
}
