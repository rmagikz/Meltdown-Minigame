using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] Button mainMenu;

    void Start()
    {
        mainMenu.onClick.AddListener(() => SceneManager.LoadScene(0));
    }
}
