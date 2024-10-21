using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject FailedMenu;
    public GameObject Overlay;
    public GameObject HealthBar;
    public GameObject EnergyBar;
    public GameObject Objective;
    public GameObject ExitPrompt;

    private Progress Progress;
    private bool Paused;
    private bool UpdateText = true;
    private bool ExitAvailible = false;

    void Awake()
    {
        GameManager.OnStateChanged += OnGameStateChanged;
        GameManager.OnProgress += OnGameProgressChanged;
        Exit.OnExitAvailible += OnExitAvailible;
    }

    private void OnDestroy()
    {
        GameManager.OnStateChanged -= OnGameStateChanged;
        GameManager.OnProgress -= OnGameProgressChanged;
        Exit.OnExitAvailible -= OnExitAvailible;
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
        GameManager.Instance.TotalEnemies = GameObject.FindGameObjectsWithTag("Enemy").Count();
        GameManager.Instance.EnemiesLeft = GameManager.Instance.TotalEnemies;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && !Paused)
        {
            Time.timeScale = 0f;
            Paused = true;
            PauseMenu.SetActive(true);
            Overlay.SetActive(false);
            ExitPrompt.SetActive(false);
        }
        HealthBar.GetComponent<Slider>().value = GameManager.Instance.PlayerHealth;
        EnergyBar.GetComponent<Slider>().value = GameManager.Instance.Energy;
        switch (Progress)
        {
            case Progress.EnemiesRemaining:
                Objective.GetComponent<TextMeshProUGUI>().text = $"Objective: Kill All Enemies ({GameManager.Instance.TotalEnemies - GameManager.Instance.EnemiesLeft}/{GameManager.Instance.TotalEnemies})";
                break;
            case Progress.BossRemaining:
                if (!UpdateText)
                    break;
                Objective.GetComponent<TextMeshProUGUI>().text = "Objective: Kill The Boss";
                UpdateText = false;
                break;
            case Progress.Complete:
                if (!UpdateText)
                    break;
                Objective.GetComponent<TextMeshProUGUI>().text = "Exit The Apartment";
                UpdateText = false;
                break;
        }
    }

    public void Resume()
    {
        Paused = false;
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);
        Overlay.SetActive(true);
        if (ExitAvailible)
            ExitPrompt.SetActive(true);
    }

    public void OnGameStateChanged(GameState state)
    {

        switch (state)
        {
            case GameState.Failed:
                Time.timeScale = 0f;
                Paused = true;
                PauseMenu.SetActive(false);
                FailedMenu.SetActive(true);
                Overlay.SetActive(false);
                ExitPrompt.SetActive(false);
                break;
            default:
                Resume();
                break;

        }
    }

    public void OnGameProgressChanged(Progress progress)
    {
        Progress = progress;
        UpdateText = true;
    }

    public void ChangeVolume(float volume)
    {
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }

    public void OnExitAvailible(bool exitAvailible)
    {
        if (exitAvailible)
        {
            ExitPrompt.SetActive(true);
        }
        else
        {
            ExitPrompt.SetActive(false);
        }
        ExitAvailible = exitAvailible;
    }
}