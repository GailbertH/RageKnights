using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField]
    private float radius;
    [SerializeField]
    [Range(0,100)]
    private float angle;

    [SerializeField]
    private LayerMask targetMask;
    [SerializeField]
    private LayerMask obstructionMask;

    public bool isSeeingPlayer;
    public GameObject playerReference;
    private Coroutine fieldOfViewRoutine;

    public float GetAngle { get { return this.angle; } }
    public float GetRadius { get { return this.radius;} }

    private void Start()
    {
        fieldOfViewRoutine = StartCoroutine(FOVRoutine());
    }

    private void OnDestroy()
    {
        StopCoroutine(fieldOfViewRoutine);
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.right, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    isSeeingPlayer = true;
                }
                else
                    isSeeingPlayer = false;
            }
            else
                isSeeingPlayer = false;
        }
        else if (isSeeingPlayer)
            isSeeingPlayer = false;
    }
}
