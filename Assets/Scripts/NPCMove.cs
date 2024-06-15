using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMove : MonoBehaviour
{
    NavMeshAgent _navMeshAgent;           // 네비게이션 에이전트 
    Transform objectToChase;              // 추격할 위치

    public void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        objectToChase = GameObject.FindGameObjectWithTag("Player").transform;     // 플레이어 위치로
        if (_navMeshAgent == null)
        {
            Debug.LogError("Nav Mesh Agent component not found attached to " + gameObject.name);
        }
        else _navMeshAgent.SetDestination(objectToChase.position);            // 플레이어를 향해 이동
    }

    public void Update()
    {
        objectToChase = GameObject.FindGameObjectWithTag("Player").transform;
        _navMeshAgent.SetDestination(objectToChase.position);
    }
}
