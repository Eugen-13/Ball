using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;



// 1. Баланс
// 2. Звуки
// 3. Меню


public class GlobalScript : MonoBehaviour
{

    public float borderX = 0;
    public float borderY = 0;
    public GameObject pause;
    public GameObject pauseText;
    public bool isPause = false;
    public GameObject textObj;
    public TextMeshProUGUI text;
    public int score = 0;
    public bool isFirstBall = true;

    public GameObject circle;
    public List<GameObject> balls;
    public List<GameObject> squares;
    public int timeScale = 1;


    public GameObject ThreeBalls;
    public GameObject Big;
    public GameObject decressPlatform;
    public GameObject incressPlatform;
    public GameObject Fast;
    public GameObject Slow;
    public GameObject square;
    public GameObject DestroyBall;
    public GameObject Small;

    private void Start()
    {
        if (isFirstBall)
        {
            InvokeRepeating("CreateSqures", 0, 2);
           // InvokeRepeating("RemoveFirstSquare", 0, 2);
            squares = new List<GameObject>();

            balls = new List<GameObject>();
            balls.Add(circle);


            text.text = "Score: " + score.ToString();
            Button btn = pause.GetComponent<Button>();
            btn.onClick.AddListener(myPause);

            borderX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0)).x;
            borderY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)).y;
        }
    }
    private void RemoveFirstSquare()
    {
        Destroy(squares[0]);
        squares.RemoveAt(0);
    }
    private void Update()
    {
        if (isPause)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                myPause();
            }
        }
        if (balls.Count < 1)
        {
            SceneManager.LoadScene(0);
        }
    }
    public void myPause()
    {
        if (!isPause)
        {
            timeScale = 0;
            CancelInvoke();
            isPause = true;
            var components = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var item in components)
            {
                if (item.name != "Main Camera" && item.name != "Canvas" && item.name != "EventSystem")
                    item.GetComponent<Renderer>().material.color = new Color(item.GetComponent<Renderer>().material.color.r, item.GetComponent<Renderer>().material.color.g, item.GetComponent<Renderer>().material.color.b, item.GetComponent<Renderer>().material.color.a / 2);
            }
            pauseText.SetActive(true);
            textObj.SetActive(false);
            pause.SetActive(false);
        }
        else
        {
            timeScale = 1;
            InvokeRepeating("CreateSqures", 0, 1);
            isPause = false;
            var components = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var item in components)
            {
                if (item.name != "Main Camera" && item.name != "Canvas" && item.name != "EventSystem")
                    item.GetComponent<Renderer>().material.color = new Color(item.GetComponent<Renderer>().material.color.r, item.GetComponent<Renderer>().material.color.g, item.GetComponent<Renderer>().material.color.b, item.GetComponent<Renderer>().material.color.a * 2);
            }
            pauseText.SetActive(false);
            textObj.SetActive(true);
            pause.SetActive(true);
        }
    }
    private void CreateSqures()
    {
        float x = Random.Range(-borderX + 0.5f, borderX - 0.5f);
        float y = Random.Range(0.5f, borderY - 0.5f);
        GameObject temp;
        if (squares.Count > 0)
        {
            bool SuccesfulRandom = false;
            int i = 0;
            while (!SuccesfulRandom)
            {
                SuccesfulRandom = true;
                foreach (var square in squares)
                {
                    BoxCollider2D box = square.GetComponent<BoxCollider2D>();
                    if (Mathf.Abs(box.transform.position.x - x) + Mathf.Abs(box.transform.position.y - y) < (box.size.x * box.transform.localScale.x * 1.5f))
                    {
                        SuccesfulRandom = false;
                        x = Random.Range(-borderX + 0.5f, borderX - 0.5f);
                        y = Random.Range(0.5f, borderY - 0.5f);
                        break;
                    }
                }
                i++;
                if (i == 1000)
                {
                    Debug.Log("No places");
                    CancelInvoke();
                    break;
                }
            }
        }
        GameObject tempObjesct;
        int[] mass = { 1, 2, 2, 5, 6 };
        int forRandSpecSquares = mass[Random.Range(0, mass.Length)];
        switch (forRandSpecSquares)
        {
            case 1:
                tempObjesct = DestroyBall;
                break;
            case 2:
                tempObjesct = ThreeBalls;
                break;
            case 3:
                tempObjesct = decressPlatform;
                break;
            case 4:
                tempObjesct = incressPlatform;
                break;
            case 5:
                tempObjesct = Big;
                break;
            case 6:
                tempObjesct = Small;
                break;
            case 7:
                tempObjesct = Fast;
                break;
            case 8:
                tempObjesct = Slow;
                break;
            default:
                tempObjesct = square;
                break;
        }
        temp = Instantiate(tempObjesct, new Vector3(x, y), Quaternion.identity);
        if (forRandSpecSquares > 8)
            temp.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        squares.Add(temp);
    }
}
