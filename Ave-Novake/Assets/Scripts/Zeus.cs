using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class Zeus : MonoBehaviour
{
    public GameObject enemy_prefab;
    public Transform player_transform;
    public float spawn_distace = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        player_transform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Spawn();
    }

    void Spawn()
    {
        Vector3 spawn_position;
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (player_transform.localScale.x > 0)
            {
                spawn_position = player_transform.position + -1.0f * player_transform.right * spawn_distace;
            }
            else
            {
                spawn_position = player_transform.position + player_transform.right * spawn_distace;
            }
            Instantiate(enemy_prefab,spawn_position,Quaternion.identity);
        }
    }
}
