using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class KitchenObjectSO : ScriptableObject
{
    public GameObject prefab;
    public Sprite sprite;
    public string objectName;
    public float cuttingSeconds;
    public bool canCut;
    public KitchenObjectSO cutsInto;
}
