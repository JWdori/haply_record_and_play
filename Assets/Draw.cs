using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Draw : MonoBehaviour
{
    public Camera cam; // Main Camera 가져오기
    public Material defaultMaterial; // Line Renderer에 사용할 기본 머티리얼
    public Transform movingObject; // 움직이는 오브젝트를 할당하기 위한 변수

    private LineRenderer curLine; // 현재 그려지고 있는 라인
    private int positionCount = 2; // 초기 시작과 끝 지점
    private Vector3 PrevPos = Vector3.zero; // 초기 위치 (0, 0, 0)

    // Update is called once per frame
    void Update()
    {
        //DrawMouse();
        //CheckObjectMovement();
    }

    void DrawMouse()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.3f));

        if (Input.GetMouseButtonDown(0))
        {
            createLine(mousePos);
        }
        else if (Input.GetMouseButton(0))
        {
            connectLine(mousePos);
        }
    }

    public void createLine(Vector3 startPos)
    {
        positionCount = 2;
        GameObject line = new GameObject("Line");
        LineRenderer lineRend = line.AddComponent<LineRenderer>();

        line.transform.parent = cam.transform;
        line.transform.position = startPos;

        lineRend.startWidth = 0.01f;
        lineRend.endWidth = 0.01f;
        lineRend.numCornerVertices = 5;
        lineRend.numCapVertices = 5;
        lineRend.material = defaultMaterial;
        lineRend.SetPosition(0, startPos);
        lineRend.SetPosition(1, startPos);

        curLine = lineRend;
        PrevPos = startPos; // 라인 생성 시 이전 위치를 시작 위치로 설정
    }

    public void connectLine(Vector3 mousePos)
    {
        if (PrevPos != mousePos && Mathf.Abs(Vector3.Distance(PrevPos, mousePos)) >= 0.001f)
        {
            PrevPos = mousePos;
            positionCount++;
            curLine.positionCount = positionCount;
            curLine.SetPosition(positionCount - 1, mousePos);
        }
    }


}
