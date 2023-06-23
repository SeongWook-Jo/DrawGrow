using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System;

public class PlayerCtrl : MonoBehaviour
{
    public event Action ArriveLastDestination = null;
    enum EPlayerState
    {
        None,
        Idle,
        Run,
        Battle,
        Die,
    }

    [Header("State")]
    [SerializeField]
    private EPlayerState playerState = EPlayerState.Idle;
    [SerializeField]
    private int level = 1;
    public int Level
    {
        get { return level; }
    }
    [SerializeField]
    private Color levelTxtColor;
    [SerializeField]
    private Text txtLevel;
    private Vector3[] movePoints;
    private LineRenderer lineRenderer;

    [Space(20)]
    [Header("Weapon")]
    [SerializeField]
    private TrailRenderer weaponTrailRenderer;
    [SerializeField]
    private GameObject particleTreasure;
    [SerializeField]
    private GameObject defaultWeapon;
    [SerializeField]
    private GameObject treasureWeapon;

    private Animator anim;
    private NavMeshAgent nav;
    private GameManager gameManager;
    private Transform myTr;
    private int currPointIdx = 0;
    private int lastPointIdx = 0;



    private void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        lineRenderer = GameObject.FindObjectOfType<LineRenderer>();
        myTr = transform;
    }

    void Start()
    {

        LineCtrl.DrawEnd += SetPositions;
        txtLevel.color = levelTxtColor;
    }

    void Update()
    {
        AnimationSync();
        txtLevel.color = levelTxtColor;

        if (GameManager.GameState == EGameState.Ready)
        {
            playerState = EPlayerState.Idle;
        }
        else if (GameManager.GameState == EGameState.Playing)
        {
            bool canMove = !(playerState == EPlayerState.Battle || playerState == EPlayerState.Die);

            if (!canMove) return;

            nav.SetDestination(movePoints[currPointIdx]);
            playerState = EPlayerState.Run;

            bool isArrive = Vector3.Distance(myTr.position, movePoints[currPointIdx]) < 0.5f;

            if (isArrive)
            {
                if (lastPointIdx - 1 > currPointIdx)
                {
                    currPointIdx++;
                }
                else
                {
                    if (playerState != EPlayerState.Battle)
                    {
                        playerState = EPlayerState.Idle;
                        ArriveLastDestination?.Invoke();
                    }
                }
            }
        }
        else
        {
            playerState = EPlayerState.Idle;
        }

    }

    public void LevelChange(int addLevel)
    {
        level += addLevel;
        txtLevel.text = $"Lv.{level}";
    }

    public void Battle(MonsterCtrl monster)
    {
        myTr.LookAt(monster.transform);
        monster.Battle(myTr);
        playerState = EPlayerState.Battle;
        StartCoroutine(CoroutineBattle(monster));
    }
    //무기 변경 부분
    public void GetTreasure()
    {
        Instantiate<GameObject>(particleTreasure, transform.position, Quaternion.identity, parent: transform);
        LevelChange(5);
        nav.speed += 5;

        //무기 변경 구현 예정.
        defaultWeapon.SetActive(false);
        treasureWeapon.SetActive(true);
        weaponTrailRenderer = treasureWeapon.GetComponentInChildren<TrailRenderer>();
    }

    public void BattleEnd()
    {
        playerState = EPlayerState.Run;
    }

    private void AnimationSync()
    {
        switch (playerState)
        {
            case EPlayerState.Idle:
                anim.SetBool("Run", false);
                anim.SetBool("Attack", false);
                weaponTrailRenderer.enabled = false;
                break;
            case EPlayerState.Run:
                anim.SetBool("Run", true);
                anim.SetBool("Attack", false);
                weaponTrailRenderer.enabled = false;
                break;
            case EPlayerState.Battle:
                anim.SetBool("Attack", true);
                weaponTrailRenderer.enabled = true;
                break;
            case EPlayerState.Die:
                anim.SetTrigger("Die");
                weaponTrailRenderer.enabled = false;
                playerState = EPlayerState.None;
                GameManager.CallStageFail();
                break;
            default:
                break;
        }
    }

    //라인 렌더러 포지션 가져오는 부분
    private void SetPositions()
    {
        lastPointIdx = lineRenderer.positionCount;
        movePoints = new Vector3[lastPointIdx];
        lineRenderer.GetPositions(movePoints);
        currPointIdx = 0;
    }

    private void Die()
    {
        playerState = EPlayerState.Die;
    }

    IEnumerator CoroutineBattle(MonsterCtrl monster)
    {
        yield return new WaitForSeconds(monster.lifeTime);
        if (monster.level > this.level)
        {
            this.Die();
            monster.BattleWin();
        }
        else
        {
            BattleEnd();
            monster.Die(myTr);
            LevelChange(monster.level);
        }
    }


}
