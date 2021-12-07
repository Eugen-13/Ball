using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformMove : MonoBehaviour
{
    private float borderX;
    public float speed = 10f;

    private void Awake()
    {
        borderX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0)).x;
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if ((Mathf.Abs(transform.position.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x) > 0.05f))
            {
                int i = 0;
                if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x > 0)
                    i = 1;
                else
                    i = -1;
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime * i, transform.localPosition.y);
            }
            if (transform.position.x > borderX)
                transform.position = new Vector3(borderX, transform.localPosition.y);
            else if (transform.position.x < -borderX)
                transform.position = new Vector3(-borderX, transform.localPosition.y);
        }
    }
}
