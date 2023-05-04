using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourcesPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text ammountCoinText;
    [SerializeField] private Image coinImage;
    [SerializeField] private ParticleSystem particle;
    private void Start()
    {
        coinImage.sprite = Bank.Instance.CoinSprite;
        ammountCoinText.text = Bank.Instance.AmmountCoin.ToString();
        Bank.Instance.changedAmmount += RenderAmmountCoin;
    }
    private void RenderAmmountCoin()
    {
        ammountCoinText.text = Bank.Instance.AmmountCoin.ToString();
        particle.Play();
    }
    private void OnDestroy()
    {
        Bank.Instance.changedAmmount -= RenderAmmountCoin;
    }
}
