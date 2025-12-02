using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Begin : MonoBehaviour
{
    public float min_stby_timer;
    public float min_stby;
    public bool waiting;
    // Start is called before the first frame update
    void Start()
    {
        min_stby_timer = 0.0f;
        waiting = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (waiting)
        {
            min_stby_timer += Time.deltaTime;
            if (min_stby_timer >= min_stby)
            {
                min_stby_timer = min_stby;
                waiting = false;
            }
        }
        else if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(1);
        }
    }
}
