using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class BlockPushMechanic : MonoBehaviour
{
    BlockData[,] BlockStartingPositions;


    [ItemCanBeNull]
    List<BlockType> BlockStartPosTypes = new List<BlockType>
    {
        BlockType.yellow, BlockType.Blocker, BlockType.none, BlockType.none, BlockType.none,
        BlockType.Blocker, BlockType.Blocker, BlockType.none, BlockType.Blocker, BlockType.none,
        BlockType.none, BlockType.green, BlockType.Blocker, BlockType.Blocker, BlockType.Blocker,
        BlockType.none, BlockType.blue, BlockType.none, BlockType.none, BlockType.none,
        BlockType.none, BlockType.none, BlockType.Blocker, BlockType.Blocker, BlockType.red
    };

    // Start is called before the first frame update
    void Start()
    {
        CopyMaterials();
        CalculateCellCenters();
        CreateFloor();
        CreatePillars();
        CreateTargets();
        CreatePushableBlocks();
        GetComponent<BoxCollider>().size = new Vector3(CellSize * GridDimensions.x + 2f, 30f, CellSize * GridDimensions.y + 2f);
    }

    //we copy materials so we can change variables without worrying about it chaning permenantly
    void CopyMaterials()
    {
        for (int i = 0; i < TargetMaterials.Count; i++)
        {
            TargetMaterials[i] = new Material(TargetMaterials[i]);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PuzzleActive) DirectionCheck();
    }

    void DirectionCheck()
    {
        //Compare suns rotation with that of the pillar
        CubesInShadow.Clear();
        Vector3 SunDirectionOnPlane = Vector3.ProjectOnPlane(Sun.forward, Vector3.up);
        if (CellBounds == null) CalculateCellCenters();
        foreach (Bounds CellBound in CellBounds)
        {
            bool inShadow = false;

            foreach (GameObject pillar in Pillars)
            {
                //Get pillars position as a vector 2

                //GameObject pillar = Pillars[6];
                Vector3 ClosestPointOnBounds = CellBound.ClosestPoint(pillar.transform.position);
                //ClosestPointOnBounds = Vector3.ProjectOnPlane(ClosestPointOnBounds,Vector3.up);
                /*Gizmos.color = Color.red;
                Gizmos.DrawSphere(CellBound.center, .3f);*/
                //Distance Of Bounds to pillar and project it on a flat plane
                Vector3 DistanceFromPillarToBounds = ClosestPointOnBounds - pillar.transform.position;
                Vector3 DirectionFromPillarToBounds = Vector3.ProjectOnPlane(DistanceFromPillarToBounds.normalized, Vector3.up);

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


    void Awake()
    {
        CalculateCellCenters();
    }

    void CalculateCellCenters()
    {
        CellBounds = new Bounds[GridDimensions.x, GridDimensions.y];
        float HalfY = GridDimensions.y * CellSize / 2f - CellSize / 2f;
        float HalfX = GridDimensions.x * CellSize / 2f - CellSize / 2f;
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
        float HalfY = GridDimensions.y * CellSize / 2f + .2f;
        float HalfX = GridDimensions.x * CellSize / 2f + .2f;


        //Create Corner Pillars
        Pillars.Add(Instantiate(PillarPrefab, transform.position + new Vector3(HalfX, 0, HalfY), transform.rotation, transform));
        Pillars.Add(Instantiate(PillarPrefab, transform.position + new Vector3(HalfX, 0, -HalfY), transform.rotation, transform));
        Pillars.Add(Instantiate(PillarPrefab, transform.position + new Vector3(-HalfX, 0, HalfY), transform.rotation, transform));
        Pillars.Add(Instantiate(PillarPrefab, transform.position + new Vector3(-HalfX, 0, -HalfY), transform.rotation, transform));

        float HalfCellSize = CellSize / 2f + .2f;

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
        foreach (GameObject pillar in Pillars)
        {
            pillar.layer = LayerMask.NameToLayer("ShadowCasters");
            pillar.transform.localScale = pillar.transform.localScale * CellSize;
            pillar.name = index.ToString();
            index++;
        }
    }

    void CreateFloor()
    {
        FloorParts = new (GameObject, Material)[GridDimensions.x, GridDimensions.y];
        Material DefaultFloorMaterial = FloorPartPrefab.transform.GetComponentInChildren<MeshRenderer>().sharedMaterial;
        for (int y = 0; y < GridDimensions.y; y++)
        {
            for (int x = 0; x < GridDimensions.x; x++)
            {
                FloorPartPrefab.transform.localScale = new Vector3(CellSize * .2f, 1, CellSize * .2f);
                GameObject Floor = Instantiate(FloorPartPrefab, CellBounds[y, x].center, transform.rotation, transform);

                Material FloorMaterial = new Material(DefaultFloorMaterial);
                Floor.GetComponentInChildren<MeshRenderer>().material = FloorMaterial;
                FloorParts[y, x] = (Floor, FloorMaterial);
            }
        }
    }

    void ChangeFloorColor()
    {
        for (int y = 0; y < GridDimensions.y; y++)
        {
            for (int x = 0; x < GridDimensions.x; x++)
            {
                Color TargetColor = Color.white;
                if (CubesInShadow[x + GridDimensions.y * y])
                {
                    TargetColor *= .5f;
                }

                if (FloorParts[y, x].Item2.color != TargetColor)
                {
                    FloorParts[y, x].Item2.DOColor(TargetColor, .7f);
                }
            }
        }
    }

    IEnumerator UpdateFloorColor()
    {
        while (PuzzleActive)
        {
            yield return new WaitForSeconds(.75f);
            ChangeFloorColor();

        }
    }

    void CreateTargets()
    {
        Vector3 TransformPoint = transform.position;
        TransformPoint.x -= CellSize * GridDimensions.x / 2f;
        TransformPoint.z -= CellSize * GridDimensions.x / 2f;

        //Find Cell Target Is In

        Vector3 TargetPositionLocal;
        Vector2Int CellLocationOfTarget;
        int x;
        int z;

        //scale all target prefabs
        foreach (GameObject prefab in TargetPrefabs)
        {
            prefab.transform.localScale = new Vector3(CellSize * .1f, 1, CellSize * .1f);
        }


        //n
        Vector3 RedSpawnPosition =
            new Vector3(transform.position.x, transform.position.y, transform.position.z - CellSize * GridDimensions.y / 2f - CellSize / 2f);
        GameObject RedSpawn = Instantiate(TargetPrefabs[(int)colors.red], RedSpawnPosition, transform.rotation, transform);
        RedSpawn.GetComponent<MeshRenderer>().material = TargetMaterials[(int)colors.red];
        TargetPositionLocal = RedSpawnPosition - TransformPoint;
        x = (int)Mathf.Floor(TargetPositionLocal.x / CellSize);
        z = (int)Mathf.Floor(TargetPositionLocal.z / CellSize);
        CellLocationOfTarget = new Vector2Int(x, z);
        Targets.Add(colors.red, CellLocationOfTarget);
        //e
        Vector3 BlueSpawnPosition =
            new Vector3(transform.position.x - CellSize * GridDimensions.y / 2f - CellSize / 2f, transform.position.y, transform.position.z);
        GameObject BlueSpawn = Instantiate(TargetPrefabs[(int)colors.blue], BlueSpawnPosition, transform.rotation, transform);
        BlueSpawn.GetComponent<MeshRenderer>().material = TargetMaterials[(int)colors.blue];
        TargetPositionLocal = BlueSpawnPosition - TransformPoint;
        x = (int)Mathf.Floor(TargetPositionLocal.x / CellSize);
        z = (int)Mathf.Floor(TargetPositionLocal.z / CellSize);
        CellLocationOfTarget = new Vector2Int(x, z);
        Targets.Add(colors.blue, CellLocationOfTarget);
        //s
        Vector3 GreenSpawnPosition =
            new Vector3(transform.position.x, transform.position.y, transform.position.z + CellSize * GridDimensions.y / 2f + CellSize / 2f);
        GameObject GreenSpawn = Instantiate(TargetPrefabs[(int)colors.green], GreenSpawnPosition, transform.rotation, transform);
        GreenSpawn.GetComponent<MeshRenderer>().material = TargetMaterials[(int)colors.green];
        TargetPositionLocal = GreenSpawnPosition - TransformPoint;
        x = (int)Mathf.Floor(TargetPositionLocal.x / CellSize);
        z = (int)Mathf.Floor(TargetPositionLocal.z / CellSize);
        CellLocationOfTarget = new Vector2Int(x, z);
        Targets.Add(colors.green, CellLocationOfTarget);
        //w
        Vector3 YellowSpawnPosition =
            new Vector3(transform.position.x + CellSize * GridDimensions.y / 2f + CellSize / 2f, transform.position.y, transform.position.z);
        GameObject YellowSpawn = Instantiate(TargetPrefabs[(int)colors.yellow], YellowSpawnPosition, transform.rotation, transform);
        YellowSpawn.GetComponent<MeshRenderer>().material = TargetMaterials[(int)colors.yellow];
        TargetPositionLocal = YellowSpawnPosition - TransformPoint;
        x = (int)Mathf.Floor(TargetPositionLocal.x / CellSize);
        z = (int)Mathf.Floor(TargetPositionLocal.z / CellSize);
        CellLocationOfTarget = new Vector2Int(x, z);
        Targets.Add(colors.yellow, CellLocationOfTarget);
    }

    void CreatePushableBlocks()
    {
        BlockStartingPositions = new BlockData[GridDimensions.x, GridDimensions.y];
        for (int y = 0; y < GridDimensions.y; y++)
        {
            for (int x = 0; x < GridDimensions.x; x++)
            {
                Vector3 PosToSpawn = CellBounds[y, x].center;
                GameObject ItemToSpawn;
                BlockStartingPositions[y, x] = new BlockData(y * GridDimensions.x + x,
                    BlockStartPosTypes[y * GridDimensions.x + x], new Vector2(x, y));
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
                        ItemToSpawn.GetComponent<MeshRenderer>().material = new Material(TargetMaterials[(int)colors.red]);

                        BlockStartingPositions[y, x].gObject = ItemToSpawn;
                        BlockStartingPositions[y, x].color = colors.red;
                        break;
                    case BlockType.green:
                        ItemToSpawn = Instantiate(PushableBlockPrefab, PosToSpawn, transform.rotation, transform);
                        ItemToSpawn.GetComponent<MeshRenderer>().material = new Material(TargetMaterials[(int)colors.green]);

                        BlockStartingPositions[y, x].gObject = ItemToSpawn;
                        BlockStartingPositions[y, x].color = colors.green;
                        break;
                    case BlockType.yellow:
                        ItemToSpawn = Instantiate(PushableBlockPrefab, PosToSpawn, transform.rotation, transform);
                        ItemToSpawn.GetComponent<MeshRenderer>().material = new Material(TargetMaterials[(int)colors.yellow]);

                        BlockStartingPositions[y, x].gObject = ItemToSpawn;
                        BlockStartingPositions[y, x].color = colors.yellow;
                        break;
                    case BlockType.blue:
                        ItemToSpawn = Instantiate(PushableBlockPrefab, PosToSpawn, transform.rotation, transform);
                        ItemToSpawn.GetComponent<MeshRenderer>().material = new Material(TargetMaterials[(int)colors.blue]);

                        BlockStartingPositions[y, x].gObject = ItemToSpawn;
                        BlockStartingPositions[y, x].color = colors.blue;
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
        if (collision.TryGetComponent<GameSpecificCharacterController>(out GameSpecificCharacterController controller))
        {
            controller.GetCameraManager().SetCameraRotation(90,180,0);
            controller.GetcCharacterController().SetUseUpAxis();
        }
        PuzzleActive = true;
        StartCoroutine(UpdateFloorColor());
    }

    void OnTriggerExit(Collider collision)
    {
        InputManager.Interaction?.RemoveListener(OnPlayerInteract);
        PuzzleActive = false;
        if (collision.TryGetComponent<GameSpecificCharacterController>(out GameSpecificCharacterController controller))
        {
            controller.GetCameraManager().ResetTransform();
            controller.GetcCharacterController().ResetCustomAxis();
        }
        StopCoroutine(UpdateFloorColor());
    }


    void OnPlayerInteract(GameObject Interactor)
    {
        Vector3 TransformPoint = transform.position;
        TransformPoint.x -= CellSize * GridDimensions.x / 2f;
        TransformPoint.z -= CellSize * GridDimensions.x / 2f;


        //Find Cell Player Is In
        Vector3 PlayerPositionLocal = Interactor.transform.position - TransformPoint;

        int x = (int)Mathf.Floor(PlayerPositionLocal.x / CellSize);
        int z = (int)Mathf.Floor(PlayerPositionLocal.z / CellSize);
        Vector2Int CellLocationOfPlayer = new Vector2Int(x, z);
        if (CellLocationOfPlayer.x < 0 || CellLocationOfPlayer.x >= GridDimensions.x || CellLocationOfPlayer.y < 0 || CellLocationOfPlayer.y >= GridDimensions.y) return;

        if (CubesInShadow[CellLocationOfPlayer.x + GridDimensions.y * CellLocationOfPlayer.y])
        {
            if (PushableBlocks[CellLocationOfPlayer].gObject == null) return;
            //Get the direction of the player from the cell so we know which direction the player is trying to move the block
            Vector3 CenterOfCell = PushableBlocks[CellLocationOfPlayer].gObject.transform.position;
            Vector3 DirectionToPlayer = (Interactor.transform.position - CenterOfCell).normalized;
            DirectionToPlayer.y = 0;
            Vector3 FlattenedDirection = MathUtil.GetAxis(DirectionToPlayer);

            //Inverse the direction of the cell
            FlattenedDirection = -FlattenedDirection;

            //Check if the next cube over is free
            Vector2Int IndexOfNextCell = new Vector2Int(CellLocationOfPlayer.x + (int)FlattenedDirection.x, CellLocationOfPlayer.y + (int)FlattenedDirection.z);


            //Check if next cell is out of range
            if ((IndexOfNextCell.x < 0 || IndexOfNextCell.y < 0 || IndexOfNextCell.x >= GridDimensions.x ||
                IndexOfNextCell.y >= GridDimensions.y) && PushableBlocks[CellLocationOfPlayer].type == BlockType.Blocker) return;

            if (PushableBlocks[CellLocationOfPlayer].color != colors.none &&
                Targets[PushableBlocks[CellLocationOfPlayer].color] == IndexOfNextCell)
            {
                //This is when a target block moves into the correct target holder
                PushableBlocks[CellLocationOfPlayer].gObject.transform.DOMove(
                    PushableBlocks[CellLocationOfPlayer].gObject.transform.position + FlattenedDirection * CellSize, 1f).OnComplete(() => TargetMaterials[(int)PushableBlocks[CellLocationOfPlayer].color].SetColor("_EmissionColor", TargetMaterials[(int)PushableBlocks[CellLocationOfPlayer].color].color * 2f));



                PushableBlocks[CellLocationOfPlayer].type = BlockType.none;
                PushableBlocks[CellLocationOfPlayer].gObject = null;

                NumberOfBlocksInTargets++;



                //If all blocks are in trigger win
                if (NumberOfBlocksInTargets >= 4)
                {
                    OnCompletion?.Invoke();
                    Camera.main.transform.DOShakePosition(1.5f, Vector3.one * .2f);
                }
            }
            else if (!(IndexOfNextCell.x < 0 || IndexOfNextCell.y < 0 || IndexOfNextCell.x >= GridDimensions.x ||
                     IndexOfNextCell.y >= GridDimensions.y))
            {
                if (PushableBlocks[IndexOfNextCell].type == BlockType.none)
                {
                    PushableBlocks[CellLocationOfPlayer].gObject.transform.DOMove(
                        PushableBlocks[CellLocationOfPlayer].gObject.transform.position +
                        FlattenedDirection * CellSize, 1f);
                    PushableBlocks[IndexOfNextCell].color = PushableBlocks[CellLocationOfPlayer].color;
                    PushableBlocks[IndexOfNextCell].type = PushableBlocks[CellLocationOfPlayer].type;
                    PushableBlocks[IndexOfNextCell].gObject = PushableBlocks[CellLocationOfPlayer].gObject;

                    PushableBlocks[CellLocationOfPlayer].type = BlockType.none;
                    PushableBlocks[CellLocationOfPlayer].gObject = null;
                    PushableBlocks[CellLocationOfPlayer].color = colors.none;
                }
            }
        }
        else
        {
            if (PushableBlocks[CellLocationOfPlayer].gObject == null) return;
            PushableBlocks[CellLocationOfPlayer].gObject.transform.DOShakePosition(1f, ShakeStrength).SetEase(Ease.InOutCirc);
        }
    }

    #region Structs

    class BlockData
    {
        public Vector2 CellIndex;
        public colors color;
        public GameObject gObject;
        public int ID;
        public BlockType type;

        public BlockData(int _id, BlockType _type, Vector2 _CellIndex)
        {
            ID = _id;
            type = _type;
            CellIndex = _CellIndex;
            color = colors.none;
        }
    }

    #endregion

    #region GridValues

    [Header("Grid Values")]
    [SerializeField] Vector2Int GridDimensions = Vector2Int.one * 5;
    [SerializeField] float CellSize = 3f;
    [SerializeField] float BlockSize = 2f;
    [SerializeField] [Range(0, 1)] float ShadowAngleThreshold = .8f;
    [SerializeField] [Range(0, 5)] float ShadowLengthThresholdInCells = 2;
    [SerializeField] Vector3 ShakeStrength = new Vector3(.1f, .1f, .1f);
    [SerializeField] UnityEvent OnCompletion = new UnityEvent();
    int NumberOfBlocksInTargets;
    bool PuzzleActive;

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

    enum colors
    {
        red,
        blue,
        green,
        yellow,
        none
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
    [SerializeField] GameObject FloorPartPrefab;
    [SerializeField] Material DefaultPushableMaterial;
    [SerializeField] List<Material> TargetMaterials = new List<Material>();
    [SerializeField] List<GameObject> TargetPrefabs = new List<GameObject>();

    #endregion

    #region Lists

    [SerializeField] List<GameObject> Pillars = new List<GameObject>();
    [SerializeField] Dictionary<Vector2Int, BlockData> PushableBlocks = new Dictionary<Vector2Int, BlockData>();
    [SerializeField] Dictionary<colors, Vector2> Targets = new Dictionary<colors, Vector2>();

    [SerializeField] List<bool> CubesInShadow = new List<bool>();
    [SerializeField]static Bounds[,] CellBounds;
    [SerializeField] (GameObject, Material)[,] FloorParts;

    #endregion


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

    /*//Draw debug grid in inspector
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

    }*/
}
