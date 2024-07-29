using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectHolder
{
    public void ObtainKitchenObject(KitchenObject kitchenObject);

    public KitchenObject GetHeldObject();

    public void ReleaseHeldObject();

    public bool HasHeldObject();

    public GameObject GetHoldingPoint();
}
