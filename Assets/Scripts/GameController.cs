using Random = System.Random;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public TextMeshProUGUI stopGoText;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI scoreText;
    public Animator stoplightAnimator;
    public Animator playerAnimator;
    public ParticleSystem winEffect;
    public GameObject player;
    public AudioSource audioSource;
    public AudioClip winSound;
    public AudioClip loseSound;
    public SpriteRenderer cookieRenderer;
    private Random rand = new Random();
    private bool coroutineBegun = false;
    private bool stopped = true;
    private bool dead = false;
    public bool won = false;
    private bool particlesPlaying = false;
    private bool deathSoundPlaying;
    private float currentSpeed = 0;
    public float moveSpeed;
    private float startTime;
    private readonly Vector2 start = new Vector3(-8.2f, -3.06f);
    public double maxTime;
    public double minTime;
    private float timer = 0;
    
    
    void Start()
    {
        player.transform.position = start;
        Debug.Log("Game Start");
    }

    void Update()
    {
        
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
        
        if (Input.GetKeyDown(KeyCode.Space) && (dead || won))
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

        if (won && !particlesPlaying)
        {
            Instantiate(winEffect, player.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            audioSource.clip = winSound;
            audioSource.Play();
            particlesPlaying = true;
            cookieRenderer.enabled = false;
        }

        if (dead && !deathSoundPlaying)
        {
            audioSource.clip = loseSound;
            audioSource.Play();
            deathSoundPlaying = true;
        }

        player.transform.position += new Vector3(currentSpeed, 0, 0);
        if (!won && !dead)
        {
            timer += Time.deltaTime;
            scoreText.text = "Time: " + timer.ToString("0.00");
        }

        
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

