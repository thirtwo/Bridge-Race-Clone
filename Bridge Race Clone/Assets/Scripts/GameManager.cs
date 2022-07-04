using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Thirtwo.Character;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static event Action OnGameStarted;
    public static event Action<bool> OnGameFinished;
    public static bool isGameStarted = false;
    public static bool isGameFinished = false;
    public static int scoreMultiplier = 0;
    private static int money = 0;
    [SerializeField] private Transform[] finishPoints;
    private static Transform _winner;
    public static int Money
    {
        get { return money; }
        set
        {
            money = value;
        }
    }
    private void Awake()
    {
        isGameStarted = false;
        isGameFinished = false;
        OnGameFinished += GameManager_OnGameFinished;
    }

    private void GameManager_OnGameFinished(bool obj)
    {
        _winner.position = finishPoints[0].position + Vector3.up;
        finishPoints[0].GetComponent<MeshRenderer>().material = _winner.GetChild(0).GetComponentInChildren<SkinnedMeshRenderer>().material;
        var chars = FindObjectsOfType<CharController>();
        var transforms = new List<Transform>();
        for (int i = 0; i < chars.Length; i++)
        {
            if (chars[i].transform == _winner) continue;
            transforms.Add(chars[i].transform);
        }
        transforms = transforms.OrderBy(t => Vector3.Distance(t.position, _winner.position)).ToList();
        transforms[0].position = finishPoints[1].position + Vector3.up * 2;
        transforms[1].position = finishPoints[2].position + Vector3.up * 2;
        finishPoints[1].GetComponent<MeshRenderer>().material = transforms[0].GetChild(0).GetComponentInChildren<SkinnedMeshRenderer>().material;
        finishPoints[2].GetComponent<MeshRenderer>().material = transforms[1].GetChild(0).GetComponentInChildren<SkinnedMeshRenderer>().material;

        StartCoroutine(Co_CamAnim());
    }
    private IEnumerator Co_CamAnim()
    {
        var cam = Camera.main;
        cam.GetComponent<CameraFollow>().smoothness++;
        yield return new WaitForSeconds(0.5f);
        cam.GetComponent<CameraFollow>().smoothness++;
        yield return new WaitForSeconds(0.5f);
        cam.GetComponent<CameraFollow>().enabled = false;
        cam.transform.DOMoveZ(cam.transform.position.z - 30, 3.5f);
    }
  
    public static void StartGame()
    {
        if (isGameStarted) return;
        isGameStarted = true;
        OnGameStarted?.Invoke();
    }
    public static void FinishGame(bool isWin, Transform winner)
    {
        if (isGameFinished) return;
        isGameFinished = true;
        _winner = winner;
        OnGameFinished?.Invoke(isWin);
    }

    public void NextLevel()
    {
        var index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);//only have one scene
    }
    public void Retry()
    {
        var index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);
    }
}
