using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class HudUI : MonoBehaviour
{
    [Header("Buttons")] 
    [SerializeField] Button RestartButton;
    [SerializeField] Button NextLevelButton;
    [SerializeField] Button MainMenuButton;
    [SerializeField] Button ExitButton;
    [SerializeField] Button ContinueButton;
    [SerializeField] Button ConfirmButton;

    [Header("Messanges")]
    [SerializeField] TMP_Text text;
    [SerializeField] TMP_Text timeText;
    [SerializeField] Slider timeSlider;
    [SerializeField] Image BackGround;
    [SerializeField] GameObject LoseMessange;
    [SerializeField] GameObject WinMessange;
    [SerializeField] GameObject PauseMessange;
    [SerializeField] GameObject ConfirmMessange;

    AudioListener _audioListener;
    void Start()
    {
        Time.timeScale = 1;
        _audioListener = FindObjectOfType<AudioListener>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 1)
            Pause();
        else if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0)
            Continue();

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
        PauseMessange.SetActive(true);
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
        PauseMessange.SetActive(false);
        ConfirmMessange.SetActive(false);
    }

    public void MainMenu()
    {
        Confirm(delegate {
            Time.timeScale = 1;
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        });
    }

    public void Restart()
    {
        Confirm(delegate {
            Time.timeScale = 1;
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex); 
        });
        
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex % UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings);
    }

    public void Lose()
    {
        Pause();
        PauseMessange.SetActive(false);
        LoseMessange.SetActive(true);
        ContinueButton.gameObject.SetActive(false);
        NextLevelButton.gameObject.SetActive(false);

    }

    public void Win()
    {
        Pause();
        PauseMessange.SetActive(false);
        WinMessange.SetActive(true);
        ContinueButton.gameObject.SetActive(false);
        NextLevelButton.gameObject.SetActive(true);

    }

    public void Exit()
    {
        Confirm(delegate { Application.Quit(0); });
        
    }

    void Confirm(UnityEngine.Events.UnityAction action)
    {
        ConfirmMessange.SetActive(true);
        ConfirmButton.onClick.RemoveAllListeners();
        ConfirmButton.onClick.AddListener(action);
    }

    public void SetTime(float time)
    {
        timeText.gameObject.SetActive(true);
        timeText.text = $"{(int)(time / 60):00}:{(int)(time % 60):00}";
    }

    public void SetTimeSlider(float value)
    {
        timeSlider.gameObject.SetActive(true);
        timeSlider.value = value;
    }
}
