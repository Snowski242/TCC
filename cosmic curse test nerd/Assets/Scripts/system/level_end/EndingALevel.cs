using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingALevel : MonoBehaviour
{
    public Animator animator;


    private void OnEnable()
    {
        LevelEnd.OnLevelEnd += Animation;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Animation()
    {
        animator.SetTrigger("restart");
        StartCoroutine(LoadingResults());
    }

    IEnumerator LoadingResults()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(1);


    }

}
