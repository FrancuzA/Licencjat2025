using System.Collections;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    private Animator _animator;
    public int correctWords = 0;
    public GameObject CutSceneOne;
    public GameObject CutSceneTwo;

    private void Awake()
    {
        Dependencies.Instance.RegisterDependency<CutSceneManager>(this);
    }
    void Start()
    {
        _animator = GetComponent<Animator>();
        StartCoroutine(StartFirstCutScene());
    }


    public void AddCorrectWord()
    {
        correctWords++;
    }

    public void StartEndCutScene()
    {
        StartCoroutine(StartSecondCutScene());
    }

    private IEnumerator StartFirstCutScene()
    {
        CutSceneOne.SetActive(true);
        _animator.SetTrigger("FirstCutscene");
        yield return new WaitForSecondsRealtime(10);
        CutSceneOne.SetActive(false);
    }

    private IEnumerator StartSecondCutScene()
    {
        CutSceneTwo.SetActive(true);
        _animator.SetTrigger("SecondCutscene");
        yield return new WaitForSecondsRealtime(5);
        CutSceneTwo.SetActive(false);
    }

}
