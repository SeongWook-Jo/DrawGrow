using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Main : MonoBehaviour
{
    [SerializeField]
    private GameObject readyUI;
    [SerializeField]
    private GameObject clearUI;
    [SerializeField]
    private GameObject bg;
    [SerializeField]
    private GameObject failUI;
    [SerializeField]
    private Button btnLevelUp;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        GameManager.GameStart += () =>
        {
            bg.SetActive(false);
            readyUI.SetActive(false);
        };
        GameManager.StageClear += () =>
        {
            bg.SetActive(true);
            clearUI.SetActive(true);
        };
        GameManager.StageFail += () =>
        {
            bg.SetActive(true);
            failUI.SetActive(true);
        };
        btnLevelUp.onClick.AddListener(OnClickLevelUp);
    }

    public void OnClickLevelUp()
    {
        GameObject.FindObjectOfType<PlayerCtrl>().LevelChange(1);
        GameObject.FindObjectOfType<PlayerData>().AddGold(-200);
    }

}
