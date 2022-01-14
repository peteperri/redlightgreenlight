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
    //public TextMeshProUGUI scoreText;
    public GameObject player;
    //private int presses = 0;
    private bool coroutineBegun = false;
    private bool stopped = true;
    private bool dead = false;
    public bool won = false;
    public float moveSpeed = 0;
    private float startTime;
    private readonly Vector2 start = new Vector3(-10.93f, -4);
    public double maxTime = 10.0;
    public double minTime = 10.0;
    Random rand = new Random();
    
    void Start()
    {
        player.transform.position = start;
        Debug.Log("Game Start");
    }

    void Update()
    {
       
        double time = rand.NextDouble() * (maxTime - minTime) + minTime;
        if (!coroutineBegun)
        {
            coroutineBegun = true;
            if (!stopped)
            {
                StartCoroutine(WaitAndChangeText(time, "GO"));
                stopped = true;
            }
            else
            {
                StartCoroutine(WaitAndChangeText(time, "STOP"));
                stopped = false;
            }
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
                moveSpeed = 0.1f;
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
            yield return new WaitForSeconds((float) time);
            stopGoText.text = newText;
            coroutineBegun = false;
        }
    }
}

