using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform PlayerTransform { get; set; }

    void LateUpdate()
    {
        if (PlayerTransform == null)
            return;

        if (PlayerTransform.position.y <= 7f)
            return;

        transform.position = new Vector3(PlayerTransform.transform.position.x, PlayerTransform.transform.position.y, -10);
    }
}
