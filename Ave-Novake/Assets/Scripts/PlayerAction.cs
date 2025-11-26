using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    //移动相关
    public float player_speed;
    public float player_jump_speed;
    public bool player_two_feet_on_the_ground;
    public Transform player_feet;
    public LayerMask ground;
    public Rigidbody2D player_rb;
    public Collider2D player_coll;
    public Animator player_anim;

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
        player_two_feet_on_the_ground = Physics2D.OverlapCircle(player_feet.position,0.1f,ground);
        PlayerAttack();
        PlayerMove();
    }

    //移动（运动）函数
    void PlayerMove()
    {
        //检测水平方向按键是否被按下
        float horizontal_num = Input.GetAxis("Horizontal");
        float face = Input.GetAxisRaw("Horizontal");

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
            player_anim.SetFloat("run",Mathf.Abs(player_speed * horizontal_num));
            player_anim.SetBool("jump",false);
        }
        else
        {
            player_anim.SetFloat("run",0.0f);
            player_anim.SetBool("jump",true);
        }
    }

    //攻击系统
    void PlayerAttack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            player_anim.SetTrigger("attack");
        }
    }
    
}