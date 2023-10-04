using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTransitions : MonoBehaviour
{
    [SerializeField] Animator CanvasAnimator;


    public enum AnimationsTypes
    {
        CircleSwipe,
        CurtainSwipe,
        CurtainJoin,
        BatCover
    }
    #region AnimatorIDS

    List<int> AnimatorIDS = new List<int>();

    void GetAnimatorIDS()
    {
        AnimatorIDS.Add(Animator.StringToHash("CircleSwipe"));
        AnimatorIDS.Add(Animator.StringToHash("CurtainSwipe"));
        AnimatorIDS.Add(Animator.StringToHash("CurtainJoin"));
        AnimatorIDS.Add(Animator.StringToHash("BatCover"));
    }
    #endregion

    public bool test = false;
    public AnimationsTypes type = AnimationsTypes.BatCover;
    public void Start()
    {
        GetAnimatorIDS();
    }

    public void Update()
    {
        if (test)
        {
            test = false;
            LoadScene(0, type);
        }
    }

    public void LoadScene(int SceneID,AnimationsTypes AnimationType)
    {
        TriggerAnimation(AnimationType);
        StartCoroutine(WaitAndLoad(SceneID, AnimationType));
    }

    IEnumerator WaitAndLoad(int SceneID, AnimationsTypes AnimationType )
    {
        yield return new WaitForSeconds(1);
        DontDestroyOnLoad(this);
        SceneManager.LoadScene(SceneID, LoadSceneMode.Single);
        StartCoroutine(LoadIn());
    }

    IEnumerator LoadIn()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    public void TriggerAnimation(AnimationsTypes type)
    {
        CanvasAnimator.SetTrigger(AnimatorIDS[(int)type]);
    }
}
