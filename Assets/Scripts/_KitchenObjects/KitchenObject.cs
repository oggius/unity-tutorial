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

    public void ChangeHolder(IKitchenObjectHolder newHolder) {
        if (newHolder.HasHeldObject()) {
            Debug.LogError("New holder already has kitchen object " + newHolder.GetHeldObject());
            return;
        }

        if (this.holder != null) {
            this.holder.ReleaseHeldObject();
        }

        newHolder.ObtainKitchenObject(this);
        this.holder = newHolder;

        // place visual into the holding point
        transform.parent = newHolder.GetHoldingPoint().transform;
        transform.localPosition = Vector3.zero;
    }

    public void SetHolder(IKitchenObjectHolder newHolder) {
        newHolder.ObtainKitchenObject(this);
        this.holder = newHolder;

        // place visual into the holding point
        transform.parent = newHolder.GetHoldingPoint().transform;
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectHolder GetHolder() {
        return this.holder;
    }

    public void DestroySelf() {
        holder.ReleaseHeldObject();
        Destroy(gameObject);
    }

    // spawns kitchen object and assigns it to the holder
    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectHolder holder) {
        GameObject kitchenObjectInstance = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObjectInstance.GetComponent<KitchenObject>();
        kitchenObject.ChangeHolder(holder);

        return kitchenObject;
    }

    public bool IsCuttable() {
        return kitchenObjectSO.canCut;
    }

    public KitchenObjectSO GetCutsInto() {
        return kitchenObjectSO.cutsInto;
    }
}
