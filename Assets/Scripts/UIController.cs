using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController main;

    [SerializeField] private Text _score;
    [SerializeField] private Text _health;

    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _resumeButton;

    private void Awake()
    {
        main = this;
    }

    public void UpdateText()
    {
        _score.text = $"Score: {GameplayController.main.score}";
        _health.text = $"Health: {GameplayController.main.health}";
    }

    public void OpenMenu()
    {
        Time.timeScale = 0;

        _resumeButton.SetActive(GameplayController.main.gameplayIsStart);
        _menu.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1;

        _menu.SetActive(false);
    }

    public void NewGame()
    {
        Time.timeScale = 1;

        _menu.SetActive(false);
        GameplayController.main.NewGame();
    }

    public void ChangeControl()
    {
        InputSystem.main.ChangeControlType();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
