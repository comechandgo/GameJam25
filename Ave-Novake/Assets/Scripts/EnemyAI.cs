using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public Animator enemy_anim;
    public float hp;
    public float max_hp;
    public float enemy_speed;
    public float follow_distance;
    public float abandon_follow_distance;
    // Start is called before the first frame update
    void Start()
    {
        enemy_anim = GetComponent<Animator>();
        hp = max_hp;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyAction();
    }

    void EnemyAction()
    {
        float face = transform.position.x - target.position.x;
        float delta_distance = Mathf.Abs(transform.position.x - target.position.x);
        if (delta_distance > follow_distance && delta_distance < abandon_follow_distance)
        {
            transform.position = Vector2.MoveTowards(transform.position,target.position,enemy_speed * Time.deltaTime);
            enemy_anim.SetFloat("run",1.0f);
            if (face > 0)
            {
                transform.localScale = new Vector3(1.0f, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-1.0f, transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
            enemy_anim.SetFloat("run",0.0f);
        }
    }
}
