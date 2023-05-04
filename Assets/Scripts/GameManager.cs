using System.Collections;
using UnityEngine;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Character[] characters;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject loosePanel;
    [SerializeField] private float panelOpenDelay = 1;

    [SerializeField] private int routCounter = 0;
    [SerializeField] private int charcterReachedTargetCounter = 0;

    private int coinAmmountReward = 10;

    public static Action winLevel;
    public static Action looseLevel;


    private void Start()
    {
        levelText.text = $"Level:{SceneLoader.Instance.Level + 1}";
        foreach (var character in characters)
        {
            character.routeDrawed += OnDrawRoute;
            character.achieveGoal += OnReachedTarget;
            character.collideObstacle += OnCollisionObstacle;
        }
    }

    private void OnDrawRoute()
    {
        routCounter++;
        if (routCounter == characters.Length)
        {
            foreach (var character in characters)
            {
                character.Move();
            }
        }
    }
    private void OnReachedTarget()
    {
        charcterReachedTargetCounter++;
        if (charcterReachedTargetCounter == characters.Length)
        {
            Win();
        }
    }
    private void OnCollisionObstacle()
    {
        foreach (var character in characters)
        {
            character.StopMoving();
        }
        Loose();
    }
    private void Win()
    {
        winLevel?.Invoke();
        Bank.Instance.AddCoin(coinAmmountReward);
        StartCoroutine(SetActivePanelWithDelay(winPanel));
        Debug.Log("win");
    }
    private IEnumerator SetActivePanelWithDelay(GameObject panel)
    {
        yield return new WaitForSeconds(panelOpenDelay);
        panel.SetActive(true);
    }
    private void Loose()
    {
        looseLevel?.Invoke();
        StartCoroutine(SetActivePanelWithDelay(loosePanel));
        Debug.Log("Loose");
    }
    public void Reset()
    {
        foreach (var character in characters)
        {
            character.Reset();
        }
        loosePanel.SetActive(false);
        winPanel.SetActive(false);
        routCounter = 0;
        charcterReachedTargetCounter = 0;
    }
}
