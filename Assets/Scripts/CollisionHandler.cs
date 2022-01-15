using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        GameController controller = other.GetComponent<Collider2D>().GetComponent<GameController>();
        controller.moveSpeed = 0;
        controller.stopGoText.text = "CONGRATS";
        controller.statusText.text = "YOU WIN";
        controller.won = true;
    }
}
