using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public enum EGameState
{
    Ready = 0,
    Draw,
    Playing,
    Fail,
    End,
}

public class GameManager : MonoBehaviour
{

    public static event Action StageClear = null;
    public static event Action StageFail = null;
    public static event Action GameStart = null;
    public static event Action NextStage = null;

    private PlayerData playerDate;
    private static EGameState gameState = EGameState.Ready;
    public static EGameState GameState => gameState;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (gameState == EGameState.Ready)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameState = EGameState.Draw;
                GameStart?.Invoke();
            }
        }
    }

    public void OnClickNextStage()
    {
        gameState = EGameState.Ready;
        SceneManager.LoadScene($"Stage{playerDate.stage + 1}");
        NextStage?.Invoke();
    }

    public static void CallStageClear()
    {
        StageClear?.Invoke();
        gameState = EGameState.End;
    }

    public static void CallStageFail()
    {
        gameState = EGameState.Fail;
        StageFail?.Invoke();
    }

    void Init()
    {
        GameObject.FindObjectOfType<PlayerCtrl>().ArriveLastDestination += MoveEnd;
        LineCtrl.DrawEnd += EndLineDraw;
        playerDate = GameObject.FindObjectOfType<PlayerData>();
    }

    private void MoveEnd()
    {
        if (gameState == EGameState.End) return;
        StartCoroutine(CoroutineFail());

    }
    IEnumerator CoroutineFail()
    {
        yield return new WaitForSeconds(0.6f);
        StageFail?.Invoke();
        gameState = EGameState.Fail;
    }

    private void EndLineDraw()
    {
        gameState = EGameState.Playing;
    }

}
