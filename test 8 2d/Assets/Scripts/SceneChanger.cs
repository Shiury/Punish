using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public Animator transitionAnim;
    public string sceneName;

    void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(ChangeScene());
    }

    IEnumerator ChangeScene()
    {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(sceneName);


    }
}
