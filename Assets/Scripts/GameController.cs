using System;
using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public TextMeshProUGUI stopGoText;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI scoreText;
    public Animator animator;
    public GameObject player;
    private int presses = 0;
    private bool coroutineBegun = false;
    private bool stopped = true;
    private bool dead = false;
    public bool won = false;
    public float moveSpeed = 0;
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

        scoreText.text = "Button Presses: " + presses.ToString();
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

        if (Input.GetKey(KeyCode.Space) && !dead && !won)
        {
            if (stopped)
            {
                stopGoText.text = "CONGRATS";
                statusText.text = "YOU ARE DEAD";
                dead = true;
                moveSpeed = 0;
            }
            if(!stopped)
            {
                moveSpeed = 0.05f;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && !dead)
        {
            moveSpeed = 0;
        }

        player.transform.position += new Vector3(moveSpeed, 0, 0);
    }

    private IEnumerator WaitAndChangeText(double time, string newText)
    {
        if (!won && !dead)
        {
            if (newText.Equals("STOP"))
            {
                yield return new WaitForSeconds((float) (time - time/5));
                animator.SetInteger("state",2);
                stopGoText.text = "SLOW";
                yield return new WaitForSeconds((float) time/5);
                stopGoText.text = newText;
                animator.SetInteger("state",3);
                coroutineBegun = false;
            }
            else
            {
                yield return new WaitForSeconds((float) time);
                animator.SetInteger("state",1);
                stopGoText.text = newText;
                coroutineBegun = false;  
            }

            
        }
    }
}

