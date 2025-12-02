using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Rigidbody2D enemy_rb;
    public Animator enemy_anim;
    public Transform target;
    public GameObject target_GO;
    public GameObject[] new_target;
    public EnemyAI target_ai;
    public string target_tag = "Player";
    public bool friendly_status = false;
    public bool need_to_seek = false;
    public int working_status = 1;
    public float hp;
    public float destroy_timer = 3.0f;
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
        target_GO = GameObject.FindGameObjectWithTag(target_tag);
        target = target_GO.transform;
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
        if (hp > 0.0f)
        {
            if (friendly_status && !need_to_seek)
            {
                if (target_ai.hp <= 0.0f)
                {
                    need_to_seek = true;
                    target_GO = null;
                    target = null;
                    target_ai = null;
                }
            }
            if (friendly_status && need_to_seek)
            {
                anim_key = 2;
                working_status = 0;
                new_target = GameObject.FindGameObjectsWithTag(target_tag);
                if (new_target.Length >= 1)
                {
                    float distance, new_target_distance = abandon_follow_distance;
                    for (int i = 0; i < new_target.Length; i++)
                    {
                        if (new_target[i] != null && new_target[i].GetComponent<EnemyAI>().hp > 0)
                        {
                            distance = Vector3.Distance(transform.position, new_target[i].transform.position);
                            if (distance < new_target_distance)
                            {
                                target_GO = new_target[i];
                                target_ai = target_GO.GetComponent<EnemyAI>();
                                target = target_GO.transform;
                                new_target_distance = distance;
                                need_to_seek = false;
                                if (working_status != 1)
                                {
                                    working_status = 1;
                                }
                            }
                        }
                    }
                }
            }
            if (working_status == 1 && target_GO.tag == target_tag)
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
            else if (!need_to_seek)
            {
                if (target_GO.tag != target_tag)
                {
                    need_to_seek = true;
                }
            }
        }
        else
        {
            destroy_timer -= Time.deltaTime;
            if (destroy_timer <= 0)
            {
                destroy_timer = 0.0f;
                Destroy(gameObject);
            }
        }
    }

    //受击系统
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hp >= 0.0f)
        {
            if (other.CompareTag("Weapon") && !friendly_status)
            {
                InjuryJudgment(10.0f);
            }
            else if (other.CompareTag("Skill"))
            {
                gameObject.tag = "Allies";
                transform.Find("enemy attack").tag = "Weapon";
                transform.Find("hp").GetComponent<SpriteRenderer>().color = Color.green;
                gameObject.layer = 7;
                friendly_status = true;
                need_to_seek = true;
                target_tag = "Hostile";
                enemy_anim.SetTrigger("hurt");
            }
            if (other.CompareTag("Hostile Weapon"))
            {
                InjuryJudgment(5.0f);
            }
            hurt = 1;
            attack = 0;
            attack_timer = 0.0f;
            //拓展槽，可以给多样的受击
        }
    }

    void InjuryJudgment(float m_hp)
    {
        hp -= m_hp;
        Transform hp_line = transform.Find("hp");
        if (hp > 0.0f)
        {
            enemy_anim.SetTrigger("hurt");
            float new_hp = hp / 100.0f;
            hp_line.localScale = new Vector3(new_hp, hp_line.localScale.y, hp_line.localScale.z);
        }
        else
        {
            hp_line.localScale = new Vector3(0, hp_line.localScale.y, hp_line.localScale.z);
            enemy_anim.SetBool("dead",true);
            hp = 0.0f;
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
