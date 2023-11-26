using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private GameObject target;

    private void Update()
    {
        Vector3 look = transform.InverseTransformPoint(target.transform.position);
        float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg + 90;

        transform.Rotate(0, 0, angle); 
    }
}
