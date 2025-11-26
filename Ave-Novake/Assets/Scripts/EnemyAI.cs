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
    public int anim_key;
    public bool hurt = false;
    public float hurt_timer = 0.0f;
    public float hurt_duration = 1.05f;
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
        EnemyAnimChange();
    }

    void EnemyAction()
    {
        float face = transform.position.x - target.position.x;
        float delta_distance = Mathf.Abs(transform.position.x - target.position.x);
        if (delta_distance > follow_distance && delta_distance < abandon_follow_distance)
        {
            transform.position = Vector2.MoveTowards(transform.position,target.position,enemy_speed * Time.deltaTime);
            if (face > 0)
            {
                transform.localScale = new Vector3(1.0f, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-1.0f, transform.localScale.y, transform.localScale.z);
            }
            anim_key = 3;
        }
        else
        {
            enemy_anim.SetFloat("run",0.0f);
            anim_key = 2;
        }
    }

    //受击系统
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hp >= 0)
        {
            if (other.CompareTag("Weapon"))
            {
                hp -= 5;
                hurt = true;
                enemy_anim.SetTrigger("hurt");
            }
            //拓展槽，可以给多样的受击
        }
    }

    /*
    动画切换器(避免挨揍的时候放不出受击动画)
    动画代号(i)的对照表：
    0：受击 1.05s (实际上anim_key不会被设置为0)
    1：攻击
    2：待机
    3：跑动
    */
    void EnemyAnimChange()
    {
        if (hurt)
        {
            hurt_timer += Time.deltaTime;
            if (hurt_timer >= hurt_duration)
            {
                hurt_timer = 0.0f;
                hurt = false;
                anim_key = 2;
            }
        }
        else
        {
            if (anim_key == 2)
            {
                enemy_anim.SetFloat("run",0.0f);
            }
            else if (anim_key == 3)
            {
                enemy_anim.SetFloat("run",1.0f);
            }
        }
    }
}
