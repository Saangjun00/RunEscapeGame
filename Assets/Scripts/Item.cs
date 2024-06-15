using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float rotateSpeed;

    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerController player = other.GetComponent<PlayerController>(); //PlayerBall의 스크립트 컴포넌트 가져오기
            player.itemCnt++; //아이템 카운트 + 1
            gameObject.SetActive(false); //오브젝트 비활성화
        }
    }
}