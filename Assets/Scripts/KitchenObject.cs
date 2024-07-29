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
        if (this.holder != null) {
            this.holder.ReleaseHeldObject();
        }

        if (newHolder.HasHeldObject()) {
            Debug.LogWarning("New holder already has kitchen object");
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
