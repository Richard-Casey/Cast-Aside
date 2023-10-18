using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class BlockPushMechanic : MonoBehaviour
{
    #region GridValues
    [Header("Grid Values")]
    [SerializeField] Vector2Int GridDimensions = Vector2Int.one * 5;
    [SerializeField] float CellSize = 3f;
    [SerializeField] float BlockSize = 2f;
    [SerializeField] [Range(0, 1)] float ShadowAngleThreshold = .8f;
    [SerializeField] [Range(0, 5)] float ShadowLengthThresholdInCells = 2;
    #endregion

    #region Enums

    enum CellType
    {
        
    }

    #endregion

    #region References

    //Old Attempt
    /*[SerializeField] Camera ShadowCamera;
    [SerializeField] RenderTexture ShadowTestTexture;*/
    [SerializeField] Transform Ground;
    [SerializeField] Transform Sun;
    Texture2D OutputTexture;
    #endregion


    #region Prefabs

    [SerializeField] GameObject PillarPrefab;


    #endregion


    #region Structs


    #endregion

    #region Lists

    List<GameObject> Pillars = new List<GameObject>();
    [SerializeField] List<bool> CubesInShadow = new List<bool>();
    Bounds[,] CellBounds;
    /*Vector2Int[,] CellCentersOnScreen;*/
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        CalculateCellCenters();
        CreatePillars();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DirectionCheck()
    {
        //Compare suns rotation with that of the pillar
        CubesInShadow.Clear();
        Vector3 SunDirectionOnPlane = Vector3.ProjectOnPlane(Sun.forward,Vector3.up);
        
        foreach (var CellBound in CellBounds)
        {
            bool inShadow = false;
            
            foreach (var pillar in Pillars)
            {
                //Get pillars position as a vector 2

                //GameObject pillar = Pillars[6];
                Vector3 ClosestPointOnBounds = CellBound.ClosestPoint(pillar.transform.position);
                //ClosestPointOnBounds = Vector3.ProjectOnPlane(ClosestPointOnBounds,Vector3.up);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(CellBound.center, .3f);
                //Distance Of Bounds to pillar and project it on a flat plane
                Vector3 DistanceFromPillarToBounds = (ClosestPointOnBounds - pillar.transform.position);
                Vector3 DirectionFromPillarToBounds = Vector3.ProjectOnPlane(DistanceFromPillarToBounds.normalized,Vector3.up);

                float Dot = Vector3.Dot(DirectionFromPillarToBounds, SunDirectionOnPlane);
                if (Dot > ShadowAngleThreshold &&
                    DistanceFromPillarToBounds.magnitude <= ShadowLengthThresholdInCells * CellSize)
                {
                    inShadow = true;
                }
            }

            CubesInShadow.Add(inShadow);

        }
    }

    void CalculateCellCenters()
    {
        CellBounds = new Bounds[GridDimensions.x, GridDimensions.y];
        float HalfY = ((GridDimensions.y * CellSize) / 2f) - CellSize / 2f;
        float HalfX = ((GridDimensions.x * CellSize) / 2f) - CellSize / 2f;
        int xIndex = 0;
        int yIndex = 0;
        for (float y = -HalfY; y <= HalfY; y += CellSize)
        {
            for (float x = -HalfX; x <= HalfX; x += CellSize)
            {
                CellBounds[yIndex, xIndex] =
                    new Bounds(new Vector3(transform.position.x + x, transform.position.y, transform.position.z + y),
                        new Vector3(CellSize, .1f, CellSize));
                xIndex++;
            }

            xIndex = 0;
            yIndex++;
        }


        /*CellCentersOnScreen = new Vector2Int[GridDimensions.x, GridDimensions.y];
        for (int y = 0; y < GridDimensions.y; y++)
        {
            for (int x = 0; x < GridDimensions.x; x++)
            {
                Vector2 CellCenter = CellCenters[y, x];
                Vector3 point = ShadowCamera.WorldToScreenPoint(transform.position + new Vector3(CellCenter.x, 0, CellCenter.y));
                CellCentersOnScreen[y, x] = new Vector2Int(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y));
            }
        }*/
    }

    void CreatePillars()
    {
        float HalfY = ((GridDimensions.y * CellSize) / 2f) + .2f ;
        float HalfX = ((GridDimensions.x * CellSize) / 2f) + .2f;

        //Ground.localScale = new Vector3(HalfX * 2f, .1f, HalfY * 2f);

        //Create Corner Pillars
        Pillars.Add(Instantiate(PillarPrefab,transform.position + new Vector3(HalfX,0,HalfY),transform.rotation,transform));
        Pillars.Add(Instantiate(PillarPrefab, transform.position + new Vector3(HalfX, 0, -HalfY), transform.rotation, transform));
        Pillars.Add(Instantiate(PillarPrefab, transform.position + new Vector3(-HalfX, 0, HalfY), transform.rotation, transform));
        Pillars.Add(Instantiate(PillarPrefab, transform.position + new Vector3(-HalfX, 0, -HalfY), transform.rotation, transform));

        float HalfCellSize = (CellSize / 2f) +.2f;

        //U
        Pillars.Add(Instantiate(PillarPrefab, transform.position + new Vector3(HalfCellSize, 0, HalfY), transform.rotation, transform));
        Pillars.Add(Instantiate(PillarPrefab, transform.position + new Vector3(-HalfCellSize, 0, HalfY), transform.rotation, transform));

        //D
        Pillars.Add(Instantiate(PillarPrefab, transform.position + new Vector3(HalfCellSize, 0, -HalfY), transform.rotation, transform));
        Pillars.Add(Instantiate(PillarPrefab, transform.position + new Vector3(-HalfCellSize, 0, -HalfY), transform.rotation, transform));

        //L
        Pillars.Add(Instantiate(PillarPrefab, transform.position + new Vector3(HalfX, 0, -HalfCellSize), transform.rotation, transform));
        Pillars.Add(Instantiate(PillarPrefab, transform.position + new Vector3(-HalfX, 0, -HalfCellSize), transform.rotation, transform));

        //R
        Pillars.Add(Instantiate(PillarPrefab, transform.position + new Vector3(HalfX, 0, HalfCellSize), transform.rotation, transform));
        Pillars.Add(Instantiate(PillarPrefab, transform.position + new Vector3(-HalfX, 0, HalfCellSize), transform.rotation, transform));

        int index = 0;

        //Assign Them To The Correct Layer Bc ya know prefabs dont carry layers??
        foreach (var pillar in Pillars)
        {
            pillar.layer = LayerMask.NameToLayer("ShadowCaster2");
            pillar.name = index.ToString();
            index++;
        }
    }

    void FixedUpdate()
    {

    }

    //Old Inefficent attempt at capturing real shadows, to under performant due to the limitation of gpu readback
    /*void SampleBlockCenters()
    {
        CubesInShadow = new List<bool>();

        if (!OutputTexture) OutputTexture = new Texture2D(ShadowTestTexture.width, ShadowTestTexture.height, TextureFormat.RGBA32, false);
        RenderTexture.active = ShadowTestTexture;
        Rect sourceRect = new Rect(0, 0, ShadowTestTexture.width, ShadowTestTexture.height);
        OutputTexture.ReadPixels(sourceRect, 0, 0);
        RenderTexture.active = null;

        var PixelData = OutputTexture.GetPixels();
        foreach (var Pixel in CellCentersOnScreen)
        {
            int Index = (Pixel.y * ShadowCamera.pixelWidth) + Pixel.x;
            if (PixelData[Index].r < .5f)
            {
                CubesInShadow.Add(true);
            }
            else
            {
                CubesInShadow.Add(false);
            }
        }
    }*/


    //Draw debug grid in inspector
    void OnDrawGizmos()
    {
        float HalfY = ((GridDimensions.y * CellSize) / 2f) - CellSize / 2f;
        float HalfX = ((GridDimensions.x * CellSize) / 2f) - CellSize / 2f;

        if (CellBounds == null) CalculateCellCenters();
        if(Pillars.Count > 0) DirectionCheck();

        for (int y = 0; y < GridDimensions.y; y++)
        {
            for (int x = 0; x < GridDimensions.x; x++)
            {
                Bounds cellBounds = CellBounds[y, x];
                Gizmos.color = Color.white;
                if (CubesInShadow.Count > 0)
                {
                    int index = (y * GridDimensions.x) + x;
                    Color colorToDraw = CubesInShadow[index] ? Color.cyan : Color.white;
                    colorToDraw.a = .5f;
                    Gizmos.color = colorToDraw;
                }
                Gizmos.DrawCube(cellBounds.center - new Vector3(0,1,0), cellBounds.size);

            }
        }

    }
}
