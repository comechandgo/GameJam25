using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    public Transform player_target;
    public float move_time;
    void Start()
    {
        player_target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        CameraMoving(-5.0f, 40.0f);
    }

    void CameraMoving(float left_b, float right_b)
    {
        Vector3 target_position = transform.position;
        if (player_target != null && player_target.position != transform.position && player_target.position.x > left_b && player_target.position.x < right_b)
        {
            target_position = player_target.position;
        }
        else if (player_target.position.x < left_b)
        {
            target_position = new Vector3(left_b, player_target.position.y, transform.position.z);
        }
        else if (player_target.position.x > right_b)
        {
            target_position = new Vector3(right_b, player_target.position.y, transform.position.z);
        }
        transform.position = Vector3.Lerp(transform.position, target_position, move_time * Time.deltaTime);
    }
}
