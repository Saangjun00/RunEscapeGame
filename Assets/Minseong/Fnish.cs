using UnityEngine;

public class Finish : MonoBehaviour
{
    // �浹�� ������ ������Ʈ�� �±׸� �����մϴ�.
    public string targetTag = "Goal";

    // �浹 ���� �޼��� (Ʈ���� �浹)
    private void OnTriggerEnter(Collider other)
    {
        // Ʈ���ŵ� ������Ʈ�� �±װ� ������ �±׿� ��ġ�ϴ��� Ȯ���մϴ�.
        if (other.gameObject.CompareTag(targetTag))
        {
            // �ֿܼ� �޽��� ���
            Debug.Log("Ŭ�����߽��ϴ�!");

            // ���� ����
            Application.Quit();

            // �����Ϳ��� ������ �����Ϸ��� �Ʒ� �ڵ嵵 �ʿ��մϴ�.
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}