using UnityEngine;
using UnityEngine.UI;

public class UILineConnector : MonoBehaviour
{
    [SerializeField] private RectTransform startImage;
    [SerializeField] private RectTransform endImage;
    
    private LineRenderer lineRenderer;
    private Canvas canvas;
    private RectTransform canvasRect;

    private void Awake()
    {
        // LineRenderer 컴포넌트 추가 및 설정
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 2f;
        lineRenderer.endWidth = 2f;
        lineRenderer.positionCount = 2;
        
        // 라인 렌더러의 머티리얼 설정
        Material lineMaterial = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.material = lineMaterial;
        
        // 캔버스 참조 가져오기
        canvas = GetComponentInParent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (startImage == null || endImage == null) return;
        
        UpdateLinePosition();
    }

    private void UpdateLinePosition()
    {
        // UI 좌표를 월드 좌표로 변환
        Vector3 startPos = GetWorldPositionFromRectTransform(startImage);
        Vector3 endPos = GetWorldPositionFromRectTransform(endImage);
        
        // 라인 위치 업데이트
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }

    private Vector3 GetWorldPositionFromRectTransform(RectTransform rectTransform)
    {
        // UI 요소의 중심점 구하기
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        Vector3 center = (corners[0] + corners[2]) / 2f;
        
        // UI 좌표를 월드 좌표로 변환
        return center;
    }

    // 연결할 이미지 설정 메서드
    public void SetConnectedImages(RectTransform start, RectTransform end)
    {
        startImage = start;
        endImage = end;
        UpdateLinePosition();
    }
} 