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
            PlayerController player = other.GetComponent<PlayerController>(); //PlayerBall�� ��ũ��Ʈ ������Ʈ ��������
            player.itemCnt++; //������ ī��Ʈ + 1
            gameObject.SetActive(false); //������Ʈ ��Ȱ��ȭ
        }
    }
}