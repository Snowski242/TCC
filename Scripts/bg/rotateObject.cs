using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateObject : MonoBehaviour
{
    [SerializeField] float _degreesPerSecond;
    [SerializeField] Vector3 _axis = Vector3.forward;
    void Update()
    {
        transform.Rotate(_axis.normalized * _degreesPerSecond * Time.deltaTime);
    }
}
