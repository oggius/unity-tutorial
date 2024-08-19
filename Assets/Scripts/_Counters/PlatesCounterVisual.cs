using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter counter;
    [SerializeField] private KitchenObjectSO plateSO;
    [SerializeField] private Transform topPoint;

    private List<GameObject> platesSpawned;

    private void Awake() {
        platesSpawned = new List<GameObject>();
    }

    private void Start() { 
        counter.plateSpawned += PlatesCounter_plateSpawned;
        counter.platePicked += PlatesCounter_platePicked;
    }
    
    private void PlatesCounter_plateSpawned(object sender, System.EventArgs e) {
        float spaceBetweenPlaces = 0.15f;
        var instance = Instantiate(plateSO.prefab, topPoint);
        platesSpawned.Add(instance);
        instance.transform.localPosition =  new Vector3(0,  spaceBetweenPlaces * (platesSpawned.Count - 1), 0);
    }

    private void PlatesCounter_platePicked(object sender, System.EventArgs e) {
        var plate = platesSpawned[platesSpawned.Count - 1];
        platesSpawned.Remove(plate);
        Destroy(plate);
    }
    
}
