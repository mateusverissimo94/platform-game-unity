using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset;

    void LateUpdate()
    {
        if (playerTransform != null)
        {
            transform.position = playerTransform.position + offset;

            if (playerTransform.localScale.x > 0)
            {
                transform.localScale = new Vector3(1f,1f,1f);
            } else
            {
                transform.localScale = new Vector3(-1f,1f,1f);
            }
        }
    }
}
