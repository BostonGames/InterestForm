using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneButton : MonoBehaviour
{
    public void LoadFormScene()
    {
        SceneManager.LoadScene(0);
    }
    public void LoadExampleSceneDesert()
    {
        SceneManager.LoadScene(1);
    }
}
