using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public ResultUI resultUI; // ResultUI ��ũ��Ʈ ����

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
            resultUI = FindObjectOfType<ResultUI>(); // ResultUI ��ũ��Ʈ �ڵ� �Ҵ�
        }
    }

    public void GameClear()
    {
        resultUI.ShowResult(); // ��� â ǥ��
        Time.timeScale = 0f; // ���� �ð� ����
        StartCoroutine(RestartGameAfterDelay(3f)); // 5�� �Ŀ� ���� �����
    }

    private IEnumerator RestartGameAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // �ǽð����� 5�� ���
        Time.timeScale = 1f; // ���� �ð� �ٽ� ����
        resultUI.RestartGame(); // ���� �����
    }
}
