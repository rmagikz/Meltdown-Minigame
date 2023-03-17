using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicLiquid : MonoBehaviour
{
    [SerializeField] GameOver gameOver;

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.name != "Player") return;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GameManager.instance.Pause(false);
        gameOver.gameObject.SetActive(true);
    }
}
