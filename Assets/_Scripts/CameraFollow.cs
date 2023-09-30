using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    Bounds camBounds;
    float speed;
    Player player;

    private void Start()
    {
        speed = playerTransform.gameObject.GetComponent<Player>().speed;
        camBounds = GameManager.instance.levelBounds;
        camBounds.extents -= new Vector3(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize, 0);
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
        player = playerTransform.GetComponent<Player>();
    }

    void Update()
    {
        if (playerTransform == null)
            return;
        float z = transform.position.z;
        Vector3 target = player.GetCameraTarget();
        target.z = z;
        if ((transform.position - target).magnitude > 0)
        {
            transform.position = Vector3.Slerp(transform.position, target, speed * Time.deltaTime);
        }
        if (!camBounds.Contains(transform.position))
        {
            transform.position = camBounds.ClosestPoint(transform.position);
        }
    }
}