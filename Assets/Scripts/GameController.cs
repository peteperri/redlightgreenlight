using System;
using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public TextMeshProUGUI stopGoText;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI scoreText;
    public Animator stoplightAnimator;
    public Animator playerAnimator;
    public GameObject player;
    private int presses = 0;
    private bool coroutineBegun = false;
    private bool stopped = true;
    private bool dead = false;
    public bool won = false;
    private float currentSpeed = 0;
    public float moveSpeed;
    private float startTime;
    private readonly Vector2 start = new Vector3(-8.2f, -3.36f);
    public double maxTime;
    public double minTime;
    Random rand = new Random();
    
    void Start()
    {
        player.transform.position = start;
        Debug.Log("Game Start");
    }

    void Update()
    {

        scoreText.text = "Space Presses: " + presses.ToString();
        double time = rand.NextDouble() * (maxTime - minTime) + minTime;
        if (dead || won)
        {
            stopGoText.text = "CONGRATS";
        }
        else if (!coroutineBegun)
        {
            coroutineBegun = true;
            if (!stopped && !won)
            {
                StartCoroutine(WaitAndChangeText(time, "GO"));
                stopped = true;
            }
            else if(stopped && !won)
            {
                StartCoroutine(WaitAndChangeText(time, "STOP"));
                stopped = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && !dead && !won)
        {
            presses++;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && (dead || won))
        {
            SceneManager.LoadScene("SampleScene");
        }

        if (Input.GetKey(KeyCode.Space) && !dead && !won)
        {
            playerAnimator.SetBool("isWalking", true);
            if (stopped)
            {
                stopGoText.text = "CONGRATS";
                statusText.text = "YOU ARE DEAD";
                dead = true;
                currentSpeed = 0;
                playerAnimator.SetBool("isWalking", false);
            }
            if(!stopped)
            {
                currentSpeed = moveSpeed;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && !dead)
        {
            playerAnimator.SetBool("isWalking", false);
            currentSpeed = 0;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        player.transform.position += new Vector3(currentSpeed, 0, 0);
    }

    private IEnumerator WaitAndChangeText(double time, string newText)
    {
        if (!won && !dead)
        {
            if (newText.Equals("STOP"))
            {
                yield return new WaitForSeconds((float) (time - time/5));
                stoplightAnimator.SetInteger("state",2);
                stopGoText.text = "SLOW";
                yield return new WaitForSeconds((float) time/5);
                stopGoText.text = newText;
                stoplightAnimator.SetInteger("state",3);
                coroutineBegun = false;
            }
            else
            {
                yield return new WaitForSeconds((float) time);
                stoplightAnimator.SetInteger("state",1);
                stopGoText.text = newText;
                coroutineBegun = false;  
            }

            
        }
    }
}

