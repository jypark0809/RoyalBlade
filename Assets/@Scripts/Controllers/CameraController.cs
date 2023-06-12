using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    public Transform PlayerTransform { get; set; }

    void LateUpdate()
    {
        if (PlayerTransform == null)
            return;

        if (PlayerTransform.position.y <= 4.5)
            return;

        transform.position = new Vector3(PlayerTransform.transform.position.x, PlayerTransform.transform.position.y, -10);
    }
}
