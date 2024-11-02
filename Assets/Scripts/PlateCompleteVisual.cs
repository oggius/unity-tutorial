using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct MappedPlateIngredients {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }

    [SerializeField] PlateKitchenObject plateKitchenObject;
    [SerializeField] List<MappedPlateIngredients> mappedPlateIngredientsList;

    // Start is called before the first frame update
    void Start()
    {
        plateKitchenObject.ingredientAddedToPlate += PlateKitchenObject_ingredientAddedToPlate;
    }

    private void PlateKitchenObject_ingredientAddedToPlate(object sender, PlateKitchenObject.IngredientAddedToPlateEventArgs e) {
        foreach (MappedPlateIngredients map in mappedPlateIngredientsList) {
            if (map.kitchenObjectSO == e.ingredient) {
                map.gameObject.SetActive(true);
            }
        }
    }
}
