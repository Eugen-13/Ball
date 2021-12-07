using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;



public class Behavior : MonoBehaviour
{
    public float speed = 5f;
    public Vector3 vector;
    public GameObject platform;
    public GameObject forTakingGlobalScript;

    new public CircleCollider2D collider;
    public CapsuleCollider2D capsule;
    private void Update()
    {
        Time.timeScale = forTakingGlobalScript.GetComponent<GlobalScript>().timeScale;
    }
    private void FixedUpdate()
    {
        transform.position = new Vector3(transform.localPosition.x + (vector.x * Time.deltaTime * speed), transform.localPosition.y + (vector.y * Time.deltaTime * speed));
        if (transform.position.y > forTakingGlobalScript.GetComponent<GlobalScript>().borderY - collider.radius * transform.localScale.y)
        {
            vector = new Vector3(vector.x, -(vector.y));
            transform.position = new Vector3(transform.localPosition.x + (vector.x * Time.deltaTime * speed), transform.localPosition.y + (vector.y * Time.deltaTime * speed));
        }
        if (transform.position.x < -forTakingGlobalScript.GetComponent<GlobalScript>().borderX + collider.radius * transform.localScale.x || transform.position.x > forTakingGlobalScript.GetComponent<GlobalScript>().borderX - collider.radius * transform.lossyScale.x)
        {
            vector = new Vector3(-vector.x, vector.y);
            transform.position = new Vector3(transform.localPosition.x + (vector.x * Time.deltaTime * speed), transform.localPosition.y + (vector.y * Time.deltaTime * speed));
        }
        if (transform.position.y < -forTakingGlobalScript.GetComponent<GlobalScript>().borderY - collider.radius)
        {
            forTakingGlobalScript.GetComponent<GlobalScript>().balls.Remove(forTakingGlobalScript.GetComponent<GlobalScript>().balls.Find(x => x == gameObject));
            Destroy(gameObject);
            Destroy(this);
        }
    }
    private void platformDel2()
    {
        platform.transform.localScale = new Vector3(platform.transform.localScale.x / 2, platform.transform.localScale.y);
    }
    private void platformX2()
    {
        platform.transform.localScale = new Vector3(platform.transform.localScale.x * 2, platform.transform.localScale.y);
    }
    private void ballDel2()
    {
        foreach (var ball in forTakingGlobalScript.GetComponent<GlobalScript>().balls)
        {
            ball.transform.localScale = new Vector3(ball.transform.localScale.x / 1.5f, ball.transform.localScale.y / 1.5f);
        }
    }
    private void ballX2()
    {
        foreach (var ball in forTakingGlobalScript.GetComponent<GlobalScript>().balls)
        {
            ball.transform.localScale = new Vector3(ball.transform.localScale.x * 1.5f, ball.transform.localScale.y * 1.5f);
        }
    }
    private void speedX2()
    {
        foreach (var ball in forTakingGlobalScript.GetComponent<GlobalScript>().balls)
        {
            ball.GetComponent<Behavior>().speed *= 2;
        }
    }
    private void speedDel2()
    {
        foreach (var ball in forTakingGlobalScript.GetComponent<GlobalScript>().balls)
        {
            ball.GetComponent<Behavior>().speed /= 2;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Platform")
        {
            if (transform.position.y < platform.transform.position.y)
            {
                float Angle = 0;
                if (platform.transform.position.x > transform.position.x)
                {
                    Angle = -(Mathf.Abs(platform.transform.position.x - transform.position.x) / (capsule.size.x / 2 * transform.localScale.x) * 0.75f);
                }
                if (platform.transform.position.x < transform.position.x)
                {
                    Angle = (Mathf.Abs(platform.transform.position.x - transform.position.x) / (capsule.size.x / 2 * transform.localScale.x) * 0.75f);
                }
                float x = Mathf.Cos(Mathf.Acos(vector.x) + Angle);
                float y = -Mathf.Sqrt(1 - Mathf.Pow(x, 2));
                vector = new Vector3(x, y);
            }
            else
            {
                float Angle = 0;
                if (platform.transform.position.x > transform.position.x)
                {
                    Angle = (Mathf.Abs(platform.transform.position.x - transform.position.x) / (capsule.size.x / 2 * transform.localScale.x) * Mathf.PI / 6);
                }
                else if(platform.transform.position.x < transform.position.x)
                {
                    Angle = -(Mathf.Abs(platform.transform.position.x - transform.position.x) / (capsule.size.x / 2 * transform.localScale.x) * Mathf.PI / 6);
                }
                else if(platform.transform.position.x == transform.position.x)
                {
                    vector = new Vector3(vector.x, -vector.y);
                    return;
                }
                float x = Mathf.Cos(Mathf.Acos(vector.x) + Angle);
                float y = Mathf.Sqrt(1 - Mathf.Pow(x, 2));
                vector = new Vector3(x, y); 
            }

        }
        else if(collision.tag == "CreatesObject")
        {
            Destroy(forTakingGlobalScript.GetComponent<GlobalScript>().squares.Find(x => x.transform.position.y == collision.transform.position.y && x.transform.position.x == collision.transform.position.x));
            forTakingGlobalScript.GetComponent<GlobalScript>().score++;
            forTakingGlobalScript.GetComponent<GlobalScript>().text.GetComponent<TextMeshProUGUI>().text = "Score: " + forTakingGlobalScript.GetComponent<GlobalScript>().score.ToString();
            forTakingGlobalScript.GetComponent<GlobalScript>().squares.Remove(forTakingGlobalScript.GetComponent<GlobalScript>().squares.Find(x => x.transform.position.y == collision.transform.position.y && x.transform.position.x == collision.transform.position.x));
            if (Mathf.Abs(collision.transform.position.x - transform.position.x) == Mathf.Abs(collision.transform.position.y - transform.position.y))
                vector = new Vector3(-vector.x, -vector.y);
            else if (Mathf.Abs(collision.transform.position.x - transform.position.x) > Mathf.Abs(collision.transform.position.y - transform.position.y))
                vector = new Vector3(-vector.x, vector.y);
            else if (Mathf.Abs(collision.transform.position.x - transform.position.x) < Mathf.Abs(collision.transform.position.y - transform.position.y))
                vector = new Vector3(vector.x, -vector.y);
            if (collision.name == "ThreeBalls(Clone)")
            {
                for (int i = -1; i <= 1; i += 2)
                {
                    float Angle = Mathf.PI / 6;
                    GameObject temp = this.gameObject;
                    float x = 0;
                    float y = 0;
                    if (Mathf.Abs(collision.transform.position.x - transform.position.x) > Mathf.Abs(collision.transform.position.y - transform.position.y))
                    {
                        y = Mathf.Cos(Mathf.Acos(vector.y) + Angle * i);
                        x = Mathf.Sqrt(1 - Mathf.Pow(y, 2));
                    }
                    else
                    {
                        x = Mathf.Cos(Mathf.Acos(vector.x) + Angle * i);
                        y = Mathf.Sqrt(1 - Mathf.Pow(x, 2));
                    }
                    GameObject temp1 = Instantiate(temp) as GameObject;
                    temp1.GetComponent<Behavior>().vector = new Vector3(x, y);
                    forTakingGlobalScript.GetComponent<GlobalScript>().balls.Add(temp1);
                }
            }
            else if(collision.name == "decressPlatform(Clone)")
            {
                platform.transform.localScale = new Vector3(platform.transform.localScale.x / 2, platform.transform.localScale.y);
                Invoke("platformX2", 10);
            }
            else if (collision.name == "incressPlatform(Clone)")
            {
                platform.transform.localScale = new Vector3(platform.transform.localScale.x * 2, platform.transform.localScale.y);
                Invoke("platformDel2", 10);
            }
            else if (collision.name == "Big(Clone)")
            {
                foreach (var ball in forTakingGlobalScript.GetComponent<GlobalScript>().balls)
                {
                    ball.transform.localScale = new Vector3(ball.transform.localScale.x * 1.5f, ball.transform.localScale.y * 1.5f);
                }
                Invoke("ballDel2", 10);
            }
            else if (collision.name == "Small(Clone)")
            {
                foreach (var ball in forTakingGlobalScript.GetComponent<GlobalScript>().balls)
                {
                    ball.transform.localScale = new Vector3(ball.transform.localScale.x / 1.5f, ball.transform.localScale.y / 1.5f);
                }
                Invoke("ballX2", 10);
            }
            else if (collision.name == "Fast(Clone)")
            {
                foreach (var ball in forTakingGlobalScript.GetComponent<GlobalScript>().balls)
                {
                    ball.GetComponent<Behavior>().speed *= 2;
                }
                Invoke("speedDel2", 10);
            }
            else if (collision.name == "Slow(Clone)")
            {
                foreach (var ball in forTakingGlobalScript.GetComponent<GlobalScript>().balls)
                {
                    ball.GetComponent<Behavior>().speed /= 2;
                }
                Invoke("speedX2", 10);
            }
            else if (collision.name == "Destroy(Clone)")
            {
                Destroy(gameObject);
                Destroy(this);
                forTakingGlobalScript.GetComponent<GlobalScript>().balls.Remove(forTakingGlobalScript.GetComponent<GlobalScript>().balls.Find(x => x == gameObject));
                forTakingGlobalScript.GetComponent<GlobalScript>().score -= 2;
                forTakingGlobalScript.GetComponent<GlobalScript>().text.GetComponent<TextMeshProUGUI>().text = "Score: " + forTakingGlobalScript.GetComponent<GlobalScript>().score.ToString();
            }
        }
        else
        {
            if (Mathf.Abs(collision.transform.position.x - transform.position.x) == Mathf.Abs(collision.transform.position.y - transform.position.y))
                vector = new Vector3(-vector.x, -vector.y);
            else if (Mathf.Abs(collision.transform.position.x - transform.position.x) > Mathf.Abs(collision.transform.position.y - transform.position.y))
                vector = new Vector3(-vector.x, vector.y);
            else if (Mathf.Abs(collision.transform.position.x - transform.position.x) < Mathf.Abs(collision.transform.position.y - transform.position.y))
                vector = new Vector3(vector.x, -vector.y);
        }
    }
}
