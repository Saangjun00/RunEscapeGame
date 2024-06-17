using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public ResultUI resultUI; // ResultUI 스크립트 참조

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (resultUI == null)
        {
            resultUI = FindObjectOfType<ResultUI>(); // ResultUI 스크립트 자동 할당
        }
    }

    public void GameClear()
    {
        resultUI.ShowResult(); // 결과 창 표시
        Time.timeScale = 0f; // 게임 시간 정지
        StartCoroutine(RestartGameAfterDelay(3f)); // 5초 후에 게임 재시작
    }

    private IEnumerator RestartGameAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // 실시간으로 5초 대기
        Time.timeScale = 1f; // 게임 시간 다시 시작
        resultUI.RestartGame(); // 게임 재시작
    }
}
