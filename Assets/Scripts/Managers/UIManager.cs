using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject PauseMenu;
    private bool Paused;
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
        if (Input.GetKey(KeyCode.Escape) || Paused) 
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

    public void ChangeVolume(float volume)
    {
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }
}
