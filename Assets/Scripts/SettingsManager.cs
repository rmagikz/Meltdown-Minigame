using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] Button toggleAudio, back;
    [SerializeField] Slider audioSlider;
    [SerializeField] PlayerController playerController;

    void Start()
    {
        toggleAudio.onClick.AddListener(ToggleAudio);
        back.onClick.AddListener(Back);
        audioSlider.onValueChanged.AddListener(UpdateSensitivity);
    }

    void ToggleAudio()
    {
        GameManager.instance.ToggleAudio();
    }

    void Back()
    {
        gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }

    void UpdateSensitivity(float value)
    {
        playerController.SetSensitivity(value);
    }
}
