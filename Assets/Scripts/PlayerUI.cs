using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public PlayerController playerController; // PlayerController 스크립트 참조
    public TextMeshProUGUI scoreText; // TextMeshPro 점수 텍스트 UI
    public TextMeshProUGUI timeText; // TextMeshPro 시간 텍스트 UI

    public float gameTime = 0f; // 게임 시간 변수

    void Start()
    {
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>(); // PlayerController 스크립트 자동 할당
        }

        if (scoreText == null || timeText == null)
        {
            Debug.LogError("ScoreText 또는 TimeText가 할당되지 않았습니다.");
        }
    }

    void Update()
    {
        UpdateScore(); // 점수 업데이트
        UpdateGameTime(); // 게임 시간 업데이트
    }

    void UpdateScore()
    {
        scoreText.text = " " + playerController.itemCnt.ToString(); // 점수 텍스트 업데이트
    }

    void UpdateGameTime()
    {
        gameTime += Time.deltaTime; // 시간 누적
        int minutes = Mathf.FloorToInt(gameTime / 60F); // 분 계산
        int seconds = Mathf.FloorToInt(gameTime % 60F); // 초 계산
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // 시간 텍스트 업데이트
    }
}