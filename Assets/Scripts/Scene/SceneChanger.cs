using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    [SerializeField] private Image progress;
    [SerializeField] private Text progressTxt;
    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadingScene(sceneIndex));
    }
    IEnumerator LoadingScene(int sceneIndex)
    { 
        AsyncOperation loading= SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
        while (!loading.isDone)
        {
            progress.fillAmount = loading.progress;
            progressTxt.text = ((int)(loading.progress * 100)).ToString()+"%";
            yield return null;
        }
    }
}
