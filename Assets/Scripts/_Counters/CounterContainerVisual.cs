using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterContainerVisual : MonoBehaviour
{
    private const string OPEN_CLOSE_TRIGGER = "OpenClose";

    [SerializeField] private ContainerCounter counter;
    private Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Start()
    {
        counter.playerGrabbedObject += Counter_playerGrabbedObject;
    }

    private void Counter_playerGrabbedObject(object sender, System.EventArgs e)
    {
        Debug.Log("Animation started");
        animator.SetTrigger(OPEN_CLOSE_TRIGGER);
    }
}
