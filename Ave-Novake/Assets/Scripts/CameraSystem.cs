using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    public Transform player_target;
    public float move_time;
    /*
    private void LateUpdate()
    {
        if (player_target != null && player_target.position != transform.position)
        {
            
            if (player_target.position != transform.position)
            {
                transform.position = Vector3.Lerp(transform.position,player_target.position,move_time * Time.deltaTime);
            }
            
            transform.position = Vector3.Lerp(transform.position,player_target.position,move_time * Time.deltaTime);
        }
    }
    */
    void Start()
    {
        player_target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        if (player_target != null && player_target.position != transform.position)
        {
            
            if (player_target.position != transform.position)
            {
                transform.position = Vector3.Lerp(transform.position,player_target.position,move_time * Time.deltaTime);
            }
            
            transform.position = Vector3.Lerp(transform.position,player_target.position,move_time * Time.deltaTime);
        }
    }
}
