using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class MonsterCtrl : MonoBehaviour
{
    public float lifeTime;
    public int level;
    public float addforcePower;

    //두번 트리거 되는 현상때문에 임시변수 선언
    [HideInInspector]
    public bool canBattle = true;


    enum EMonsterType
    {
        Minion,
        Boss,
    }
    [SerializeField]
    private EMonsterType monsterType;
    [SerializeField]
    private Text txtLevel;
    [SerializeField]
    private GameObject itemObj;
    [Tooltip("1~Count만큼 아이템 드랍")]
    [SerializeField]
    private int dropItemCount;
    [SerializeField]
    private GameObject particleDie;
    private PlayerCtrl player;
    private Animator anim;
    private CapsuleCollider col;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        col = GetComponent<CapsuleCollider>();
    }

    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerCtrl>();
        txtLevel.text = $"Lv.{level}";
    }

    void Update()
    {
        if (this.level > player.Level)
        {
            txtLevel.color = Color.red;
        }
        else
        {
            txtLevel.color = Color.green;
        }
    }

    public void Battle(Transform target)
    {
        transform.LookAt(target.position);
        anim.SetBool("Attack", true);
    }
    public void BattleWin()
    {
        anim.SetBool("Attack", false);
    }

    public void Die(Transform playerTr)
    {
        anim.SetTrigger("Die");
        gameObject.layer = LayerMask.NameToLayer("DieMonster");
        GetComponent<Rigidbody>().AddForce((-transform.forward + transform.up) * addforcePower);
        // col.enabled = false;
        txtLevel.enabled = false;
        CreateDieParticle();
        DropItem();
        if (monsterType == EMonsterType.Boss)
        {
            GameManager.CallStageClear();
        }
    }

    private void DropItem()
    {
        int itemCount = Random.Range(1, dropItemCount + 1);

        for (int i = 0; i < itemCount; i++)
        {
            Instantiate<GameObject>(itemObj, transform.position, Quaternion.Euler(-90f, 0f, 0f));
        }
    }

    //몬스터 크기에 따라서 파티클 생성위치 변경
    private void CreateDieParticle()
    {
        switch (monsterType)
        {
            case EMonsterType.Minion:
                Instantiate<GameObject>(particleDie, transform.position + new Vector3(0f, 0.8f, 0f), Quaternion.identity);
                break;
            case EMonsterType.Boss:
                Instantiate<GameObject>(particleDie, transform.position + new Vector3(0f, 1.4f, 0f), Quaternion.identity);
                break;
        }
    }

}
