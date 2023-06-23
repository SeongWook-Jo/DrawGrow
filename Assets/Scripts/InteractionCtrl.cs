using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionCtrl : MonoBehaviour
{
    PlayerCtrl player;

    private void Awake()
    {
        player = GetComponent<PlayerCtrl>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            MonsterCtrl monster = other.GetComponent<MonsterCtrl>();

            if (!monster.canBattle) return;
            monster.canBattle = false;
            player.Battle(monster);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Treasure"))
        {
            player.GetTreasure();
            Destroy(other.gameObject);
        }
    }
}
