using UnityEngine;

public class DataPlayer : MonoBehaviour
{
    [HideInInspector]
    public GameObject targetObject; // 위치를 가져올 대상 오브젝트의 참조
    public HapticController hapticController;
    [HideInInspector]
    public Vector3 position; // 인스펙터에서 수정 가능한 포지션 변수

    private void Update()
    {

        // 인스펙터에서 직접 수정한 포지션 값을 실시간으로 반영합니다.
        position = targetObject.transform.position;
        // forceX 값이 변경될 때마다 출력합니다.
        Debug.Log("ForceX: " + hapticController.forceX + "x포스피드백");
        // 타겟 오브젝트의 현재 포지션 값을 출력합니다.
        Debug.Log("Position: " + position);
    }

    public void Play()
    {
        if (targetObject == null)
        {
            Debug.LogError("타겟 오브젝트가 할당되지 않았습니다.");
            return;
        }

        // 타겟 오브젝트의 현재 포지션 값을 가져옵니다.
        Vector3 targetPosition = targetObject.transform.position;

        if (targetPosition.x == 0)
        {
            hapticController.forceX = 0;
        }
        else if (targetPosition.x >= 0.3f)
        {
            hapticController.forceX = -1;
        }
        else if (targetPosition.x == 0.1f)
        {
            hapticController.forceX = -2;
        }
        else
        {
            hapticController.forceX = 2;
        }

    }
}
