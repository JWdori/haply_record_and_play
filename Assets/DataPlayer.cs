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



    public float maxDistance = 0.2f; // 최대 거리
    public float maxForce = 1f; // 최대 힘
    public AnimationCurve distanceCurve; // 거리에 따른 힘 피드백 곡선





    private void Awake()
    {
        // CSV 파일에서 데이터를 읽어와서 lines 배열에 저장
        if (csvFile != null)
            lines = csvFile.text.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
    }

    private void Update()
    {
        if (startBtn && lines != null && currentLineIndex < lines.Length)
        {
            string line = lines[currentLineIndex];
            string[] data = line.Split(new char[] { ',' });

            // 데이터를 파싱하여 변수에 저장
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
                //Debug.Log(line+"\n"+"현재 힘피드백은"+ hapticController.forceX+ hapticController.forceY+ hapticController.forceZ);








                // 두 위치 사이의 거리 계산
                float distance = Vector3.Distance(targetObject.transform.position, transform.position);
                // 최대 거리 이상이면 힘을 0으로 설정
                if (distance >= maxDistance)
                {
                    hapticController.forceX = 1f;
                    hapticController.forceY = 1f;
                    hapticController.forceZ = 1f;
                }
                else
                {
                    // 거리에 따라 힘 피드백을 감쇠 함수를 사용하여 계산
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
                Debug.LogError("CSV ㄹ오류. 오류 데이터: " + line);
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
        hapticController.forceY = 0;
        hapticController.forceZ = 0;
    }
}
