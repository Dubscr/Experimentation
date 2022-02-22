using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;
    public GameObject mainMenu;
    public GameObject options;
    // Start is called before the first frame update
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("Volume");
        mixer.SetFloat("Volume", PlayerPrefs.GetFloat("Volume"));
    }

    // Update is called once per frame
    void Update()
    {
            if (Input.GetButtonDown("Cancel") && !options.activeSelf && SceneManager.GetActiveScene().buildIndex == 1)
            {
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
                Cursor.visible = true;
                options.SetActive(true);
                return;
            } 
            if (Input.GetButtonDown("Cancel") && options.activeSelf && SceneManager.GetActiveScene().buildIndex == 1)
            {
                Resume();
                return;
            }
    }



    public void Play()
    {
        SceneManager.LoadScene(1);
        Resume();
    }

    public void Options()
    {
        mainMenu.SetActive(false);
        options.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void SetVolume(float Volume)
    {
        mixer.SetFloat("Volume", Volume);
        PlayerPrefs.SetFloat("Volume", Volume);
    }

    public void Fullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void Resume()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else
        {
            mainMenu.SetActive(true);
        }
        options.SetActive(false);

    }
}
