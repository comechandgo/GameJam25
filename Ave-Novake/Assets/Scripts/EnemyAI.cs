using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Rigidbody2D enemy_rb;
    public Animator enemy_anim;
    public Transform target;
    public GameObject[] new_target;
    public bool friendly_status = false;
    public bool need_to_seek = false;
    public int working_status = 1;
    public float hp;
    //public float max_hp;
    public float enemy_speed;
    public float follow_distance;
    public float abandon_follow_distance;
    public int anim_key;
    public int hurt = 0;
    public float hurt_timer = 0.0f;
    public float hurt_duration;
    public int attack = 0;
    public float attack_timer = 0.0f;
    public float attack_duration;
    // Start is called before the first frame update
    void Start()
    {
        enemy_rb = GetComponent<Rigidbody2D>();
        enemy_anim = GetComponent<Animator>();
        //hp = max_hp;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyAnimChanger();
        EnemyAction();
    }

    //行动系统
    void EnemyAction()
    {
        if (friendly_status && need_to_seek)
        {
            new_target = GameObject.FindGameObjectsWithTag("Hostile");
            if (new_target.Length >= 1)
            {
                float distance, new_target_distance = Mathf.Infinity;
                for (int i = 0; i < new_target.Length; i++)
                {
                    if (new_target[i] != null)
                    {
                        distance = Vector3.Distance(transform.position, new_target[i].transform.position);
                        if (distance < new_target_distance)
                        {
                            target = new_target[i].transform;
                            new_target_distance = distance;
                        }
                    }
                }
                if (working_status != 1)
                {
                    working_status = 1;
                }
                need_to_seek = false;
            }
            else
            {
                working_status = 0;
            }
        }
        if (working_status == 1)
        {
            float delta_distance = Mathf.Abs(transform.position.x - target.position.x);
            float face = transform.position.x - target.position.x;
            if (hurt == 0)
            {
                if (delta_distance < follow_distance && attack == 0)
                {
                    attack = 1;
                    enemy_anim.SetTrigger("attack");
                }
                else if (delta_distance > follow_distance && delta_distance < abandon_follow_distance)
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
                else if (delta_distance > abandon_follow_distance)
                {
                    anim_key = 2;
                }
            }
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
                enemy_anim.SetTrigger("hurt");
            }
            else if (other.CompareTag("Hostile Weapon"))
            {
                hp -= 5;
                enemy_anim.SetTrigger("hurt");
            }
            else if (other.CompareTag("Skill"))
            {
                gameObject.tag = "Allies";
                gameObject.layer = 7;
                friendly_status = true;
                need_to_seek = true;
                enemy_anim.SetTrigger("hurt");
            }
            hurt = 1;
            attack = 0;
            attack_timer = 0.0f;
            //拓展槽，可以给多样的受击
        }
    }

    /*
    动画切换器(避免挨揍的时候放不出受击动画)
    动画代号(i)的对照表：
    0：受击 1.05s (实际上anim_key不会被设置为0)
    1：攻击 1.05s (实际上anim_key不会被设置为1)
    2：待机
    3：跑动
    */
    void EnemyAnimChanger()
    {
        if (hurt == 1)
        {
            hurt_timer += Time.deltaTime;
            if (hurt_timer >= hurt_duration)
            {
                hurt_timer = 0.0f;
                hurt = 0;
                anim_key = 2;
            }
        }
        else if (attack == 1)
        {
            attack_timer += Time.deltaTime;
                if (attack_timer >= attack_duration)
                {
                    attack_timer = 0.0f;
                    attack = 0;
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
