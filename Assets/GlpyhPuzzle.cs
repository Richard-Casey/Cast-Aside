using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class GlpyhPuzzle : MonoBehaviour
{

    [SerializeField] GameObject Player;
    [SerializeField] Objective thisObjective;

    [SerializeField] Material ShadowMaterial;
    [SerializeField] Material NonShadowMaterial;

    [SerializeField] float GlyphMoveSpeed = 2f;
    [SerializeField] float HeldGlyphOffset = 6f;

    [SerializeField] GameObject[] CurrentEnteredGlyphCombo = new GameObject[4];

    [SerializeField] List<GameObject> AllGlyphsPrefabs = new List<GameObject>();
    [SerializeField] List<GameObject> SpawnedGlyphs = new List<GameObject>(); // This acts as the combo also
    [SerializeField] GameObject HeldGlpyh;
    [SerializeField] List<Transform> PositionsToSpawnGlyphs = new List<Transform>();

    [SerializeField] List<GlyphTurnIn> TurnIns = new List<GlyphTurnIn>();
    [SerializeField] List<GameObject> TurnInGlows = new List<GameObject>();

    public static UnityEvent<GameObject,Transform> OnGlyphPickup = new UnityEvent<GameObject,Transform>();
    public static UnityEvent<GameObject, Transform> OnGlyphDrop = new UnityEvent<GameObject, Transform>();
    public static UnityEvent<int> OnGlyphPlace = new UnityEvent<int>();

    public bool PlayerInDropOff = false;
    public bool IsPlayerHoldingGlyph = false;
    public bool IsPuzzleReseting = false;
    void Start()
    {
        
        OnGlyphPickup?.AddListener(GlyphPickup);
        OnGlyphDrop?.AddListener(GlyphDrop);
        OnGlyphPlace?.AddListener(GlyphPlace);
        GetRandomCombo();

    }

    // Update is called once per frame
    void Update()
    {
        if (glyphsPlaced >= 4)
        {
            glyphsPlaced = 0;
            CheckCombo();
        }

    }


    void GetRandomCombo()
    {
        CurrentEnteredGlyphCombo = new GameObject[4];
        glyphsPlaced = 0;
        //Copy the list of glyphs so we cant randomly select them
        List<GameObject> TempGameobjectArray = new List<GameObject>();
        GameObject[] temp = new GameObject[AllGlyphsPrefabs.Count];
        AllGlyphsPrefabs.CopyTo(temp);
        TempGameobjectArray = temp.ToList();


        //Spawn The glyphs
        for (int i = 0; i < 4; i++)
        {
            int RandomIndex = Random.Range(0, TempGameobjectArray.Count);
            GameObject GlyphToSpawn = TempGameobjectArray[RandomIndex];

            //Remove the spawned from array
            TempGameobjectArray.RemoveAt(RandomIndex);

            //Spawn The Glyph
            var SpawnedGlyph = Instantiate(GlyphToSpawn, PositionsToSpawnGlyphs[i].position, Quaternion.identity, transform);
            SpawnedGlyph.GetComponent<Glyph>().puzzleManager = this;
            SpawnedGlyphs.Add(SpawnedGlyph);
        }

        List<GameObject> Shuffled = SpawnedGlyphs.OrderBy(i => Guid.NewGuid()).ToList();
        SpawnedGlyphs = Shuffled;
    }

    void GlyphPickup(GameObject glyph, Transform Player)
    {
        if (SpawnedGlyphs.Contains(glyph))
        {
            glyph.GetComponent<MeshRenderer>().material = NonShadowMaterial;
            HeldGlpyh = glyph;
            IsPlayerHoldingGlyph = true;
            glyph.transform.parent = Player;
            glyph.transform.DOLocalMove(Vector3.up * HeldGlyphOffset,1f).OnComplete(() => HeldGlpyh = glyph).SetEase(Ease.InOutQuint);
            foreach (var glow in TurnInGlows)
            {
                glow.SetActive(true);
            }
        }
    }

    void GlyphDrop(GameObject glyph, Transform Player)
    {
        if (HeldGlpyh == glyph)
        {
            glyph.GetComponent<MeshRenderer>().material = ShadowMaterial;
            HeldGlpyh = null;
            IsPlayerHoldingGlyph = false;
            glyph.transform.parent = transform;
            foreach (var glow in TurnInGlows)
            {
                glow.SetActive(false);
            }
        }
    }

    [SerializeField] int glyphsPlaced = 0;
    void GlyphPlace(int ID)
    {
        if (!IsPlayerHoldingGlyph) return;
        CurrentEnteredGlyphCombo[ID-1] = HeldGlpyh;
        HeldGlpyh.transform.parent = TurnIns[ID-1].transform;
        HeldGlpyh.transform.DOLocalMove(TurnIns[ID-1].GlyphHolderPosition.localPosition, 1f).OnComplete(() =>
        {
            glyphsPlaced += 1;
        });

        HeldGlpyh = null;
        IsPlayerHoldingGlyph = false;
        foreach (var glow in TurnInGlows)
        {
            glow.SetActive(false);
        }
    }

    void CheckCombo()
    {
        bool isWrong = false;
        for (int i = 0; i < 4; i++)
        {
            if (!(CurrentEnteredGlyphCombo[i] == SpawnedGlyphs[i]))
            {
                isWrong = true;
            }
        }

        if(isWrong)OnFail();
        else OnSuccess();

    }

    void OnFail()
    {
        IsPuzzleReseting = true;
        glyphsPlaced = 0;
        CurrentEnteredGlyphCombo = new GameObject[4];
        foreach (var glyph in SpawnedGlyphs)
        {
            glyph.transform.parent = transform;
            glyph.GetComponent<MeshRenderer>().material = ShadowMaterial;
            glyph.GetComponent<Glyph>().ResetPosition();
        }
    }

    void OnSuccess()
    {
        ObjectiveManager.ObjectiveComplete?.Invoke(thisObjective);
    }
}
