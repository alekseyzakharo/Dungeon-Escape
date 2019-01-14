using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard_FieldOfView : MonoBehaviour {

    public float detectRadius;
    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    public float meshResolution;
    public int edgeResolveIterations;

    public MeshFilter detectMeshFilter;
    public MeshFilter viewMeshFilter;
    Mesh detectMesh;
    Mesh viewMesh;

    public Material enemyMat;

    void Start()
    {
        detectMesh = new Mesh();
        viewMesh = new Mesh();

        detectMesh.name = "Detect Mesh";
        viewMesh.name = "View Mesh";

        detectMeshFilter.mesh = detectMesh;
        viewMeshFilter.mesh = viewMesh;

        StartCoroutine("FindTargetsWithDelay", 0.2f);
    }

    void Update()
    {
        DrawFieldOfView();
        
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTarget();
        }
    }

    void FindVisibleTarget()
    {
        visibleTargets.Clear();
        //find all targets around
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, detectRadius, targetMask);
        for(int i = 0;i<targetsInViewRadius.Length;i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            //check if angle between yourself and the target is less than  your view angle
            //if(Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                //raycast to target, to check if any obstacles are in the way
                if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                {
                    //trigger chase target............
                    visibleTargets.Add(target);
                }
            }
        }
    }

    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;

        List<Vector3> viewPoints = new List<Vector3>();
        List<Vector3> detectPoints = new List<Vector3>();

        ViewCastInfo oldViewCast = new ViewCastInfo();
        for(int i =0;i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);
       
            if(i > 0)
            {
                if(oldViewCast.hit != newViewCast.hit)
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if(edge.pointA != Vector3.zero)
                    {
                        AddPoint(ref viewPoints, ref detectPoints, edge.pointA);
                        //viewPoints.Add(edge.pointA);
                    }
                    if(edge.pointB != Vector3.zero)
                    {
                        AddPoint(ref viewPoints, ref detectPoints, edge.pointB);
                        //viewPoints.Add(edge.pointB);
                    }
                }
            }
            AddPoint(ref viewPoints, ref detectPoints, newViewCast.point);
            //viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int viewVertexCount = viewPoints.Count + 1;
        int detectVertextCount = detectPoints.Count + viewPoints.Count;

        Vector3[] viewVertices = new Vector3[viewVertexCount];
        Vector3[] detectVertices = new Vector3[detectVertextCount];

        int[] viewTriangles = new int[(viewVertexCount - 2) * 3];
        int[] detectTriangles = new int[(detectVertextCount - 2) * 3];

        viewVertices[0] = Vector3.zero;
        int iterator = 0;
        for (int i = 0; i < viewVertexCount - 1; i++)
        {
            viewVertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            if (i < viewVertexCount - 2)
            {
                viewTriangles[i * 3] = 0;
                viewTriangles[i * 3 + 1] = i + 1;
                viewTriangles[i * 3 + 2] = i + 2;
            }

            detectVertices[iterator++] = transform.InverseTransformPoint(viewPoints[i]);
            detectVertices[iterator++] = transform.InverseTransformPoint(detectPoints[i]);

        }
        viewMesh.Clear();
        viewMesh.vertices = viewVertices;
        viewMesh.triangles = viewTriangles;
        viewMesh.RecalculateNormals();

        for(int i = 0;i < detectVertextCount - 1;i+=2)
        {

            if(i < detectVertextCount - 2)
            {
                detectTriangles[i * 3] = i;
                detectTriangles[i * 3 + 1] = i + 1;
                detectTriangles[i * 3 + 2] = i + 3;

                detectTriangles[i * 3 + 3] = i;
                detectTriangles[i * 3 + 4] = i + 3;
                detectTriangles[i * 3 + 5] = i + 2;
            }
        }

        detectMesh.Clear();
        detectMesh.vertices = detectVertices;
        detectMesh.triangles = detectTriangles;
        detectMesh.RecalculateNormals();

    }

    void AddPoint(ref List<Vector3> viewPoints, ref List<Vector3> detectPoints, Vector3 point)
    {
        float distance = Vector3.Distance(transform.position, point);
        if (distance > viewRadius)
        {
            float time = viewRadius / distance;
            Vector3 dir = point - transform.position;
            viewPoints.Add(transform.position + (dir * time));
            detectPoints.Add(point);
        }
        else
        {
            viewPoints.Add(point);
            detectPoints.Add(point);
        }
    }
  
    EdgeInfo FindEdge(ViewCastInfo minViewcast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewcast.angle;
        float maxAngle = maxViewCast.angle;

        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for(int i = 0; i < edgeResolveIterations;i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            if(newViewCast.hit == minViewcast.hit)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }
        return new EdgeInfo(minPoint, maxPoint);
    }
     
    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 direction = DirFromAngle(globalAngle, true);
        RaycastHit hit;
        if(Physics.Raycast(transform.position, direction, out hit, detectRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + direction * detectRadius, detectRadius, globalAngle);
        }
    }


    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if(!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _distance, float _angle)
        {
            hit = _hit;
            point = _point;
            distance = _distance;
            angle = _angle;  
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }
}
