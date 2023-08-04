using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.right, 360, fov.GetRadius);

        Vector3 leftAngle = DirectionFromAngle(fov.transform.eulerAngles.y * -1, fov.GetAngle / 2);
        Vector3 rightAngle = DirectionFromAngle(fov.transform.eulerAngles.y * -1, -fov.GetAngle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + leftAngle * fov.GetRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + rightAngle * fov.GetRadius);

        if (fov.isSeeingPlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, fov.playerReference.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegress)
    {
        angleInDegress += eulerY;
        return new Vector3(Mathf.Cos(angleInDegress * Mathf.Deg2Rad), 0, Mathf.Sin(angleInDegress * Mathf.Deg2Rad));
    }

}
