using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject lockObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            if (player.HasKey)
            {
                Destroy(lockObject);
                GameManager.Instance.StateOfGame = GameManager.GameState.Win;
            }
        }
    }
}
