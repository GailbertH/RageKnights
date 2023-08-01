using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAdventureController : MonoBehaviour
{
    [SerializeField]
    private float groundDist;
    [SerializeField]
    private LayerMask floor;
    [SerializeField]
    private Transform targetPosition1;
    private Transform targetPosition2;
    private NavMeshAgent navMeshAgent;
    public string adventureId;
    private void Awake()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 castPos = transform.position;
        Vector3 direction = -transform.up;
        direction.y += -1.5f;
        Debug.DrawRay(castPos, direction, Color.yellow);
        if (Physics.Raycast(castPos, direction, out hit, Mathf.Infinity, floor))
        {
            if (hit.collider != null)
            {
                Debug.Log("Did Hit");
                Vector3 movePos = transform.position;
                movePos.y = hit.point.y + groundDist;
                transform.position = movePos;
            }
        }

        navMeshAgent.destination = targetPosition1.position;
    }
}
