using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class LineCtrl : MonoBehaviour
{
    public static event Action DrawEnd = null;

    [HideInInspector]
    public LineRenderer lineRend;

    [SerializeField]
    private PlayerCtrl player;
    private Transform playerTr;
    [SerializeField]
    private float lineDist;
    [SerializeField]
    private Transform targetForZDepth;
    private Vector3 prePosition;
    private Vector3 currPosition;

    private float zDepth;
    private int lineIdx = 0;
    private Camera cam;

    private Vector3 mousePo;

    private void Awake()
    {
        lineRend = GetComponent<LineRenderer>();
        cam = Camera.main;
    }
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerCtrl>();
        zDepth = Vector3.Distance(cam.transform.position, targetForZDepth.position);
        playerTr = GameObject.FindObjectOfType<PlayerCtrl>().transform;
    }

    void Update()
    {
        bool canDraw = GameManager.GameState == EGameState.Draw;

        if (!canDraw) return;

        if (Input.GetMouseButtonDown(0))
        {
            lineRend.enabled = true;
            lineIdx = 0;
            lineRend.positionCount = lineIdx + 1;
            prePosition = playerTr.position;
            lineRend.SetPosition(lineIdx++, prePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 5000f, 1 << LayerMask.NameToLayer("Ground")))
            {
                Vector3 drawPoint = hit.point + new Vector3(0f, 0.1f, 0f);
                if (Vector3.Distance(prePosition, drawPoint) > 0.1f)
                {
                    lineRend.positionCount = lineIdx + 1;
                    lineRend.SetPosition(lineIdx++, drawPoint);
                    prePosition = drawPoint;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            DrawEnd?.Invoke();
        }
    }

}


