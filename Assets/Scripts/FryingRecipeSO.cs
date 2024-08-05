using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FryingRecipeSO : ScriptableObject
{
    public KitchenObjectSO uncookedProduct;
    public KitchenObjectSO friedProduct;
    public KitchenObjectSO burnedProduct;
    public float fryingTime;
    public float burningTime;
}
