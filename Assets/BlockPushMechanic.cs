using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
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

    enum BlockType
    {
        none,
        Blocker,
        red,
        green,
        yellow,
        blue,
        Count
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
    [SerializeField] GameObject PushableBlockPrefab;
    [SerializeField] Material DefaultPushableMaterial;
    [SerializeField] Material Red;
    [SerializeField] Material Yellow;
    [SerializeField] Material Blue;
    [SerializeField] Material Green;

    #endregion


    #region Structs

    class BlockData
    {
        public int ID;
        public BlockType type;
        public Vector2 CellIndex;
        public GameObject gObject;

        public BlockData(int _id,BlockType _type, Vector2 _CellIndex)
        {
            ID = _id;
            type = _type;
            CellIndex = _CellIndex;
        }
    }

    #endregion

    #region Lists

    List<GameObject> Pillars = new List<GameObject>();
    Dictionary<Vector2Int, BlockData> PushableBlocks = new Dictionary<Vector2Int, BlockData>();
    
    [SerializeField] List<bool> CubesInShadow = new List<bool>();
    Bounds[,] CellBounds;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        CalculateCellCenters();
        CreatePillars();
        CreatePushableBlocks();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DirectionCheck();
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
                /*Gizmos.color = Color.red;
                Gizmos.DrawSphere(CellBound.center, .3f);*/
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


    #region DeprecaiatedCode

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

    /*void CalculateCellCenters()
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
        }#1#
    }*/

    #endregion

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


    }

    void CreatePillars()
    {
        float HalfY = ((GridDimensions.y * CellSize) / 2f) + .2f ;
        float HalfX = ((GridDimensions.x * CellSize) / 2f) + .2f;


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



    [ItemCanBeNull]
    List<BlockType> BlockStartPosTypes = new List<BlockType>()
    {
        BlockType.yellow,BlockType.Blocker,BlockType.none,BlockType.none,BlockType.none,
        BlockType.Blocker,BlockType.Blocker,BlockType.none,BlockType.Blocker,BlockType.none,
        BlockType.none,BlockType.green,BlockType.Blocker,BlockType.Blocker,BlockType.Blocker,
        BlockType.none,BlockType.blue,BlockType.none,BlockType.none,BlockType.none,
        BlockType.none,BlockType.none,BlockType.Blocker,BlockType.Blocker,BlockType.red
    };
    BlockData[,] BlockStartingPositions;

    void CreatePushableBlocks()
    {
        BlockStartingPositions = new BlockData[GridDimensions.x, GridDimensions.y];
        for (int y = 0; y < GridDimensions.y; y++)
        {
            for (int x = 0; x < GridDimensions.x; x++)
            {

                
                Vector3 PosToSpawn = CellBounds[y, x].center + Vector3.up;
                GameObject ItemToSpawn;
                BlockStartingPositions[y, x] = new BlockData((y * GridDimensions.x) + x,
                    BlockStartPosTypes[(y * GridDimensions.x) + x], new Vector2(x, y));
                switch (BlockStartingPositions[y, x].type)
                {
                    case BlockType.none:
                        break;
                    case BlockType.Blocker:
                        ItemToSpawn = Instantiate(PushableBlockPrefab, PosToSpawn, transform.rotation, transform);
                        ItemToSpawn.GetComponent<MeshRenderer>().material = DefaultPushableMaterial;

                        BlockStartingPositions[y, x].gObject = ItemToSpawn;
                        break;
                    case BlockType.red:
                        ItemToSpawn = Instantiate(PushableBlockPrefab, PosToSpawn, transform.rotation, transform);
                        ItemToSpawn.GetComponent<MeshRenderer>().material = Red;

                        BlockStartingPositions[y, x].gObject = ItemToSpawn;
                        break;
                    case BlockType.green:
                        ItemToSpawn = Instantiate(PushableBlockPrefab, PosToSpawn, transform.rotation, transform);
                        ItemToSpawn.GetComponent<MeshRenderer>().material = Green;

                        BlockStartingPositions[y, x].gObject = ItemToSpawn;
                        break;
                    case BlockType.yellow:
                        ItemToSpawn = Instantiate(PushableBlockPrefab, PosToSpawn, transform.rotation, transform);
                        ItemToSpawn.GetComponent<MeshRenderer>().material = Yellow;
                        
                        BlockStartingPositions[y, x].gObject = ItemToSpawn;
                        break;
                    case BlockType.blue:
                        ItemToSpawn = Instantiate(PushableBlockPrefab, PosToSpawn, transform.rotation, transform);
                        ItemToSpawn.GetComponent<MeshRenderer>().material = Blue;

                        BlockStartingPositions[y, x].gObject = ItemToSpawn;
                        break;
                    case BlockType.Count:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                PushableBlocks.Add(new Vector2Int(x, y), BlockStartingPositions[y, x]);
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        InputManager.Interaction?.AddListener(OnPlayerInteract);
    }

    void OnTriggerLeave()
    {
        InputManager.Interaction?.RemoveListener(OnPlayerInteract);
    }


    void OnPlayerInteract(GameObject Interactor)
    {
        Vector3 TransformPoint = transform.position;
        TransformPoint.x -= (CellSize * GridDimensions.x) / 2f;
        TransformPoint.z -= (CellSize * GridDimensions.x) / 2f;

        //Find Cell Player Is In
        Vector3 PlayerPositionLocal = Interactor.transform.position - TransformPoint;

        int x = (int)(Mathf.Floor(PlayerPositionLocal.x / CellSize));
        int z = (int)(Mathf.Floor(PlayerPositionLocal.z / CellSize));
        Vector2Int CellLocationOfPlayer = new Vector2Int(x, z);


        if (CubesInShadow[CellLocationOfPlayer.x + (GridDimensions.y * CellLocationOfPlayer.y)] == true)
        {
            //Get the direction of the player from the cell so we know which direction the player is trying to move the block
            Vector3 CenterOfCell = PushableBlocks[CellLocationOfPlayer].gObject.transform.position;
            Vector3 DirectionToPlayer = (Interactor.transform.position - CenterOfCell).normalized;
            DirectionToPlayer.y = 0;
            Vector3 FlattenedDirection = MathUtil.GetAxis(DirectionToPlayer);

            //Inverse the direction of the cell
            FlattenedDirection = -FlattenedDirection;

            //Check if the next cube over is free
            Vector2Int IndexOfNextCell = new Vector2Int((CellLocationOfPlayer.x + (int)FlattenedDirection.x), (CellLocationOfPlayer.y + (int)FlattenedDirection.z));

            //Check if next cell is out of range
            if (IndexOfNextCell.x < 0 || IndexOfNextCell.y < 0 || IndexOfNextCell.x >= GridDimensions.x ||
                IndexOfNextCell.y >= GridDimensions.y) return;

            if (PushableBlocks[IndexOfNextCell].type == BlockType.none)
            {
                PushableBlocks[CellLocationOfPlayer].gObject.transform.DOMove(
                    PushableBlocks[CellLocationOfPlayer].gObject.transform.position + (FlattenedDirection * CellSize), 1f, false);
                PushableBlocks[IndexOfNextCell].type = PushableBlocks[CellLocationOfPlayer].type;
                PushableBlocks[IndexOfNextCell].gObject = PushableBlocks[CellLocationOfPlayer].gObject;

                PushableBlocks[CellLocationOfPlayer].type = BlockType.none;
                PushableBlocks[CellLocationOfPlayer].gObject = null;
            }
        }
        else
        {
            Debug.Log("NotShadow");
        }


    }

    //Draw debug grid in inspector
    void OnDrawGizmos()
    {
        float HalfY = ((GridDimensions.y * CellSize) / 2f) - CellSize / 2f;
        float HalfX = ((GridDimensions.x * CellSize) / 2f) - CellSize / 2f;

        if (CellBounds == null) CalculateCellCenters();

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
