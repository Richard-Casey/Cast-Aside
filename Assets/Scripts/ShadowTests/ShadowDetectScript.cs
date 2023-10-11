using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ShadowDetectScript : MonoBehaviour
{

    public int AmountNeeded = 2;
    public LayerMask ShadowCasters;
    int Hitsthisfram = 0;
    public Transform LightSource;
    enum BodyParts
    {
        head, chest, leftarm,rightarm,pelvis,leftfppt,rightfoot
    }
    public List<Transform> Limbs = new List<Transform>();

    // Update is called once per frame
    void Update()
    {
        Hitsthisfram = 0;
        foreach (var Limb in Limbs)
        {
            //Calculate The Direction To The Light Source
            Vector3 Direction = (LightSource.position - Limb.position);
            RaycastHit data;
            Debug.DrawRay(Limb.position,Direction,Color.red,.2f);
            if (!Physics.Raycast(Limb.position, Direction, out data, Mathf.Infinity, ShadowCasters))
            {
                Hitsthisfram++;
            }
        }

        if (Hitsthisfram >= AmountNeeded)
        {
            Debug.Log("InSun");
        }
    }
}
