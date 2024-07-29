using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectHolder holder;

    public KitchenObjectSO GetKitchenObjectSO() {
        return kitchenObjectSO;
    }

    public void SetHolder(IKitchenObjectHolder newHolder) {
        if (newHolder.HasHeldObject()) {
            Debug.Log("New holder already has kitchen object" + newHolder.GetHeldObject());
            return;
        }

        if (this.holder != null) {
            this.holder.ReleaseHeldObject();
        }

        this.holder = newHolder;
        this.holder.ObtainKitchenObject(this);

        // place visual into the holding point
        transform.parent = newHolder.GetHoldingPoint().transform;
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectHolder GetHolder() {
        return this.holder;
    }
}
