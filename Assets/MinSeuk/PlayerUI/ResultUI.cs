using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultUI : MonoBehaviour
{
    public GameObject resultPanel; // ��� â �г�
    public TextMeshProUGUI resultScoreText; // ��� ���� �ؽ�Ʈ
    public TextMeshProUGUI resultTimeText; // ��� �ð� �ؽ�Ʈ
    public PlayerUI ui; // UI ��ũ��Ʈ ����

    void Start()
    {
        if (resultPanel != null)
        {
            resultPanel.SetActive(false); // ���� �� ��� â ��Ȱ��ȭ
        }

        if (ui == null)
        {
            ui = FindObjectOfType<PlayerUI>(); // UI ��ũ��Ʈ �ڵ� �Ҵ�
        }
    }

    public void ShowResult()
    {
        if (resultPanel != null)
        {
            resultPanel.SetActive(true); // ��� â Ȱ��ȭ
        }

        // ������ �ð� ����
        resultScoreText.text = " " + ui.playerController.itemCnt.ToString();
        resultTimeText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(ui.gameTime / 60F), Mathf.FloorToInt(ui.gameTime % 60F));
    }

    public void HideResult()
    {
        if (resultPanel != null)
        {
            resultPanel.SetActive(false); // ��� â ��Ȱ��ȭ
        }
    }

    public void RestartGame()
    {
        HideResult(); // ��� â ��Ȱ��ȭ
        Time.timeScale = 1f; // ���� �ð� �ٽ� ����

        // ���� ������ �ð� �ʱ�ȭ
        ui.playerController.itemCnt = 0;
        ui.gameTime = 0f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // ���� �� �ٽ� �ε�
    }
}
