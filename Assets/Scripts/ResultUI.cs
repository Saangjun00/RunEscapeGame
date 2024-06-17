using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultUI : MonoBehaviour
{
    public GameObject resultPanel; // 결과 창 패널
    public TextMeshProUGUI resultScoreText; // 결과 점수 텍스트
    public TextMeshProUGUI resultTimeText; // 결과 시간 텍스트
    public PlayerUI ui; // UI 스크립트 참조

    void Start()
    {
        if (resultPanel != null)
        {
            resultPanel.SetActive(false); // 시작 시 결과 창 비활성화
        }

        if (ui == null)
        {
            ui = FindObjectOfType<PlayerUI>(); // UI 스크립트 자동 할당
        }
    }

    public void ShowResult()
    {
        if (resultPanel != null)
        {
            resultPanel.SetActive(true); // 결과 창 활성화
        }

        // 점수와 시간 설정
        resultScoreText.text = " " + ui.playerController.itemCnt.ToString();
        resultTimeText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(ui.gameTime / 60F), Mathf.FloorToInt(ui.gameTime % 60F));
    }

    public void HideResult()
    {
        if (resultPanel != null)
        {
            resultPanel.SetActive(false); // 결과 창 비활성화
        }
    }

    public void RestartGame()
    {
        HideResult(); // 결과 창 비활성화
        Time.timeScale = 1f; // 게임 시간 다시 시작

        // 게임 점수와 시간 초기화
        ui.playerController.itemCnt = 0;
        ui.gameTime = 0f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 현재 씬 다시 로드
    }
}
