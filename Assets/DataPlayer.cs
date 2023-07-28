using UnityEngine;

public class DataPlayer : MonoBehaviour
{
    [HideInInspector]
    public GameObject targetObject; // ��ġ�� ������ ��� ������Ʈ�� ����
    public HapticController hapticController;
    [HideInInspector]
    public Vector3 position; // �ν����Ϳ��� ���� ������ ������ ����

    private void Update()
    {

        // �ν����Ϳ��� ���� ������ ������ ���� �ǽð����� �ݿ��մϴ�.
        position = targetObject.transform.position;
        // forceX ���� ����� ������ ����մϴ�.
        Debug.Log("ForceX: " + hapticController.forceX + "x�����ǵ��");
        // Ÿ�� ������Ʈ�� ���� ������ ���� ����մϴ�.
        Debug.Log("Position: " + position);
    }

    public void Play()
    {
        if (targetObject == null)
        {
            Debug.LogError("Ÿ�� ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        // Ÿ�� ������Ʈ�� ���� ������ ���� �����ɴϴ�.
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
