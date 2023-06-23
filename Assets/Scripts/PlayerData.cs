using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    public int stage;
    public int gold;
    public Text txtGold;
    public Text txtStage;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        Init();
        GameManager.NextStage += NextStage;
    }
    private void Init()
    {
        txtGold.text = $"{gold}";
        txtStage.text = $"Stage {stage}";
    }

    public void AddGold()
    {
        gold += 100;
        txtGold.text = $"{gold}";
    }

    public void AddGold(int money)
    {
        gold += money;
        txtGold.text = $"{gold}";
    }


    public void NextStage()
    {
        stage++;
        txtStage.text = $"Stage {stage}";
    }

}
