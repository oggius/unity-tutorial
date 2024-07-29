using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour
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
            placedObject.SetClearCounter(secondClearCounter);
        }
    }

    public void Interact() {
        if (placedObject == null) {
            // instantiate product
            GameObject kitchenObjectInstance = Instantiate(kitchenObject.prefab, topCounterPoint.transform);
            kitchenObjectInstance.GetComponent<KitchenObject>().SetClearCounter(this);
            //// fix the position (TODO understand why it is not 0 by default)
            //kitchenObjectInstance.transform.localPosition = Vector3.zero;

            //// set placed object
            //placedObject = kitchenObjectInstance.GetComponent<KitchenObject>();

            //placedObject.SetClearCounter(this);
        } else {
            Debug.Log("Placed on " + placedObject.GetClearCounter());
        }
    }

    public GameObject GetPlacementPoint() {
        return this.topCounterPoint;
    }

    public void SetPlacedObject(KitchenObject kitchenObject) {
        this.placedObject = kitchenObject;
    }

    public KitchenObject GetPlacedObject() {
        return placedObject;
    }

    public void ClearPlacedObject() {
        this.placedObject = null;
    }

    public bool HasPlacedObject() {
        return placedObject != null;
    }
}
