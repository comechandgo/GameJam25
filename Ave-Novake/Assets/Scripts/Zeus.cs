using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using JetBrains.Annotations;
using UnityEngine;

public class Zeus : MonoBehaviour
{
    public GameObject enemy_prefab; //预制体，在Unity内设置
    public Transform player_transform; //玩家位置，在Start中用标签Player查找
    public float spawn_distace = 15.0f; //在玩家前多少生成，其实这个也可以在Unity内设置
    /*
    public float spawn_cd;
    public float spawn_timer = 0.0f;
    */
    public int[] spawn_map = {1, 1};
    public int[] spawn_map_copy = {1, 1};

    void Start()
    {
        player_transform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        ProactivelySpawn();
        TrackingSpawn(30.0f, 40.0f, spawn_map, 0);
        TrackingSpawn(-10.0f, 0.0f, spawn_map, 1);
    }

    void ProactivelySpawn()
    {
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            Spawn();
        }
    }

    void TrackingSpawn(float min, float max, int[] map, int point)
    {
        if (player_transform.position.x > min && player_transform.position.x < max && map[point] > 0)
        {
            Spawn();
            map[point]--;
        }
        else if (player_transform.position.x < min || player_transform.position.x > max)
        {
            map[point] = spawn_map_copy[point];
        }
    }

    void Spawn()
    {
        Vector3 spawn_position;
        if (player_transform.localScale.x > 0) //判断玩家朝向，保证怪刷在玩家前面
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
