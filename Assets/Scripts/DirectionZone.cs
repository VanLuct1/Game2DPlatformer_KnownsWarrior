using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionZone : MonoBehaviour
{
    public List<Collider2D> directedColliders = new List<Collider2D>();
    Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        directedColliders.Add(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        directedColliders.Remove(collision);
    }
}
