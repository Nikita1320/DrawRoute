using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Animator animatorLoadPanel;
    private int level;
    private static SceneLoader instance;
    public int Level => level;
    public int MaxLevel => SceneManager.sceneCountInBuildSettings - 1;
    public static SceneLoader Instance => instance;

    private void Awake()
    {
        if (instance == null)
        {
            level = (int)SaveLoadSystem.LoadData("Level", 0);
            GameManager.winLevel += () =>
            {
                level++;
                SaveLoadSystem.SaveData("Level", level);
            };
        }
        instance = this;
    }
    public void LoadLevel()
    {
        if (SceneManager.sceneCountInBuildSettings - 1 == level)
        {
            LoadMainScene();
        }
        else
        {
            animatorLoadPanel.SetTrigger("Open");
            SceneManager.LoadSceneAsync(level + 1);
        }
    }
    public void LoadMainScene()
    {
        animatorLoadPanel.SetTrigger("Open");
        SceneManager.LoadSceneAsync(0);
    }
}
