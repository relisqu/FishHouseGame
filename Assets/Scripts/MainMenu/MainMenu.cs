using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    int level = 1;
    [SerializeField] List<Button> levels;

    private void Reset()
    {
        PlayerPrefs.SetInt("MaxLevel", 1);

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    void Start()
    {

        if (PlayerPrefs.HasKey("MaxLevel"))
            level = PlayerPrefs.GetInt("MaxLevel");
        else
        {
            
            PlayerPrefs.SetInt("MaxLevel", 1);
        }
        Debug.Log(level);
        for (int i = 0; i < Mathf.Min(level, 4); i++)
        {
            levels[i].interactable = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.T) && Input.GetKeyDown(KeyCode.Return))
            Reset();
    }

    public void LoadScene(int sceneNumber)
    {
        if (sceneNumber == 0)
            UnityEngine.SceneManagement.SceneManager.LoadScene(level);
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNumber);
    }
}
