using UnityEngine;

public class DataPlayer : MonoBehaviour
{
    [HideInInspector]
    public GameObject targetObject; // 위치를 가져올 대상 오브젝트의 참조
    public HapticController hapticController;
    [HideInInspector]
    public Vector3 position; // 인스펙터에서 수정 가능한 포지션 변수
    private bool startBtn = false;
    [HideInInspector]
    public TextAsset csvFile; // csv 파일을 할당받을 변수

    private string[] lines; // CSV 파일의 라인들을 저장할 배열
    private int currentLineIndex = 1; // CSV 파일을 파싱할 때 현재 처리 중인 라인의 인덱스

    private void Awake()
    {
        // CSV 파일에서 데이터를 읽어와서 lines 배열에 저장합니다.
        if (csvFile != null)
            lines = csvFile.text.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
    }

    private void Update()
    {
        if (startBtn && lines != null && currentLineIndex < lines.Length)
        {
            string line = lines[currentLineIndex];
            string[] data = line.Split(new char[] { ',' });

            // 데이터를 파싱하여 변수에 저장합니다.
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
                // 여기서 hapticController.forceX 등 다른 동작을 수행할 수 있습니다.
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
                Debug.LogError("CSV 데이터를 파싱하는 중 오류가 발생했습니다. 오류가 발생한 데이터: " + line);
            }

            currentLineIndex++;
        }

        // 인스펙터에서 직접 수정한 포지션 값을 실시간으로 반영
        position = targetObject.transform.position;
    }

    public void Play()
    {
        startBtn = true;
        currentLineIndex = 1; // 시작할 때 첫 번째 데이터 라인부터 처리하도록 초기화합니다.
    }

    public void Stop()
    {
        startBtn = false;
        hapticController.forceX = 0;
    }
}
