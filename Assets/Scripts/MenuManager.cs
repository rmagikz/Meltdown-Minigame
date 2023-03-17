using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject titleText;
    [SerializeField] Button play, settings, exit;
    [SerializeField] GameObject settingsMenu;

    void Start()
    {
        play.onClick.AddListener(Play);
        settings.onClick.AddListener(Settings);
        exit.onClick.AddListener(Exit);
    }

    void Play()
    {
        titleText.SetActive(false);
        gameObject.SetActive(false);
        GameManager.instance.Play();
    }

    void Settings()
    {
        gameObject.SetActive(false);
        settingsMenu.SetActive(true);
    }

    void Exit()
    {
        Application.Quit();
    }
}
