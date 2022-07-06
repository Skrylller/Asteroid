using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public static GameplayController main;

    [SerializeField] private int _maxHealth;
    [SerializeField] private int _firsNumAsteroid;

    public int score { get; private set; }
    public int health { get; private set; }
    public bool gameplayIsStart { get; private set; }

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        gameplayIsStart = false;
        UIController.main.OpenMenu();
        UIController.main.UpdateText();
    }

    public void NewGame()
    {
        score = 0;
        health = _maxHealth;
        UIController.main.UpdateText();
        gameplayIsStart = true;
        ObjectsPull.main.DeactiveAll();
        PlayerController.main.ResetPlayer();
        Generator.main.StartGenerate(_firsNumAsteroid);
    }

    public void PlusScore(int value)
    {
        score += value;
    }

    public bool MinusHealth()
    {
        health--;
        UIController.main.UpdateText();

        if (health <= 0)
        {
            gameplayIsStart = false;
            UIController.main.OpenMenu();
            ObjectsPull.main.DeactiveAll();
            return false;
        }
        return true;
    }
}
