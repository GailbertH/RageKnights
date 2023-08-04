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

    public float GetAngle { get { return this.angle; } }
    public float GetRadius { get { return this.radius;} }

    public void Start()
    {
        //playerReference = playRef;
    }

    // Update is called once per frame
    void Update()
    {
        FieldOfViewCheck();
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Debug.Log("RangeChecks");
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            Debug.Log("Angle - " +Vector3.Angle(transform.right, directionToTarget));
            if (Vector3.Angle(transform.right, directionToTarget) < angle / 2)
            {
                Debug.Log("AngleCheck");
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    Debug.Log("SightCheck");
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
