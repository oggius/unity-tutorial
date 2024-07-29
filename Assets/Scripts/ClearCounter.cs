using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour, IKitchenObjectHolder
{
    [SerializeField] private KitchenObjectSO kitchenObject;
    [SerializeField] private GameObject topCounterPoint;

    [SerializeField] bool testing;
    [SerializeField] ClearCounter secondClearCounter;

    private KitchenObject placedObject;

    private void Update()
    {
        if (testing && Input.GetKeyDown(KeyCode.T)) {
            Debug.Log("Testing mode");
            placedObject.SetHolder(secondClearCounter);
        }
    }

    public void Interact(Player player) {
        if (placedObject == null) {
            if (player.HasHeldObject()) {
                player.GetHeldObject().SetHolder(this);
            } else {
                // instantiate product
                GameObject kitchenObjectInstance = Instantiate(kitchenObject.prefab, topCounterPoint.transform);
                kitchenObjectInstance.GetComponent<KitchenObject>().SetHolder(this);
            }
        } else {
            placedObject.SetHolder(player);
        }
    }

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
