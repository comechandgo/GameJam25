using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    //移动相关
    public float player_speed;
    public float player_jump_speed;
    public bool player_two_feet_on_the_ground;
    public float horizontal_num;
    public Transform player_feet;
    public LayerMask ground;
    public Rigidbody2D player_rb;
    public Collider2D player_coll;
    public Animator player_anim;
    public int player_hurt = 0;
    public float player_hurt_timer = 0.0f;
    public float player_hurt_duration;
    public int player_attack = 0;
    public float player_attack_timer = 0.0f;
    public float player_attack_duration;
    public int player_skill = 0;
    public float player_skill_timer = 0.0f;
    public float player_skill_duration;
    public float player_skill_cd;
    public float player_skill_cd_timer = 0.0f;
    public int player_anim_key = 2;
    public GameObject player_skill_ammo;
    public float player_skill_ammo_speed;
    public float player_hp;
    //public float player_max_hp;
    /*
    public AudioSource player_audio;
    public AudioClip[] music_resource;
    public int music_number = 0;
    */

    // Start is called before the first frame update
    void Start()
    {
        player_rb = GetComponent<Rigidbody2D>();
        player_coll = GetComponent<Collider2D>();
        player_anim = GetComponent<Animator>();
        player_feet = GameObject.FindGameObjectWithTag("Feet").transform;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerAnimChanger();
        PlayerAttack();
        PlayerMove();
        HPLine();
    }

    //移动（运动）函数
    void PlayerMove()
    {
        player_two_feet_on_the_ground = Physics2D.OverlapCircle(player_feet.position,0.1f,ground);
        //检测水平方向按键是否被按下
        horizontal_num = Input.GetAxis("Horizontal");
        float face = Input.GetAxisRaw("Horizontal");
        if (player_hurt == 0 && player_attack == 0 && player_skill == 0)
        {
            //判断朝向
            if (face != 0)
            {
                transform.localScale = new Vector3(-face, transform.localScale.y, transform.localScale.z);
            }

            //改变水平速度
            player_rb.velocity = new Vector2(player_speed * horizontal_num, player_rb.velocity.y);

            //改变竖直速度
            if (Input.GetButton("Jump") && player_two_feet_on_the_ground)
            {
                player_rb.velocity = new Vector2(player_rb.velocity.x,player_jump_speed);
            }

            //防二段跳
            if (player_two_feet_on_the_ground)
            {
                player_anim_key = 2;
            }
            else
            {
                player_anim_key = 3;
            }
        }
    }

    //攻击系统
    void PlayerAttack()
    {
        if (player_hurt == 0 && player_attack == 0 && player_skill == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                player_attack = 1;
                player_anim.SetTrigger("attack");
            }
            else if (Input.GetKeyDown(KeyCode.E) && player_skill_cd_timer == 0.0f)
            {
                player_skill = 1;
                player_skill_cd_timer = player_skill_cd;
                player_anim.SetTrigger("skill_1");
                Vector3 ammo_position = transform.position + player_feet.up * 0.7f;
                GameObject ammo = Instantiate(player_skill_ammo,ammo_position,Quaternion.identity);
                Rigidbody2D ammo_rb = ammo.GetComponent<Rigidbody2D>();
                if (transform.localScale.x > 0)
                {
                    ammo_rb.velocity = new Vector2(-1.0f * (player_rb.velocity.x + player_skill_ammo_speed), ammo_rb.velocity.y);
                }
                else
                {
                    ammo_rb.velocity = new Vector2(player_rb.velocity.x + player_skill_ammo_speed, ammo_rb.velocity.y);
                }
            }
        }
        if (player_skill_cd_timer >= 0.0f)
        {
            player_skill_cd_timer -= Time.deltaTime;
            if (player_skill_cd_timer < 0)
            {
                player_skill_cd_timer = 0.0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (player_hp >= 0)
        {
            if (other.CompareTag("Hostile Weapon"))
            {
                player_hp -= 5;
                player_hurt = 1;
                player_anim.SetTrigger("hurt");
                player_attack = 0;
                player_attack_timer = 0.0f;
                player_skill = 0;
                player_skill_timer = 0.0f;
            }
            //拓展槽，可以给多样的受击
        }
    }

    /*
    动画切换器(避免挨揍的时候放不出受击动画)
    动画代号(i)的对照表：
    0：受击 1.05s (实际上anim_key不会被设置为0)
    2：待机或跑动
    3：跳跃
    */
    void PlayerAnimChanger()
    {
        if (player_hurt == 1)
        {
            player_hurt_timer += Time.deltaTime;
            if (player_hurt_timer >= player_hurt_duration)
            {
                player_hurt_timer = 0.0f;
                player_hurt = 0;
                player_anim_key = 2;
            }
        }
        else if (player_attack == 1)
        {
            player_attack_timer += Time.deltaTime;
            if (player_attack_timer >= player_attack_duration)
            {
                player_attack_timer = 0.0f;
                player_attack = 0;
                player_anim_key = 2;
            }
        }
        else if (player_skill == 1)
        {
            player_skill_timer += Time.deltaTime;
            if (player_skill_timer >= player_skill_duration)
            {
                player_skill_timer = 0.0f;
                player_skill = 0;
                player_anim_key = 2;
            }
        }
        else
        {
            if (player_anim_key == 2)
            {
                player_anim.SetFloat("run",Mathf.Abs(player_speed * horizontal_num));
                player_anim.SetBool("jump",false);
            }
            else if (player_anim_key == 3)
            {
                player_anim.SetFloat("run",0.0f);
                player_anim.SetBool("jump",true);
            }
        }
    }

    void HPLine()
    {
        if (player_hp >= 0)
        {
            float new_hp = player_hp / 100.0f;
            Transform hp_line = transform.Find("hp");
            hp_line.localScale = new Vector3(new_hp, hp_line.localScale.y, hp_line.localScale.z);
        }
        else
        {
            player_hp = 0;
        }
    }
}