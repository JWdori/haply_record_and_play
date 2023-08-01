using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DataPlayer : MonoBehaviour
{
    [HideInInspector]
    public GameObject targetObject; // ��ġ�� ������ ��� ������Ʈ�� ����
    public HapticController hapticController;
    [HideInInspector]
    public Vector3 position; // �ν����Ϳ��� ���� ������ ������ ����
    private bool startBtn = false;
    [HideInInspector]
    public TextAsset csvFile; // csv ������ �Ҵ���� ����

    private string[] lines; // CSV ������ ���ε��� ������ �迭
    private int currentLineIndex = 1; // CSV ������ �Ľ��� �� ���� ó�� ���� ������ �ε���

    [HideInInspector]
    public GameObject testtargetObject;
    [HideInInspector]
    public Vector3 testposition; // �ν����Ϳ��� ���� ������ ������ ����

    public TMP_Text UIText; //UI


    public float maxDistance = 0.2f; // �ִ� �Ÿ�
    public float maxForce = 1f; // �ִ� ��
    //public AnimationCurve distanceCurve; // �Ÿ��� ���� �� �ǵ�� �
    //�̰� �ʿ� ������... �������ε�




    private void Awake()
    {
        // CSV ���Ͽ��� �����͸� �о�ͼ� lines �迭�� ����
        if (csvFile != null)
            lines = csvFile.text.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
    }

    private void Update()
    {
        if (startBtn && lines != null && currentLineIndex < lines.Length)
        {
            string line = lines[currentLineIndex];
            string[] data = line.Split(new char[] { ',' });

            // �����͸� �Ľ��Ͽ� ������ ����
            if (data.Length >= 8 &&
                float.TryParse(data[1], out float time) &&
                float.TryParse(data[2], out float posX) &&
                float.TryParse(data[3], out float posY) &&
                float.TryParse(data[4], out float posZ) &&
                float.TryParse(data[5], out float velX) &&
                float.TryParse(data[6], out float velY) &&
                float.TryParse(data[7], out float velZ))
            {
                Vector3 newPosition = new Vector3(posX, posY, posZ);
                testtargetObject.transform.position = newPosition;
                Debug.Log(posX + "\n"+"���� ���ǵ����"+ hapticController.forceX+ hapticController.forceY+ hapticController.forceZ);
                CalculateForce(newPosition,time);

            }
            else
            {
                Debug.LogError("CSV ������. ���� ������: " + line);
            }
            if (currentLineIndex >= lines.Length-1)
            {
                // CSV ������ ������ �о����Ƿ� �� �ǵ���� 0���� �ʱ�ȭ
               hapticController.forceX = 0f;
               hapticController.forceY = 0f;
               hapticController.forceZ = 0f;
                return;
            }

            currentLineIndex++;
        }

        // �ν����Ϳ��� ���� ������ ������ ���� �ǽð����� �ݿ�
        position = targetObject.transform.position;
        //position = transform.position;
    }

    public void Play()
    {
        startBtn = true;
        currentLineIndex = 1; // ������ �� ù ��° ������ ���κ��� ó���ϵ��� �ʱ�ȭ�մϴ�.
    }

    public void Stop()
    {
        startBtn = false;
        hapticController.forceX = 0;
        hapticController.forceY = 0;
        hapticController.forceZ = 0;
    }


    private void CalculateForce(Vector3 targetPosition, float time)
    {
        Vector3 currentPosition = targetObject.transform.position;
        Debug.Log(targetPosition.x + "CSV��");
        Debug.Log(currentPosition.x+"������");
        // X ��ǥ�� ���� �� �ǵ�� ���
        float distanceX = Mathf.Abs(targetPosition.x - currentPosition.x);
        float forceX = 0f;
        if (distanceX < maxDistance)
        {
            float normalizedDistanceX = distanceX / maxDistance;
            forceX = maxForce * (targetPosition.x - currentPosition.x) * (1f - normalizedDistanceX);
        }

        // Y ��ǥ�� ���� �� �ǵ�� ���
        float distanceY = Mathf.Abs(targetPosition.y - currentPosition.y);
        float forceY = 0f;
        if (distanceY < maxDistance)
        {
            float normalizedDistanceY = distanceY / maxDistance;
            forceY = maxForce * (targetPosition.y - currentPosition.y) * (1f - normalizedDistanceY);
        }

        // Z ��ǥ�� ���� �� �ǵ�� ���
        float distanceZ = Mathf.Abs(targetPosition.z - currentPosition.z);
        float forceZ = 0f;
        if (distanceZ < maxDistance)
        {
            float normalizedDistanceZ = distanceZ / maxDistance;
            forceZ = maxForce * (targetPosition.z - currentPosition.z) * (1f - normalizedDistanceZ);
        }

        // �� �ǵ�� ����
        hapticController.forceX = forceX;
        hapticController.forceY = forceY;
        hapticController.forceZ = forceZ;
        UIText.text = time.ToString() + "\ndistanceX: " + distanceX.ToString() + "\ndistanceY: " + distanceY.ToString() +
            "\ndistanceZ: " + distanceZ.ToString()
            + "\nforceX: " + forceX.ToString() + "\nforceY: " + forceY.ToString() + "\nforceZ: " + forceZ.ToString()


            ;

    }
}
