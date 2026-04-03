using UnityEngine;
using UnityEngine.UI;
using static ConstValue;

public class StartSceneManager : MonoBehaviour
{
    [Header("시작 버튼")]
    [SerializeField] private Button startButton;

    void Start()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartGameClicked);
        }
    }

    public void OnStartGameClicked()
    {
        SceneChangeManager.ChangeScene(MainMenuScene);
    }
}
