using UnityEngine;

public class Finish : MonoBehaviour
{
    // 충돌을 감지할 오브젝트의 태그를 설정합니다.
    public string targetTag = "Goal";

    // 충돌 감지 메서드 (트리거 충돌)
    private void OnTriggerEnter(Collider other)
    {
        // 트리거된 오브젝트의 태그가 설정한 태그와 일치하는지 확인합니다.
        if (other.gameObject.CompareTag(targetTag))
        {
            // 콘솔에 메시지 출력
            Debug.Log("클리어했습니다!");

            // 게임 종료
            Application.Quit();

            // 에디터에서 게임을 종료하려면 아래 코드도 필요합니다.
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}