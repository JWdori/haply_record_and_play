using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObjectScript : MonoBehaviour
{
    private Draw drawScript;

    private void Start()
    {
        // Draw 스크립트에 접근하기 위해 Draw 컴포넌트를 찾아옵니다.
        drawScript = FindObjectOfType<Draw>();
    }

    private void OnCollisionEnter(Collision collision)
    {

        // 충돌한 오브젝트가 "paper" 태그를 가지고 있고, Draw 스크립트가 존재할 경우
        if (collision.gameObject.CompareTag("paper"))
        {
            Vector3 objectPosition = transform.position;
            drawScript.createLine(objectPosition); // Draw 스크립트의 createLine 함수를 호출합니다.
            //Debug.Log("충돌 감지");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // 충돌하고 있는 오브젝트가 "paper" 태그를 가지고 있고, Draw 스크립트가 존재할 경우
        if (collision.gameObject.CompareTag("paper"))
        {
            Vector3 objectPosition = transform.position;
            drawScript.connectLine(objectPosition); // Draw 스크립트의 connectLine 함수를 호출합니다.
        }
    }
}
