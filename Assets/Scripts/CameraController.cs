using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float moveSpeed = 1.5f;
    private Vector3 pos;

    private void Awake()
    {
        if (!player)
        {
            player = FindFirstObjectByType<Player>().transform;
        }
    }
    private void Update()
    {
        pos = player.position;
        pos.z = -10f;

        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * moveSpeed);
    }
}