using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text levelText;

    private void Start()
    {
        levelText.text = $"Level:{(int)SaveLoadSystem.LoadData("Level", 0) + 1} / {SceneLoader.Instance.MaxLevel + 1}";
    }
}
