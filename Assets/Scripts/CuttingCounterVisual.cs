using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    private const string IS_CUTTING = "IsCutting";

    [SerializeField] private CuttingCounter counter;
    private Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Start()
    {
        counter.cuttingStarted += Counter_cuttingStarted;
        counter.cuttingStopped += Counter_cuttingStopped;
    }

    private void Counter_cuttingStopped(object sender, System.EventArgs e)
    {
        Debug.Log("Animation finished");
        animator.SetBool(IS_CUTTING, false);
    }

    private void Counter_cuttingStarted(object sender, System.EventArgs e)
    {
        Debug.Log("Animation started");
        animator.SetBool(IS_CUTTING, true);
    }
}
