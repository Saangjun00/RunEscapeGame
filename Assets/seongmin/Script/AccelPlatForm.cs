using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AccelPlatForm : MonoBehaviour
{
    [SerializeField]
    private float accelForce;
    [SerializeField]
    private Vector3 direction;
    private AudioSource audioSource;

    public PlayerController playerController;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        playerController = GetComponent<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            audioSource.Play();

            collision.transform.GetComponent<PlayerController>().MoveDirection(direction, accelForce);
        }
    }
}
