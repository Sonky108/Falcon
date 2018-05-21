using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour {

    [SerializeField]
    private Transform FollowedObject;

    public Vector3 Offset;

    private void Update()
    {
        transform.position = FollowedObject.position + Offset;
    }
}
