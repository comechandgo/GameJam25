using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public Collider2D coll2D;
    public float lifetime = 5.0f;
    public float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        coll2D = GetComponent<Collider2D>();
        coll2D.enabled = true;
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            AmmoDestroy();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hostile"))
        {
            AmmoDestroy();
        }
    }

    void AmmoDestroy()
    {
        coll2D.enabled = false;
        Destroy(gameObject);
    }
}
