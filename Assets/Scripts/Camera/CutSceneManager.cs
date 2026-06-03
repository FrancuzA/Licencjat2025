using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CutSceneManager : MonoBehaviour
{
    private Animator _animator;
    public int correctWords = 0;
    public GameObject CutSceneOne;
    public GameObject CutSceneTwo;
    public UnityEvent onCutSceneOff;
    private CameraTilt _camera;

    private void Awake()
    {
        Dependencies.Instance.RegisterDependency<CutSceneManager>(this);
    }
    void Start()
    {
        _camera = Dependencies.Instance.GetDependancy<CameraTilt>();
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
        _camera.UILock = true;
        _animator.SetTrigger("FirstCutscene");
        yield return new WaitForSecondsRealtime(16.3f);
        _camera.UILock=false;
        CutSceneOne.SetActive(false);
        onCutSceneOff.Invoke();
    }

    private IEnumerator StartSecondCutScene()
    {
        CutSceneTwo.SetActive(true);
        _camera.UILock = true;
        _animator.SetTrigger("SecondCutscene");
        yield return new WaitForSecondsRealtime(10);
        _camera.UILock=false;
        SceneManager.LoadSceneAsync(0);
    }

}
