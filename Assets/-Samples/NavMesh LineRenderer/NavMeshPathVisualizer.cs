using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshPathVisualizer : MonoBehaviour
{
    private Vector3 startPoint;
    private Vector3 endPoint;
    private bool startPointSet = false;
    private bool endPointSet = false;

    private NavMeshPath path;
    private LineRenderer lineRenderer;
    
    [Header("Path Visualization")]
    [SerializeField] private float sphereScale = 50.0f;
    [SerializeField] private float lineWidth = 50.15f;
    [SerializeField] private Color startPointColor = Color.green;
    [SerializeField] private Color endPointColor = Color.red;
    [SerializeField] private Color cornerPointColor = Color.blue;
    [SerializeField] private Color lineStartColor = Color.green;
    [SerializeField] private Color lineEndColor = Color.red;
    
    // 직교화 설정
    [Header("Orthogonalization Settings")]
    [SerializeField] private bool useOrthogonalPaths = true;
    [SerializeField] private float raycastDistance = 200f;
    [SerializeField] private LayerMask obstacleLayerMask = -1; // 기본값으로 모든 레이어

    void Start()
    {
        // LineRenderer 컴포넌트 추가 및 설정
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 0;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = lineStartColor;
        lineRenderer.endColor = lineEndColor;

        path = new NavMeshPath();        
    }

    void Update()
    {
        // 마우스 왼쪽 버튼 클릭 감지
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // 첫번째 클릭은 시작점 설정
                if (!startPointSet)
                {
                    startPoint = hit.point;
                    startPointSet = true;
                    Debug.Log("시작점 설정: " + startPoint);
                    
                    // 시작점 표시 (녹색 구)
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.transform.position = startPoint;
                    sphere.transform.localScale = new Vector3(sphereScale, sphereScale, sphereScale);
                    sphere.GetComponent<Renderer>().material.color = startPointColor;
                    Destroy(sphere.GetComponent<Collider>());
                }
                // 두번째 클릭은 끝점 설정 및 경로 계산
                else if (!endPointSet)
                {
                    endPoint = hit.point;
                    endPointSet = true;
                    Debug.Log("끝점 설정: " + endPoint);
                    
                    // 끝점 표시 (빨간색 구)
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.transform.position = endPoint;
                    sphere.transform.localScale = new Vector3(sphereScale, sphereScale, sphereScale);
                    sphere.GetComponent<Renderer>().material.color = endPointColor;
                    Destroy(sphere.GetComponent<Collider>());
                    
                    // 경로 계산 및 시각화
                    CalculateAndVisualizePath();
                }
                // 세번째 클릭은 리셋
                else
                {
                    ResetPath();
                }
            }
        }
    }

    void CalculateAndVisualizePath()
    {
        // NavMesh 경로 계산
        NavMesh.CalculatePath(startPoint, endPoint, NavMesh.AllAreas, path);
        
        // 경로 정보 출력
        Debug.Log("경로 계산 상태: " + path.status);
        Debug.Log("경로 포인트 개수: " + path.corners.Length);
        
        if (path.status == NavMeshPathStatus.PathComplete || path.status == NavMeshPathStatus.PathPartial)
        {
            // 기본 경로 설정
            Vector3[] pathPoints = path.corners;
            
            // 직교 경로로 변환하기 (필요한 경우)
            if (useOrthogonalPaths && pathPoints.Length > 1)
            {
                pathPoints = CreateOrthogonalPath(pathPoints);
            }
            
            // LineRenderer로 경로 그리기
            lineRenderer.positionCount = pathPoints.Length;
            lineRenderer.SetPositions(pathPoints);
            
            // 각 코너 포인트 정보 출력 및 표시
            for (int i = 0; i < pathPoints.Length; i++)
            {
                // 시작점과 끝점은 이미 표시했으므로 중간 점들만 표시
                if (i > 0 && i < pathPoints.Length - 1)
                {
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.transform.position = pathPoints[i];
                    sphere.transform.localScale = new Vector3(sphereScale, sphereScale, sphereScale);
                    sphere.GetComponent<Renderer>().material.color = cornerPointColor;
                    Destroy(sphere.GetComponent<Collider>());
                }
            }
        }
        else
        {
            Debug.LogError("경로를 찾을 수 없습니다!");
        }
    }
    
    Vector3[] CreateOrthogonalPath(Vector3[] originalPath)
    {
        if (originalPath.Length <= 2)
            return originalPath;
            
        // 새로운 직교 경로를 저장할 리스트
        List<Vector3> orthoPath = new List<Vector3>();
        
        // 시작점은 그대로 추가
        orthoPath.Add(originalPath[0]);
        
        // 중간 점들 처리
        for (int i = 1; i < originalPath.Length - 1; i++)
        {
            Vector3 prevPoint = orthoPath[orthoPath.Count - 1]; // 이전에 추가된 점
            Vector3 currentPoint = originalPath[i];
            Vector3 nextPoint = originalPath[i + 1];
            
            // 두 개의 직교 점을 생성 (현재 점 주변에 두 개의 코너를 만듦)
            Vector3 orthoPoint1 = new Vector3();
            Vector3 orthoPoint2 = new Vector3();
            
            // 이전 점과 현재 점 사이의 방향을 분석하여 X축 또는 Z축을 따라 이동할지 결정
            float xDiff = math.abs(currentPoint.x - prevPoint.x);
            float zDiff = math.abs(currentPoint.z - prevPoint.z);
            
            // 이전 세그먼트
            if (xDiff > zDiff)
            {
                // X 방향으로 더 멀리 있으므로, X축을 따라 이동하고 Z는 그대로 유지
                orthoPoint1 = new Vector3(currentPoint.x, currentPoint.y, prevPoint.z);
            }
            else
            {
                // Z 방향으로 더 멀리 있으므로, Z축을 따라 이동하고 X는 그대로 유지
                orthoPoint1 = new Vector3(prevPoint.x, currentPoint.y, currentPoint.z);
            }
            
            // 다음 세그먼트 (현재 점과 다음 점 사이)
            xDiff = math.abs(nextPoint.x - currentPoint.x);
            zDiff = math.abs(nextPoint.z - currentPoint.z);
            
            if (xDiff > zDiff)
            {
                // 다음 점으로 가는 방향이 X축에 더 가까움
                orthoPoint2 = new Vector3(currentPoint.x, currentPoint.y, nextPoint.z);
            }
            else
            {
                // 다음 점으로 가는 방향이 Z축에 더 가까움
                orthoPoint2 = new Vector3(nextPoint.x, currentPoint.y, currentPoint.z);
            }
            
            // 장애물 검사: orthoPoint1이 장애물을 통과하는지 확인
            if (IsPathBlocked(prevPoint, orthoPoint1))
            {
                // 장애물이 있는 경우, 다른 축으로 시도
                if (xDiff > zDiff)
                {
                    orthoPoint1 = new Vector3(prevPoint.x, currentPoint.y, currentPoint.z);
                }
                else
                {
                    orthoPoint1 = new Vector3(currentPoint.x, currentPoint.y, prevPoint.z);
                }
                
                // 여전히 장애물이 있는지 확인
                if (IsPathBlocked(prevPoint, orthoPoint1))
                {
                    // 직교화가 불가능한 경우 원래 점 사용
                    orthoPoint1 = currentPoint;
                }
            }
            
            // 장애물 검사: orthoPoint2가 장애물을 통과하는지 확인
            if (IsPathBlocked(orthoPoint1, orthoPoint2))
            {
                // 장애물이 있는 경우, 다른 축으로 시도
                if (xDiff > zDiff)
                {
                    orthoPoint2 = new Vector3(nextPoint.x, currentPoint.y, currentPoint.z);
                }
                else
                {
                    orthoPoint2 = new Vector3(currentPoint.x, currentPoint.y, nextPoint.z);
                }
                
                // 여전히 장애물이 있는지 확인
                if (IsPathBlocked(orthoPoint1, orthoPoint2))
                {
                    // 직교화가 불가능한 경우 원래 점 사용
                    orthoPoint2 = currentPoint;
                }
            }
            
            // 계산된 직교 점들을 경로에 추가
            orthoPath.Add(orthoPoint1);
            
            // 중복 점 방지: 두 점이 너무 가까우면 하나만 추가
            if (Vector3.Distance(orthoPoint1, orthoPoint2) > 0.1f)
            {
                orthoPath.Add(orthoPoint2);
            }
        }
        
        // 끝점 추가
        orthoPath.Add(originalPath[originalPath.Length - 1]);
        
        // 최적화: 세 점이 일직선상에 있으면 중간 점 제거
        return OptimizePath(orthoPath.ToArray());
    }
    
    bool IsPathBlocked(Vector3 start, Vector3 end)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        
        if (distance < 0.1f) 
            return false; // 거리가 너무 가까우면 충돌 없음으로 처리
            
        direction.Normalize();
        
        RaycastHit hit;
        // 레이어 마스크를 사용하여 특정 레이어의 장애물만 고려
        if (Physics.Raycast(start, direction, out hit, distance, obstacleLayerMask))
        {
            // NavMeshModifierVolume 또는 임의의 콜라이더 체크
            if (hit.collider.GetComponent<NavMeshModifierVolume>() != null || 
                !hit.collider.GetComponent<NavMeshSurface>())
            {
                Debug.DrawLine(start, hit.point, Color.red, 3f);
                return true; // 경로가 차단됨
            }
        }
        
        Debug.DrawLine(start, end, Color.green, 3f);
        return false; // 경로가 열려 있음
    }
    
    Vector3[] OptimizePath(Vector3[] path)
    {
        if (path.Length <= 2) 
            return path;
            
        List<Vector3> optimizedPath = new List<Vector3>();
        optimizedPath.Add(path[0]); // 항상 시작점 추가
        
        for (int i = 1; i < path.Length - 1; i++)
        {
            Vector3 prev = path[i - 1];
            Vector3 current = path[i];
            Vector3 next = path[i + 1];
            
            // 세 점이 거의 일직선상에 있는지 확인
            // - 현재 점에서 이전-다음 점을 잇는 직선까지의 거리를 계산
            Vector3 lineDirection = (next - prev).normalized;
            float distance = Vector3.Cross(lineDirection, current - prev).magnitude;
            
            // 거리가 충분히 작은 경우, 중간 점은 생략
            if (distance > 0.5f)
            {
                optimizedPath.Add(current);
            }
        }
        
        optimizedPath.Add(path[path.Length - 1]); // 항상 끝점 추가
        return optimizedPath.ToArray();
    }

    void ResetPath()
    {
        startPointSet = false;
        endPointSet = false;
        lineRenderer.positionCount = 0;
        Debug.Log("경로 리셋");
        
        // 씬에 있는 모든 구체 제거 (경로 표시 제거)
        foreach (GameObject sphere in GameObject.FindGameObjectsWithTag("Untagged"))
        {
            if (sphere.GetComponent<MeshFilter>()?.sharedMesh.name == "Sphere")
            {
                Destroy(sphere);
            }
        }
    }
}