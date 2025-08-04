using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UILine : Graphic
{
    [System.Serializable]
    public class GradientSettings
    {
        public bool useGradient = false;
        public Color startColor = Color.white;
        public Color endColor = Color.white;
    }

    [System.Serializable]
    public class DashSettings
    {
        public bool useDash = false;
        public float dashSize = 10f;
        public float gapSize = 5f;
    }

    [System.Serializable]
    public class ArrowSettings
    {
        public bool useArrow = false;
        public float arrowSize = 20f;
        public float arrowAngle = 30f;
    }

    [System.Serializable]
    public class BezierSettings
    {
        public bool useBezier = false;
        [Range(0f, 1f)]
        public float curvature = 0.5f;
        public int segmentCount = 50;
    }

    [SerializeField] private RectTransform startPoint;
    [SerializeField] private RectTransform endPoint;
    [SerializeField] private float thickness = 2f;
    
    [Header("Line Style Settings")]
    [SerializeField] private DashSettings dashSettings = new DashSettings();
    [SerializeField] private BezierSettings bezierSettings = new BezierSettings();
    [SerializeField] private ArrowSettings arrowSettings = new ArrowSettings();
    [SerializeField] private GradientSettings gradientSettings = new GradientSettings();

    private readonly List<UIVertex> vertices = new List<UIVertex>();
    private readonly List<int> indices = new List<int>();

    protected override void Awake()
    {
        base.Awake();
        // UI 라인이 보이도록 기본 색상 설정
        color = Color.white;
        
        // RectTransform 크기를 Canvas 크기로 설정
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            rectTransform.sizeDelta = canvasRect.sizeDelta;
        }
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        vertices.Clear();
        indices.Clear();

        if (startPoint == null || endPoint == null) return;

        Vector2 start = GetCanvasSpacePoint(startPoint);
        Vector2 end = GetCanvasSpacePoint(endPoint);

        if (bezierSettings.useBezier)
        {
            DrawBezierLine(start, end, vh);
        }
        else
        {
            DrawStraightLine(start, end, vh);
        }

        if (arrowSettings.useArrow)
        {
            DrawArrow(end, GetLastSegmentDirection(), vh);
        }
    }

    private void DrawStraightLine(Vector2 start, Vector2 end, VertexHelper vh)
    {
        Vector2 direction = (end - start).normalized;
        Vector2 perpendicular = new Vector2(-direction.y, direction.x) * thickness * 0.5f;

        if (dashSettings.useDash)
        {
            DrawDashedLine(start, end, perpendicular, vh);
        }
        else
        {
            AddLineSegment(start, end, perpendicular, vh);
        }
    }

    private void DrawBezierLine(Vector2 start, Vector2 end, VertexHelper vh)
    {
        Vector2 midPoint = (start + end) * 0.5f;
        float curveOffset = Vector2.Distance(start, end) * bezierSettings.curvature;
        Vector2 controlPoint = midPoint + new Vector2(-end.y + start.y, end.x - start.x).normalized * curveOffset;

        Vector2 prevPoint = start;
        Vector2 prevDirection = (controlPoint - start).normalized;

        for (int i = 1; i <= bezierSettings.segmentCount; i++)
        {
            float t = i / (float)bezierSettings.segmentCount;
            Vector2 currentPoint = GetBezierPoint(start, controlPoint, end, t);
            Vector2 currentDirection = GetBezierDirection(start, controlPoint, end, t);

            Vector2 perpendicular = new Vector2(-currentDirection.y, currentDirection.x) * thickness * 0.5f;

            if (dashSettings.useDash)
            {
                float segmentLength = Vector2.Distance(prevPoint, currentPoint);
                DrawDashedLine(prevPoint, currentPoint, perpendicular, vh);
            }
            else
            {
                AddLineSegment(prevPoint, currentPoint, perpendicular, vh);
            }

            prevPoint = currentPoint;
            prevDirection = currentDirection;
        }
    }

    private void DrawDashedLine(Vector2 start, Vector2 end, Vector2 perpendicular, VertexHelper vh)
    {
        float length = Vector2.Distance(start, end);
        Vector2 direction = (end - start).normalized;
        float dashPlusGap = dashSettings.dashSize + dashSettings.gapSize;
        int dashCount = Mathf.FloorToInt(length / dashPlusGap);

        for (int i = 0; i < dashCount; i++)
        {
            float startOffset = i * dashPlusGap;
            float endOffset = startOffset + dashSettings.dashSize;

            Vector2 dashStart = start + direction * startOffset;
            Vector2 dashEnd = start + direction * Mathf.Min(endOffset, length);

            AddLineSegment(dashStart, dashEnd, perpendicular, vh);
        }

        // 마지막 대시 처리
        float remainingLength = length - (dashCount * dashPlusGap);
        if (remainingLength > 0)
        {
            Vector2 dashStart = start + direction * (dashCount * dashPlusGap);
            Vector2 dashEnd = end;
            AddLineSegment(dashStart, dashEnd, perpendicular, vh);
        }
    }

    private void DrawArrow(Vector2 position, Vector2 direction, VertexHelper vh)
    {
        float halfAngle = arrowSettings.arrowAngle * 0.5f * Mathf.Deg2Rad;
        float cos = Mathf.Cos(halfAngle);
        float sin = Mathf.Sin(halfAngle);

        Vector2 right = new Vector2(direction.x * cos + direction.y * sin,
                                 -direction.x * sin + direction.y * cos) * arrowSettings.arrowSize;
        Vector2 left = new Vector2(direction.x * cos - direction.y * sin,
                                  direction.x * sin + direction.y * cos) * arrowSettings.arrowSize;

        Vector2 tip = position;
        Vector2 baseRight = position - right;
        Vector2 baseLeft = position - left;

        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = gradientSettings.useGradient ? gradientSettings.endColor : color;

        // 화살표 정점 추가
        vertex.position = tip;
        vh.AddVert(vertex);
        vertex.position = baseRight;
        vh.AddVert(vertex);
        vertex.position = baseLeft;
        vh.AddVert(vertex);

        // 화살표 삼각형 추가
        int vertexCount = vh.currentVertCount;
        vh.AddTriangle(vertexCount - 3, vertexCount - 2, vertexCount - 1);
    }

    private void AddLineSegment(Vector2 start, Vector2 end, Vector2 perpendicular, VertexHelper vh)
    {
        UIVertex vertex = UIVertex.simpleVert;

        // 시작점의 두 정점
        vertex.color = gradientSettings.useGradient ? gradientSettings.startColor : color;
        vertex.position = new Vector3(start.x + perpendicular.x, start.y + perpendicular.y, 0);
        vh.AddVert(vertex);
        vertex.position = new Vector3(start.x - perpendicular.x, start.y - perpendicular.y, 0);
        vh.AddVert(vertex);

        // 끝점의 두 정점
        vertex.color = gradientSettings.useGradient ? gradientSettings.endColor : color;
        vertex.position = new Vector3(end.x + perpendicular.x, end.y + perpendicular.y, 0);
        vh.AddVert(vertex);
        vertex.position = new Vector3(end.x - perpendicular.x, end.y - perpendicular.y, 0);
        vh.AddVert(vertex);

        // 삼각형 추가
        int vertexCount = vh.currentVertCount;
        vh.AddTriangle(vertexCount - 4, vertexCount - 3, vertexCount - 2);
        vh.AddTriangle(vertexCount - 2, vertexCount - 3, vertexCount - 1);
    }

    private Vector2 GetBezierPoint(Vector2 start, Vector2 control, Vector2 end, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        return (uu * start) + (2 * u * t * control) + (tt * end);
    }

    private Vector2 GetBezierDirection(Vector2 start, Vector2 control, Vector2 end, float t)
    {
        float u = 1 - t;
        return (2 * u * (control - start) + 2 * t * (end - control)).normalized;
    }

    private Vector2 GetLastSegmentDirection()
    {
        if (bezierSettings.useBezier)
        {
            return GetBezierDirection(
                GetCanvasSpacePoint(startPoint),
                GetBezierControlPoint(),
                GetCanvasSpacePoint(endPoint),
                1f
            );
        }
        else
        {
            Vector2 start = GetCanvasSpacePoint(startPoint);
            Vector2 end = GetCanvasSpacePoint(endPoint);
            return (end - start).normalized;
        }
    }

    private Vector2 GetBezierControlPoint()
    {
        Vector2 start = GetCanvasSpacePoint(startPoint);
        Vector2 end = GetCanvasSpacePoint(endPoint);
        Vector2 midPoint = (start + end) * 0.5f;
        float curveOffset = Vector2.Distance(start, end) * bezierSettings.curvature;
        return midPoint + new Vector2(-end.y + start.y, end.x - start.x).normalized * curveOffset;
    }

    private Vector2 GetCanvasSpacePoint(RectTransform target)
    {
        // 캔버스 공간에서의 위치 계산
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas == null) return Vector2.zero;

        // 타겟의 중심점 위치를 캔버스 좌표계로 변환
        Vector3[] corners = new Vector3[4];
        target.GetWorldCorners(corners);
        Vector3 worldPos = (corners[0] + corners[2]) * 0.5f;
        
        // 스크린 좌표로 변환
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, worldPos);
        Vector2 localPoint;
        
        // 로컬 좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            screenPoint,
            canvas.worldCamera,
            out localPoint
        );

        return localPoint;
    }

    public void SetPoints(RectTransform start, RectTransform end)
    {
        startPoint = start;
        endPoint = end;
        SetVerticesDirty();
    }

    public void SetThickness(float newThickness)
    {
        thickness = newThickness;
        SetVerticesDirty();
    }

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();
        SetVerticesDirty();
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        SetVerticesDirty();
    }
#endif
} 