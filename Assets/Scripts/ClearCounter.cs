using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObject;
    [SerializeField] private GameObject topCounterPoint;

    public void Interact() {
        GameObject kitchenObjectInstance = Instantiate(kitchenObject.prefab, topCounterPoint.transform);
        kitchenObjectInstance.transform.localPosition = Vector3.zero;
        Debug.Log(kitchenObjectInstance.transform.GetComponent<KitchenObject>().GetKitchenObjectSO().objectName);
    }
}
