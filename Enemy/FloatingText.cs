using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private Transform cam;
    public float DestroyTime = 2f;

    private void Start()
    {
        GameObject parentObject = transform.parent.gameObject;

        Destroy(parentObject, DestroyTime);
        Destroy(gameObject, DestroyTime);
        cam = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (cam != null)
        {
            transform.LookAt(transform.position + cam.rotation * Vector3.forward,
            cam.rotation * Vector3.up);
        }
    }
}
