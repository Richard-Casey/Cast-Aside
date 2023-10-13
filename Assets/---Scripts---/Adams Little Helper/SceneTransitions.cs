using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class SceneTransitions : MonoBehaviour
{
    [SerializeField] Animator CanvasAnimator;

    public static UnityEvent<AnimationsTypes> PlayTransition = new UnityEvent<AnimationsTypes>();

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
    public int SceneId = 0;
    public void Start()
    {
        GetAnimatorIDS();
        PlayTransition.AddListener(PlayJustTransition);
    }

    public void Update()
    {
        if (test)
        {
            test = false;
            LoadScene(SceneId, type);
        }
    }

    public void PlayJustTransition(AnimationsTypes typeToPlay)
    {
        TriggerAnimation(typeToPlay);
    }

    public void Transition()
    {
        LoadScene(SceneId, type);
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
        yield return new WaitForSeconds(1);
        StartCoroutine(LoadIn());
    }

    IEnumerator LoadIn()
    {
        Destroy(gameObject);
        yield return new WaitForSeconds(0);
    }

    public void TriggerAnimation(AnimationsTypes type)
    {
        CanvasAnimator.SetTrigger(AnimatorIDS[(int)type]);
    }
}
