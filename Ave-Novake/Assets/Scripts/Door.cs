using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Collider2D door;
    public Transform player;
    public float cd_timer = 0.0f;
    public float cd = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        door = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(player.position.x - transform.position.x) <= 2 && Input.GetKey(KeyCode.F))
        {
            float waiting = (cd - cd_timer) / cd;
            Transform waiting_line = transform.Find("hp");
            waiting_line.localScale = new Vector3(5.0f * waiting, waiting_line.localScale.y, waiting_line.localScale.z);
            cd_timer += Time.deltaTime;
            if (cd_timer >= cd)
            {
                cd_timer = cd;
                door.enabled = false;
                transform.localScale = new Vector3(0, transform.localScale.y, transform.localScale.z);
            }
        }
    }
}
