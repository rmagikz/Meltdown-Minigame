using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private PlayerController playerController;

    [SerializeField] private MenuManager menuManager;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameMusic;

    public Transform[] pedestals;

    public static GameManager instance;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        audioSource.clip = menuMusic;
        audioSource.volume = 0.5f;
        audioSource.Play();
    }

    public void Play()
    {
        cameraController.BeginChase(playerController.transform);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Spinner.ToggleSpin();
        RivalController.ToggleRivals();

        playerController.ToggleController();

        StartCoroutine(FadeTrack(gameMusic));
    }

    public void Pause(bool showUI = true)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Spinner.ToggleSpin();
        RivalController.ToggleRivals();
        
        playerController.ToggleController();
    
        if (showUI)
            menuManager.gameObject.SetActive(true);

        StartCoroutine(FadeTrack(menuMusic));
    }

    private IEnumerator FadeTrack(AudioClip track)
    {
        for (float i = 0; i <= 1f; i += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0.5f,0f,i);
            yield return new WaitForEndOfFrame();
        }
        
        audioSource.Stop();
        audioSource.clip = track;
        audioSource.Play();

        for (float i = 0; i <= 1f; i += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f,0.5f,i);
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

    public void ToggleAudio()
    {
        audioSource.mute = !audioSource.mute;
    }
}
