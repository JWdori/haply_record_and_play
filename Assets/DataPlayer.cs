using UnityEngine;

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



    public float maxDistance = 0.2f; // �ִ� �Ÿ�
    public float maxForce = 1f; // �ִ� ��
    public AnimationCurve distanceCurve; // �Ÿ��� ���� �� �ǵ�� �





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
                targetObject.transform.position = newPosition;
                //Debug.Log(line+"\n"+"���� ���ǵ����"+ hapticController.forceX+ hapticController.forceY+ hapticController.forceZ);








                // �� ��ġ ������ �Ÿ� ���
                float distance = Vector3.Distance(targetObject.transform.position, transform.position);
                // �ִ� �Ÿ� �̻��̸� ���� 0���� ����
                if (distance >= maxDistance)
                {
                    hapticController.forceX = 1f;
                    hapticController.forceY = 1f;
                    hapticController.forceZ = 1f;
                }
                else
                {
                    // �Ÿ��� ���� �� �ǵ���� ���� �Լ��� ����Ͽ� ���
                    float normalizedDistance = distance / maxDistance;
                    float dampingFactor = distanceCurve.Evaluate(normalizedDistance);
                    hapticController.forceX = maxForce * dampingFactor * (posX - transform.position.x);
                    hapticController.forceY = maxForce * dampingFactor * (posY - transform.position.y);
                    hapticController.forceZ = maxForce * dampingFactor * (posZ - transform.position.z);
                }

                Debug.Log(distance);




            }
            else
            {
                Debug.LogError("CSV ������. ���� ������: " + line);
            }

            currentLineIndex++;
        }

        // �ν����Ϳ��� ���� ������ ������ ���� �ǽð����� �ݿ�
        position = targetObject.transform.position;
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
}
