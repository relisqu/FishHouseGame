using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudUI : MonoBehaviour
{
    [Header("Buttons")] 
    [SerializeField] Button RestartButton;
    [SerializeField] Button NextLevelButton;
    [SerializeField] Button MainMenuButton;
    [SerializeField] Button ExitButton;
    [SerializeField] Button ContinueButton;

    [Header("Messanges")]
    [SerializeField] TMP_Text text;
    [SerializeField] Image BackGround;

    AudioListener _audioListener;
    void Start()
    {
        _audioListener = FindObjectOfType<AudioListener>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();

    }

    private void Pause()
    {
        Time.timeScale = 0;
        _audioListener.enabled = false;
        BackGround.gameObject.SetActive(true);
        RestartButton.gameObject.SetActive(true);
        ContinueButton.gameObject.SetActive(true);
        MainMenuButton.gameObject.SetActive(true);
        ExitButton.gameObject.SetActive(true);
    }

    public void Continue()
    {
        Time.timeScale = 1;
        _audioListener.enabled = true;
        RestartButton.gameObject.SetActive(false);
        ContinueButton.gameObject.SetActive(false);
        MainMenuButton.gameObject.SetActive(false);
        ExitButton.gameObject.SetActive(false);
        BackGround.gameObject.SetActive(false);
    }

    public void MainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex % UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings);
    }

    public void Lose()
    {
        Pause();
        ContinueButton.gameObject.SetActive(false);
        NextLevelButton.gameObject.SetActive(false);

    }

    public void Win()
    {
        Pause();
        ContinueButton.gameObject.SetActive(false);
        NextLevelButton.gameObject.SetActive(true);

    }

    public void Exit()
    {
        Application.Quit(0);
    }
}
