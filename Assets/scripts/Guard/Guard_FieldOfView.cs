using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard_FieldOfView : MonoBehaviour {

    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    public float meshResolution;
    public int edgeResolveIterations;

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    LineRenderer lineRender;
    public Material lineMat;
    public Material enemyMat;
    [Range(0,100)]
    public float viewLineWidth;

    private Transform hips;

    void Start()
    {
        lineRender = GetComponent<LineRenderer>();
        lineRender.material = lineMat;
        lineRender.widthMultiplier = viewLineWidth / 100;

        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        hips = transform;

        StartCoroutine("FindTargetsWithDelay", 0.2f);
    }

    void Update()
    {
        DrawFieldOfView();
        
    }

    void LateUpdate()
    {
        //DrawFieldOfView();
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
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
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
        ViewCastInfo oldViewCast = new ViewCastInfo();
        int m = 6; //DELETE
        for(int i =0;i <= stepCount; i++)
        {
            if(i == 1)
            {
                m++;
            }
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);
       
            if(i > 0)
            {
                if(oldViewCast.hit != newViewCast.hit)
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if(edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if(edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }


            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }
        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        Vector3 pointA, pointB;
        //pointA = Quaternion.AngleAxis(90, Vector3.up) * viewPoints[0];
        pointA = transform.position;
        //lineRender.positionCount = vertexCount;
        //lineRender.SetPosition(0, pointA);
        int j = 6;
        for (int i = 0; i < vertexCount-1; i++)
        {
            ////if(i == 0)
            ////{
            ////    j++;
            ////}
            ////pointB = viewPoints[i];
            //pointB = transform.InverseTransformPoint(viewPoints[i]);
            //pointB = Quaternion.Euler(transform.eulerAngles) * transform.InverseTransformPoint(viewPoints[i]);
            //pointB = Quaternion.AngleAxis(90, Vector3.up) * transform.InverseTransformPoint(viewPoints[i]);
            //pointB += transform.position;

            ////lineRender.SetPosition(i+1, pointB);
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
            }
        }
        ////lineRender.SetPosition(lineRender.positionCount-1, hips.position);
        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();

        //Call Detect Enemy
        //DetectEnemy();
    }

    void DetectEnemy()
    {
        GameObject tmp = new GameObject("lineObj");
        LineRenderer render = tmp.AddComponent<LineRenderer>();
        render.material = enemyMat;
        render.widthMultiplier = viewLineWidth / 100;
        lineRender.positionCount = visibleTargets.Capacity+1;
        lineRender.SetPosition(0, transform.transform.position);

        for (int i = 1 ; i < visibleTargets.Capacity ; i++)
        {
            lineRender.SetPosition(i, visibleTargets[i].position);
        }
        Destroy(tmp);
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
        if(Physics.Raycast(transform.position, direction, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + direction * viewRadius, viewRadius, globalAngle);
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
