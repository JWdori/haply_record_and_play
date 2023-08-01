using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [HideInInspector]
    public GameObject testtargetObject;
    [HideInInspector]
    public Vector3 testposition; // 인스펙터에서 수정 가능한 포지션 변수

    public TMP_Text UIText; //UI


    public float maxDistance = 0.2f; // 최대 거리
    public float maxForce = 1f; // 최대 힘
    //public AnimationCurve distanceCurve; // 거리에 따른 힘 피드백 곡선
    //이거 필요 없을듯... 보간법인데




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
                testtargetObject.transform.position = newPosition;
                Debug.Log(posX + "\n"+"현재 힘피드백은"+ hapticController.forceX+ hapticController.forceY+ hapticController.forceZ);
                CalculateForce(newPosition,time);

            }
            else
            {
                Debug.LogError("CSV ㄹ오류. 오류 데이터: " + line);
            }
            if (currentLineIndex >= lines.Length-1)
            {
                // CSV 파일의 끝까지 읽었으므로 힘 피드백을 0으로 초기화
               hapticController.forceX = 0f;
               hapticController.forceY = 0f;
               hapticController.forceZ = 0f;
                return;
            }

            currentLineIndex++;
        }

        // 인스펙터에서 직접 수정한 포지션 값을 실시간으로 반영
        position = targetObject.transform.position;
        //position = transform.position;
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


    private void CalculateForce(Vector3 targetPosition, float time)
    {
        Vector3 currentPosition = targetObject.transform.position;
        Debug.Log(targetPosition.x + "CSV값");
        Debug.Log(currentPosition.x+"실제값");
        // X 좌표에 대한 힘 피드백 계산
        float distanceX = Mathf.Abs(targetPosition.x - currentPosition.x);
        float forceX = 0f;
        if (distanceX < maxDistance)
        {
            float normalizedDistanceX = distanceX / maxDistance;
            forceX = maxForce * (targetPosition.x - currentPosition.x) * (1f - normalizedDistanceX);
        }

        // Y 좌표에 대한 힘 피드백 계산
        float distanceY = Mathf.Abs(targetPosition.y - currentPosition.y);
        float forceY = 0f;
        if (distanceY < maxDistance)
        {
            float normalizedDistanceY = distanceY / maxDistance;
            forceY = maxForce * (targetPosition.y - currentPosition.y) * (1f - normalizedDistanceY);
        }

        // Z 좌표에 대한 힘 피드백 계산
        float distanceZ = Mathf.Abs(targetPosition.z - currentPosition.z);
        float forceZ = 0f;
        if (distanceZ < maxDistance)
        {
            float normalizedDistanceZ = distanceZ / maxDistance;
            forceZ = maxForce * (targetPosition.z - currentPosition.z) * (1f - normalizedDistanceZ);
        }

        // 힘 피드백 적용
        hapticController.forceX = forceX;
        hapticController.forceY = forceY;
        hapticController.forceZ = forceZ;
        UIText.text = time.ToString() + "\ndistanceX: " + distanceX.ToString() + "\ndistanceY: " + distanceY.ToString() +
            "\ndistanceZ: " + distanceZ.ToString()
            + "\nforceX: " + forceX.ToString() + "\nforceY: " + forceY.ToString() + "\nforceZ: " + forceZ.ToString()


            ;

    }
}
