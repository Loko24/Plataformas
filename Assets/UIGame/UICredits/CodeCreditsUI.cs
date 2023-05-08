using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CodeCreditsUI : MonoBehaviour
{
    public string sceneName;
    void Start()
    {
        sceneName = "MainMenu";
        StartCoroutine(ReturnToScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ReturnToScene()
    {
        yield return new WaitForSeconds(5);

        SceneManager.LoadScene(sceneName);
    }
}
