using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DataPlayer : MonoBehaviour
{
    [HideInInspector]
    public GameObject targetObject; // ��ġ�� ������ ��� ������Ʈ�� ����
    public HapticController hapticController;
    //public PlaneForce hapticController;

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


    public float maxDistance = 1f; // �ִ� �Ÿ�
    public float maxForce = 3f; // �ִ� ��
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
                //Debug.Log(posX + "\n"+"���� ���ǵ����"+ hapticController.forceX+ hapticController.forceY+ hapticController.forceZ);
                CalculateForce(newPosition,time);


            }
            else
            {
                Debug.LogError("CSV ������. ���� ������: " + line);
            }


            currentLineIndex++;
        }

        // �ν����Ϳ��� ���� ������ ������ ���� �ǽð����� �ݿ�
        position = targetObject.transform.position;
        testposition = testtargetObject.transform.position;
        //position = transform.position;
    }

    public void Play()
    {
        testtargetObject.SetActive(true);
        startBtn = true;
        currentLineIndex = 1; // ������ �� ù ��° ������ ���κ��� ó���ϵ��� �ʱ�ȭ�մϴ�.
    }

    public void Stop()
    {
        startBtn = false;
        hapticController.forceX = 0;
        hapticController.forceY = 0.2f; //�⺻ �߷�
        hapticController.forceZ = 0;
    }


    private Vector3 CalculateForce(Vector3 targetPosition, float time)
    {
        Vector3 currentPosition = targetObject.transform.position;
        Vector3 distanceVector = targetPosition - currentPosition;
        Vector3 normalizedDistanceVector = new Vector3(
            Mathf.Clamp(distanceVector.x / maxDistance, -1f, 1f),
            Mathf.Clamp(distanceVector.y / maxDistance, -1f, 1f),
            Mathf.Clamp(distanceVector.z / maxDistance, -1f, 1f)
        );

        Vector3 forceVector = maxForce * Vector3.Scale(distanceVector, (Vector3.one - normalizedDistanceVector));
        forceVector = new Vector3(
            Mathf.Abs(distanceVector.x) < maxDistance ? forceVector.x : maxForce * Mathf.Sign(distanceVector.x),
            Mathf.Abs(distanceVector.y) < maxDistance ? forceVector.y : maxForce * Mathf.Sign(distanceVector.y),
            Mathf.Abs(distanceVector.z) < maxDistance ? forceVector.z : maxForce * Mathf.Sign(distanceVector.z)
        );
        Debug.Log(forceVector);
        if (currentLineIndex >= lines.Length - 1)
        {
            // CSV ������ ������ �о����Ƿ� �� �ǵ���� 0���� �ʱ�ȭ
            hapticController.forceX = 0;
            hapticController.forceY = 0.2f; //�⺻ �߷�
            hapticController.forceZ = 0;
        }
        else
        {
            hapticController.forceX = forceVector.x;
            hapticController.forceY = forceVector.y;
            hapticController.forceZ = forceVector.z;
        }


        UIText.text = time.ToString()
            + "\ndistanceX: " + distanceVector.x
            + "\ndistanceY: " + distanceVector.y
            + "\ndistanceZ: " + distanceVector.z
            + "\nforceX: " + forceVector.x
            + "\nforceY: " + forceVector.y
            + "\nforceZ: " + forceVector.z;

        return forceVector;
    }

}
