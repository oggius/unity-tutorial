using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter counter;
    [SerializeField] private GameObject particles;
    [SerializeField] private GameObject heatingSurface;
    // Start is called before the first frame update
    
    void Start()
    {
        counter.fryingStateChanged += Counter_fryingStateChanged;
    }

    private void Counter_fryingStateChanged(object sender, StoveCounter.FryingStateChangedEventArgs e)
    {
        bool isFrying = e.state == StoveCounter.FryingState.Frying || e.state == StoveCounter.FryingState.Burning;

        particles.SetActive(isFrying);
        heatingSurface.SetActive(isFrying);
    }
}
