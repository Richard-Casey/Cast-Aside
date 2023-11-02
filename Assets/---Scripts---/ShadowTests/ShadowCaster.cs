using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class ShadowCaster : MonoBehaviour
{
    [System.Serializable]
    public class ShadowVolume
    {
        public Vector3 Close1;
        public Vector3 Close2;
        public Vector3 Close3;
        public Vector3 Close4;
        public Vector3 Far1;
        public Vector3 Far2;
        public Vector3 Far3;
        public Vector3 Far4;
        public Vector3 Center;
    }

    public BoxCollider[] Bounds;
    public List<ShadowVolume> Volumes = new List<ShadowVolume>();
    List<Vector3> ShadowPoints = new List<Vector3>();
    List<MeshFilter> ShadowObjects = new List<MeshFilter>();
    public Transform LightSource;
    public Material ShadowMaterial;
 
    void Start()
    {
        Bounds = GetComponents<BoxCollider>();
        foreach (var Collider in Bounds)
        {
            Volumes.Add(ExtractVertsFromCollider(Collider));
            var gObject = new GameObject();
            gObject.transform.position = Collider.center;
            
            ShadowObjects.Add(gObject.AddComponent<MeshFilter>());
            MeshRenderer renderer = gObject.AddComponent<MeshRenderer>();
            renderer.material = ShadowMaterial;
            renderer.shadowCastingMode = ShadowCastingMode.Off;


        }
    }

    private ShadowVolume ExtractVertsFromCollider(BoxCollider collider)
    {
        //Extract Coords and convert to global space
        ShadowVolume Output = new ShadowVolume();
        
        Vector3 ColliderSize = collider.size / 2f;

        //Close
        //Top
        Output.Close1 = (transform.TransformPoint(collider.center + new Vector3(ColliderSize.x, ColliderSize.y, -ColliderSize.z)));
        Output.Close2 = (transform.TransformPoint(collider.center + new Vector3(-ColliderSize.x, ColliderSize.y, -ColliderSize.z)));
        //Bottom
        Output.Close3 = (transform.TransformPoint(collider.center + new Vector3(ColliderSize.x, -ColliderSize.y, -ColliderSize.z)));
        Output.Close4 = (transform.TransformPoint(collider.center + new Vector3(-ColliderSize.x, -ColliderSize.y, -ColliderSize.z)));


        //Far
        //Top
        Output.Far1 = (transform.TransformPoint(collider.center + new Vector3(ColliderSize.x, ColliderSize.y, ColliderSize.z)));
        Output.Far2 = (transform.TransformPoint(collider.center + new Vector3(-ColliderSize.x, ColliderSize.y, ColliderSize.z)));
        //Bottom
        Output.Far3 = (transform.TransformPoint(collider.center + new Vector3(-ColliderSize.x, -ColliderSize.y, ColliderSize.z)));
        Output.Far4 = (transform.TransformPoint(collider.center + new Vector3(ColliderSize.x, -ColliderSize.y, ColliderSize.z)));

        Output.Center = transform.TransformPoint(collider.center);

        return Output;
    }

    
    private List<Vector3> FindWidestPoints(ShadowVolume volume)
    {
        List<Vector3> outputPoints = new List<Vector3>();
        List<Vector3> TopPoints = new List<Vector3>() { volume.Close1, volume.Close2, volume.Far1, volume.Far2 };
        List<Vector3> BottomPoints = new List<Vector3>() { volume.Close3, volume.Close4, volume.Far3, volume.Far4 };

        Vector3 LightsourceOnCenterPlane = new Vector3(LightSource.position.x, volume.Center.y, LightSource.position.z);

        Vector3 xzDirection =
            (volume.Center - LightsourceOnCenterPlane).normalized;

        float TopSmallest = Mathf.Infinity;
        float TopGreatest = Mathf.NegativeInfinity;
        float BottomSmallest = Mathf.Infinity;
        float BottomGreatest = Mathf.NegativeInfinity;
        Vector3 TopGreatestPoint = Vector3.zero;
        Vector3 TopSmallestPoint = Vector3.zero;
        Vector3 BottomGreatestPoint = Vector3.zero;
        Vector3 BottomSmallestPoint = Vector3.zero;

        foreach (var point in TopPoints)
        {
            Vector3 PointOnPlane = MathUtil.PointToPlane(point, LightSource.position, xzDirection);
            Vector3 Diffrence = LightsourceOnCenterPlane - PointOnPlane;
            if (Diffrence.x < TopSmallest)
            {
                TopSmallest = Diffrence.x;
                TopSmallestPoint = point;
            }
            else if (Diffrence.x > TopGreatest)
            {
                TopGreatest = Diffrence.x;
                TopGreatestPoint = point;

            }
        }

        foreach (var point in BottomPoints)
        {
            Vector3 PointOnPlane = MathUtil.PointToPlane(point, LightSource.position, xzDirection);
            Vector3 Diffrence = LightsourceOnCenterPlane - PointOnPlane;
            if (Diffrence.x < BottomSmallest)
            {
                BottomSmallest = Diffrence.x;
                BottomSmallestPoint = point;
            }
            else if (Diffrence.x > BottomGreatest)
            {
                BottomGreatest = Diffrence.x;
                BottomGreatestPoint = point;

            }
        }

        outputPoints.Add(TopGreatestPoint);
        outputPoints.Add(TopSmallestPoint);
        outputPoints.Add(BottomGreatestPoint);
        outputPoints.Add(BottomSmallestPoint);

        return outputPoints;
    }

    private Vector3? CalculateIntersectionWithPlane(Vector3 PlaneNormal, Vector3 PlaneCenter , Vector3 RayStart , Vector3 RayDirection)
    {

        Vector3 p_0 = PlaneCenter;

        Vector3 n = -PlaneNormal;

        Vector3 l_0 = RayStart;

        float denominator = Vector3.Dot(RayDirection, n);

        if (denominator > 0.00001f)
        {
            //The distance to the plane
            float t = Vector3.Dot(p_0 - l_0, n) / denominator;

            //Point Of Intersection
            Vector3 p = l_0 + RayDirection * t;
            return p;

        }
        else
        {
            Debug.Log("No intersection");
            return null;
        }

    }

    List<int> indicies = new List<int>()
    {
        1,2,0,2,3,0,
        4,7,5,7,6,5,
        0,3,4,3,7,4,
        5,6,1,6,2,1,
        2,6,3,6,7,3,
        5,1,4,1,0,4

    };

    void CreateMesh()
    {
        for(int i = 0 ; i < Volumes.Count ; i++)
        {
            Mesh mesh = new Mesh();

            //Find Four ClosestPoints
            var Closest = FindWidestPoints(Volumes[i]);

            List<Vector3> FarPoints = new List<Vector3>();
            //Cast a ray from our light through each of these points
            foreach (var closestPoint in Closest)
            {
                Vector3 Direction = (closestPoint - LightSource.position).normalized;
                Vector3? IntersectedPoint =
                    CalculateIntersectionWithPlane(Vector3.up, Vector3.zero, closestPoint, Direction);
                if (!IntersectedPoint.HasValue) return;
                FarPoints.Add(IntersectedPoint.Value);
            }

            List<Vector3> Verticies = new List<Vector3>()
            {
                Closest[3],Closest[2],Closest[0],Closest[1],
                FarPoints[3],FarPoints[2],FarPoints[0],FarPoints[1]
            };

            mesh.vertices = Verticies.ToArray();
            mesh.triangles = indicies.ToArray();

            ShadowObjects[i].mesh = mesh;


            Debug.Log("test");

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CreateMesh();
    }

}
