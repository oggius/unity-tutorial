using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;
    [SerializeField] private Image barImage;

    void Start()
    {
        cuttingCounter.cuttingProgressChanged += CuttingCounter_cuttingProgressChanged;
        cuttingCounter.cuttingStopped += CuttingCounter_cuttingStopped;
        Hide();
    }

    private void CuttingCounter_cuttingStopped(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void CuttingCounter_cuttingProgressChanged(object sender, CuttingCounter.CuttingProgressChangedEventArgs e)
    {
        float progressNormalized = e.currentProgress / e.maxProgress;
        barImage.fillAmount = progressNormalized;
        Show();
    }

    public void Hide() {
        this.gameObject.SetActive(false);
    }

    public void Show() {
        this.gameObject.SetActive(true);
    }
}
