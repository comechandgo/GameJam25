using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

public class Zeus : MonoBehaviour
{
    public GameObject[] enemy_prefab; //预制体，在Unity内设置
    public Transform player_transform; //玩家位置，在Start中用标签Player查找
    public float spawn_distace = 15.0f; //在玩家前多少生成，其实这个也可以在Unity内设置
    public float spawn_cd;
    public float spawn_cd_timer = 0.0f;
    public bool spawn_available = true;
    public int[] spawn_map;
    public AudioClip[] bgms;
    public int[] banned_group;
    private bool new_bgm = true;
    public AudioSource bgm_source;
    private int playing_bgm_num = 0;
    private bool in_the_banned_group = false;

    void Start()
    {
        bgm_source = GetComponent<AudioSource>();
        bgm_source.clip = bgms[0];
        bgm_source.Play();
        //FixedPointSpawnOneEnemy(30.0f, 0.0f);
        player_transform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        GlobalSpawnTimer();
        ProactivelySpawn();
        TrackingSpawn(-3.0f, 10.0f, spawn_map, 0, 0);
        TrackingSpawn(12.0f, 20.0f, spawn_map, 1, 1);
        ChangeMusic();
        TrackPlayerForMusic(-5.0f, 0.0f, 1);
        TrackPlayerForMusic(0.0f, 20.0f, 3);
        TrackPlayerForMusic(20.0f,30.0f, 4);
        TrackPlayerForMusic(35.0f,50.0f, 6);
    }

    void ProactivelySpawn()
    {
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            Spawn(0);
        }
    }

    void GlobalSpawnTimer()
    {
        if (spawn_cd_timer > 0)
        {
            spawn_cd_timer -= Time.deltaTime;
            if (spawn_cd_timer <= 0)
            {
                spawn_cd_timer = 0;
                spawn_available = true;
            }
        }
    }
    void TrackingSpawn(float min, float max, int[] map, int point, int num)
    {
        if (player_transform.position.x > min && player_transform.position.x < max && map[point] > 0 && spawn_available)
        {
            Spawn(num);
            map[point]--;
            spawn_available = false;
            spawn_cd_timer = spawn_cd;
        }
    }

    void Spawn(int n)
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
        Instantiate(enemy_prefab[n], spawn_position, Quaternion.identity);
    }

    void FixedPointSpawnOneEnemy(float x, float y, int num)
    {
        Vector3 spawn_position = new Vector3(x, y, transform.position.z);
        Instantiate(enemy_prefab[num], spawn_position, Quaternion.identity);
    }

    
    void ChangeMusic()
    {
        if (new_bgm)
        {
            for (int i = 0; i < banned_group.Length; i++)
            {
                if (playing_bgm_num == banned_group[i])
                {
                    in_the_banned_group = true;
                }
            }
        }
        if (in_the_banned_group)
        {
            bgm_source.loop = false;
            if (!bgm_source.isPlaying)
            {
                playing_bgm_num += 1;
                bgm_source.clip = bgms[playing_bgm_num];
                bgm_source.Play();
                in_the_banned_group = false;
                new_bgm = true;
                bgm_source.loop = true;
            }
        }
    }
    

    void TrackPlayerForMusic(float left_b, float right_b, int n)
    {
        if (player_transform.position.x > left_b && player_transform.position.x < right_b && playing_bgm_num < n)
        {
            bgm_source.loop = false;
            if (!bgm_source.isPlaying)
            {
                playing_bgm_num += 1;
                bgm_source.clip = bgms[playing_bgm_num];
                bgm_source.Play();
                new_bgm = true;
                bgm_source.loop = true;
            }
        }
        else if (playing_bgm_num > n && !in_the_banned_group)
        {
            bgm_source.loop = true;
        }
    }
}
