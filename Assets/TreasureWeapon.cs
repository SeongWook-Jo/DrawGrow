using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureWeapon : MonoBehaviour
{
    public Transform weaponTr;
    public float rotSpeed;

    void Update()
    {
        weaponTr.eulerAngles += new Vector3(0f, Time.deltaTime * rotSpeed, 0f);
    }
}
