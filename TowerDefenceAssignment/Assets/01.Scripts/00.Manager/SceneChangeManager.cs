using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static ConstValue;

public class SceneChangeManager : MonoBehaviour
{
    [Header("로딩 UI")]
    [SerializeField] private Image progressBar;
    [SerializeField] private TextMeshProUGUI progressText;

    [Header("최소 로딩 대기 시간")]
    [SerializeField] private float minWaitingTime;

    public static string nextSceneName;

    private void Start()
    {
        StartCoroutine(Loading());
    }

    private IEnumerator Loading()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(nextSceneName);
        async.allowSceneActivation = false;

        float timer = 0f;

        while (timer < minWaitingTime)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / minWaitingTime);

            // 로딩 진행도 시각화 텍스트, 바
            progressText.text = string.Format("{0:F1} %", progress * 100f);
            progressBar.fillAmount = progress;

            yield return null;
        }

        progressBar.fillAmount = 1f;

        async.allowSceneActivation = true;
    }

    public static void ChangeScene(string sceneName)
    {
        nextSceneName = sceneName;
        SceneManager.LoadScene(LoadingScene);
    }
}
