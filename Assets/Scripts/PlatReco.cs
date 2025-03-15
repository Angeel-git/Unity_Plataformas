using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatReco : MonoBehaviour
{
    [SerializeField] private Transform desde;
    [SerializeField] private Transform hasta;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(desde.position, hasta.position);
        Gizmos.DrawSphere(desde.position, 0.1f);
        Gizmos.DrawSphere(hasta.position, 0.1f);
    }
}
