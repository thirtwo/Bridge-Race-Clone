using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject tutorialCanvas;
    [SerializeField] private GameObject endGameCanvas;

    private void Start()
    {
        GameManager.OnGameStarted += GameManager_OnGameStarted;
        GameManager.OnGameFinished += GameManager_OnGameFinished;
    }

    private void GameManager_OnGameStarted()
    {
        tutorialCanvas.SetActive(false);
    }

    private void GameManager_OnGameFinished(bool obj)
    {
        endGameCanvas.SetActive(true);
    }
}
