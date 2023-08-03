using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObjectScript : MonoBehaviour
{
    private Draw drawScript;

    private void Start()
    {
        // Draw ��ũ��Ʈ�� �����ϱ� ���� Draw ������Ʈ�� ã�ƿɴϴ�.
        drawScript = FindObjectOfType<Draw>();
    }

    private void OnTriggerEnter(Collider other)
    {

        // �浹�� ������Ʈ�� "paper" �±׸� ������ �ְ�, Draw ��ũ��Ʈ�� ������ ���
        if (other.tag == "paper" && drawScript != null)
        {
            Vector3 objectPosition = transform.position;
            drawScript.createLine(objectPosition); // Draw ��ũ��Ʈ�� createLine �Լ��� ȣ���մϴ�.
            //Debug.Log("�浹 ����");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // �浹�ϰ� �ִ� ������Ʈ�� "paper" �±׸� ������ �ְ�, Draw ��ũ��Ʈ�� ������ ���
        if (other.tag == "paper" && drawScript != null)
        {
            Vector3 objectPosition = transform.position;
            drawScript.connectLine(objectPosition); // Draw ��ũ��Ʈ�� connectLine �Լ��� ȣ���մϴ�.
            //Debug.Log("�浹 ��");
        }
    }
}
