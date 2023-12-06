using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    int level = 1;
    void Start()
    {
        if (PlayerPrefs.HasKey("MaxLevel"))
            level = PlayerPrefs.GetInt("MaxLevel");
        else
        {
            PlayerPrefs.SetInt("MaxLevel", 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(int sceneNumber)
    {
        if (sceneNumber == 0)
            UnityEngine.SceneManagement.SceneManager.LoadScene(level);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNumber);
    }
}
