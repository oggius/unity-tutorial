using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter counter;
    [SerializeField] private GameObject selectedVisual;

    // Start is called before the first frame update
    private void Start()
    {
        Player.Instance.selectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    // Handles selectedCounterChanged event
    private void Player_OnSelectedCounterChanged(object sender, Player.SelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == counter) {
            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        selectedVisual.SetActive(true);
    }

    private void Hide() {
        selectedVisual.SetActive(false);
    }
}
