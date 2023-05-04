using UnityEngine;
using System;

public class Bank : MonoBehaviour
{
    [SerializeField] private int ammountCoin;
    [SerializeField] private Sprite coinSprite;

    private static Bank instance;
    public Action changedAmmount;
    public int AmmountCoin => ammountCoin;
    public Sprite CoinSprite => coinSprite;
    public static Bank Instance => instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        ammountCoin = (int)SaveLoadSystem.LoadData("AmmountCoin", 0);
        DontDestroyOnLoad(gameObject);
    }

    public void AddCoin(int ammount)
    {
        ammountCoin += ammount;
        changedAmmount?.Invoke();
    }
    public bool Spend(int ammount)
    {
        if (ammount <= ammountCoin)
        {
            ammountCoin -= ammount;
            changedAmmount?.Invoke();
            return true;
        }
        return false;
    }
}
