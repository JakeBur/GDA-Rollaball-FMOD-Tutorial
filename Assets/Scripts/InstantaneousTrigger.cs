using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class InstantaneousTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject _target;

    public Action OnTargetEnter;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == _target)
        {
            OnTargetEnter?.Invoke();
        }
    }
}
