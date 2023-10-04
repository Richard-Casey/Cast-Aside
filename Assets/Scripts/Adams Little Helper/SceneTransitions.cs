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
        CurtainJoin
    }
    #region AnimatorIDS

    List<int> AnimatorIDS = new List<int>();

    void GetAnimatorIDS()
    {
        AnimatorIDS.Add(Animator.StringToHash("CircleSwipe"));
        AnimatorIDS.Add(Animator.StringToHash("CurtainSwipe"));
        AnimatorIDS.Add(Animator.StringToHash("CurtainJoin"));
    }
    #endregion

    public bool test = false;

    public void Start()
    {
        GetAnimatorIDS();
    }

    public void Update()
    {
        if (test)
        {
            test = false;
            LoadScene(0, AnimationsTypes.CurtainJoin);
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
