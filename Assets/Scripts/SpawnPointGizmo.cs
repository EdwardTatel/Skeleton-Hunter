using UnityEngine;

public class SpawnPointGizmo : MonoBehaviour
{
    public Color gizmoColor = Color.green;
    public float gizmoSize = 0.5f;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(transform.position, Vector3.one * gizmoSize);
    }
}