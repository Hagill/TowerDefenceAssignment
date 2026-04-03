using UnityEngine;
using UnityEngine.UI;
using static ConstValue;

public class MainMenuSceneManager : MonoBehaviour
{
    [Header("버튼")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;


    void Start()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnGameClicked);
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitClicked);
        }
    }

    public void OnGameClicked()
    {
        SceneChangeManager.ChangeScene(GameScene);
    }

    public void OnExitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
