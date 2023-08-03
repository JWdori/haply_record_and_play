using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Draw : MonoBehaviour
{
    public Camera cam; // Main Camera ��������
    public Material defaultMaterial; // Line Renderer�� ����� �⺻ ��Ƽ����
    public Transform movingObject; // �����̴� ������Ʈ�� �Ҵ��ϱ� ���� ����

    private LineRenderer curLine; // ���� �׷����� �ִ� ����
    private int positionCount = 2; // �ʱ� ���۰� �� ����
    private Vector3 PrevPos = Vector3.zero; // �ʱ� ��ġ (0, 0, 0)

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
        PrevPos = startPos; // ���� ���� �� ���� ��ġ�� ���� ��ġ�� ����
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
