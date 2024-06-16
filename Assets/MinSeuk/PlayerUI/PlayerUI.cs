using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public PlayerController playerController; // PlayerController ��ũ��Ʈ ����
    public TextMeshProUGUI scoreText; // TextMeshPro ���� �ؽ�Ʈ UI
    public TextMeshProUGUI timeText; // TextMeshPro �ð� �ؽ�Ʈ UI

    public float gameTime = 0f; // ���� �ð� ����

    void Start()
    {
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>(); // PlayerController ��ũ��Ʈ �ڵ� �Ҵ�
        }

        if (scoreText == null || timeText == null)
        {
            Debug.LogError("ScoreText �Ǵ� TimeText�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    void Update()
    {
        UpdateScore(); // ���� ������Ʈ
        UpdateGameTime(); // ���� �ð� ������Ʈ
    }

    void UpdateScore()
    {
        scoreText.text = " " + playerController.itemCnt.ToString(); // ���� �ؽ�Ʈ ������Ʈ
    }

    void UpdateGameTime()
    {
        gameTime += Time.deltaTime; // �ð� ����
        int minutes = Mathf.FloorToInt(gameTime / 60F); // �� ���
        int seconds = Mathf.FloorToInt(gameTime % 60F); // �� ���
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // �ð� �ؽ�Ʈ ������Ʈ
    }
}