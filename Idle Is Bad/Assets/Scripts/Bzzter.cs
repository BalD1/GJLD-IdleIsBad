using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bzzter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.Death();
        }

    }
}
