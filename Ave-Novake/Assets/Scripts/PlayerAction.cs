using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
        player_two_feet_on_the_ground = Physics2D.OverlapCircle(player_feet.position,0.1f,ground);
        PlayerAttack();
        PlayerMove();
    }

    void PlayerMove()
    {
        float horizontal_num = Input.GetAxis("Horizontal");
        float face = Input.GetAxisRaw("Horizontal");
        if (face != 0)
        {
            transform.localScale = new Vector3(-face, transform.localScale.y, transform.localScale.z);
        }
        player_rb.velocity = new Vector2(player_speed * horizontal_num, player_rb.velocity.y);
        if (Input.GetButton("Jump") && player_two_feet_on_the_ground)
        {
            player_rb.velocity = new Vector2(player_rb.velocity.x,player_jump_speed);
        }
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
    
    void PlayerAttack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            player_anim.SetTrigger("attack");
        }
    }
    
}