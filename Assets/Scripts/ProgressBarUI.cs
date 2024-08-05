using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    private IProgressible progressible;

    [SerializeField] private GameObject progressibleObject;
    [SerializeField] private Image barImage;

    void Start()
    {
        Hide();
        // workaround for issue of Unity not supporing interface as SerializedField
        progressible = progressibleObject.GetComponent<IProgressible>();
        if (progressible == null) {
            Debug.LogError("Referenced object does not implement IProgressible");
            return;
        }
        
        progressible.progressChanged += Progressible_progressChanged;
    }

    private void Progressible_progressChanged(object sender, IProgressible.ProgressChangedEventArgs e)
    {
        float progressNormalized = e.currentProgress / e.maxProgress;
        barImage.fillAmount = progressNormalized;
        if (progressNormalized < 1) {
            Show();
        } else {
            Hide();
        }
    }

    public void Hide() {
        this.gameObject.SetActive(false);
    }

    public void Show() {
        this.gameObject.SetActive(true);
    }
}
