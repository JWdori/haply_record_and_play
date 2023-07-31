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

    private void Awake()
    {
        // CSV ���Ͽ��� �����͸� �о�ͼ� lines �迭�� �����մϴ�.
        if (csvFile != null)
            lines = csvFile.text.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
    }

    private void Update()
    {
        if (startBtn && lines != null && currentLineIndex < lines.Length)
        {
            string line = lines[currentLineIndex];
            string[] data = line.Split(new char[] { ',' });

            // �����͸� �Ľ��Ͽ� ������ �����մϴ�.
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
                Debug.Log(line);
                // ���⼭ hapticController.forceX �� �ٸ� ������ ������ �� �ֽ��ϴ�.
                if (hapticController.forceX == 0)
                {
                    hapticController.forceX = 0;
                }
                else
                {
                    hapticController.forceX = 1f;
                }
            }
            else
            {
                Debug.LogError("CSV �����͸� �Ľ��ϴ� �� ������ �߻��߽��ϴ�. ������ �߻��� ������: " + line);
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
    }
}
