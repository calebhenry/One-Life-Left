using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject FailedMenu;
    private bool Paused;

    void Awake()
    {
        GameManager.OnStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnStateChanged -= OnGameStateChanged;
    }

    // Start is called before the first frame update
    void Start()
    {
        Paused = false;
        if (!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", 1);
            PlayerPrefs.Save();
        }
        PauseMenu.GetComponentInChildren<Slider>().value = PlayerPrefs.GetFloat("Volume");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && !Paused) 
        {
            Time.timeScale = 0f;
            Paused = true;
            PauseMenu.SetActive(true);
        }
    }

    public void Resume()
    {
        Paused = false;
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);
    }

    public void OnGameStateChanged(GameState state)
    {

        switch(state)
        {
            case GameState.Failed:
                Time.timeScale = 0f;
                Paused = true;
                PauseMenu.SetActive(false);
                FailedMenu.SetActive(true);
                break;
            default:
                Resume();
                break;

        }
    }

    public void ChangeVolume(float volume)
    {
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }
}
