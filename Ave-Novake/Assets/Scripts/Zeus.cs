using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class Zeus : MonoBehaviour
{
    public GameObject enemy_prefab; //预制体，在Unity内设置
    public Transform player_transform; //玩家位置，在Start中用标签Player查找
    public float spawn_distace = 10.0f; //在玩家前多少生成，其实这个也可以在Unity内设置

    void Start()
    {
        player_transform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        Spawn();
    }

    void Spawn()
    {
        Vector3 spawn_position;
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (player_transform.localScale.x > 0) //判断玩家朝向，保证怪刷在玩家前面
            {
                spawn_position = player_transform.position + -1.0f * player_transform.right * spawn_distace;
            }
            else
            {
                spawn_position = player_transform.position + player_transform.right * spawn_distace;
            }
            Instantiate(enemy_prefab,spawn_position,Quaternion.identity); //实例化
        }
    }
}
