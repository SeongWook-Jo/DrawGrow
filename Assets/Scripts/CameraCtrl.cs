using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    [SerializeField]
    Transform playerTr;

    [SerializeField]
    Vector3 defaultPosition;
    [SerializeField]
    float defaultRotX;
    [SerializeField]
    Vector3 playOffset;
    [SerializeField]
    float playRotX;
    Transform myTr;

    Vector3 vel = Vector3.zero;

    private void Awake()
    {
        myTr = transform;
        playerTr = GameObject.FindObjectOfType<PlayerCtrl>().transform;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void LateUpdate()
    {
        if (GameManager.GameState == EGameState.Playing)
        {
            myTr.position = Vector3.SmoothDamp(myTr.position, playerTr.position + playOffset, ref vel, 0.5f);
        }
        else
        {
            myTr.position = Vector3.SmoothDamp(myTr.position, defaultPosition, ref vel, 0.5f);
        }
    }
}
