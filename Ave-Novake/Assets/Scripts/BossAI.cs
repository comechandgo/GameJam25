using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossAI : MonoBehaviour
{
    public Rigidbody2D enemy_rb;
    public Animator enemy_anim;
    public Transform target;
    public GameObject target_GO;
    public NewBehaviourScript player_script;
    public string target_tag = "Player";
    public int working_status = 1;
    public float hit_cd;
    public float hp;
    public Collider2D door_protect;
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
    public int boss_level;
    public GameObject enemy_prefab;

    // Start is called before the first frame update
    void Start()
    {
        enemy_rb = GetComponent<Rigidbody2D>();
        enemy_anim = GetComponent<Animator>();
        target_GO = GameObject.FindGameObjectWithTag(target_tag);
        target = target_GO.transform;
        player_script = target_GO.GetComponent<NewBehaviourScript>();
        door_protect = GameObject.FindGameObjectWithTag("Door").GetComponent<Collider2D>();
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
            if (working_status == 1 && target_GO.tag == target_tag)
            {
                float delta_distance = Mathf.Abs(transform.position.x - target.position.x);
                float face = transform.position.x - target.position.x;
                if (hurt == 0)
                {
                    if (delta_distance < follow_distance && attack == 0 && !door_protect.enabled)
                    {
                        if (boss_level == 1)
                        {
                            player_script.player_hp -= 20.0f;
                        }
                        else if (boss_level == 2)
                        {
                            Vector3 spawn_position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                            for (int i = 1; i <= 2; i++)
                            {
                                Instantiate(enemy_prefab, spawn_position, Quaternion.identity);
                            }
                        }
                        enemy_anim.SetTrigger("attack");
                        attack = 1;
                    }
                    else if (delta_distance > follow_distance && delta_distance < abandon_follow_distance && !door_protect.enabled)
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
        else
        {
            destroy_timer -= Time.deltaTime;
            if (destroy_timer <= 0)
            {
                destroy_timer = 0.0f;
                if (boss_level == 1)
                {
                    SceneManager.LoadScene(3);
                }
                else if (boss_level == 2)
                {
                    SceneManager.LoadScene(0);
                }
            }
        }
    }

    //受击系统
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hp >= 0.0f && !door_protect.enabled)
        {
            if (other.CompareTag("Weapon"))
            {
                InjuryJudgment(10.0f);
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
