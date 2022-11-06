using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    Transform player;

    [SerializeField]
    float Speed = 1;

    [SerializeField]
    float TrackingDistance = 15;

    [SerializeField]
    float WakeUpDistance = 5;

    [SerializeField]
    float maxSize = 5;

    [SerializeField]
    float expansionSpeed = 2;

    [SerializeField]
    float shrinkingSpeed = 2;

    [SerializeField]
    float livesFor = 10;

    [SerializeField]
    Transform target;

    [SerializeField]
    bool returnToSpawn;

    bool wake = false;
    Vector3 spawnPosition;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = transform.position;
    }

    void MoveTo(Vector3 position)
    {
        Vector3 direction = position - transform.position;
        var movement = direction.normalized * Time.deltaTime * Speed;
        if (movement.magnitude > direction.magnitude)
            movement = direction;
        transform.position += movement;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 toPlayer = player.position - transform.position;

        if (toPlayer.magnitude < WakeUpDistance)
        { 
            wake = true;
            state = ExpansionState.Expand;
        }
        if (toPlayer.magnitude< TrackingDistance && wake)
        {
            MoveTo(player.position);
        }
        else
        {
            state = ExpansionState.Shrink;
            wake = false;
        }
        if(!wake)
        {
            if(returnToSpawn)
                MoveTo(spawnPosition);
        }
        CalcualteExpansion();
    }

    ExpansionState state;
    float expansion = 0;
    // Update is called once per frame
    void CalcualteExpansion()
    {
        if (state == ExpansionState.Expand)
        {
            expansion += Time.deltaTime * expansionSpeed;
            if (expansion > maxSize)
            {
                expansion = maxSize;
                state = ExpansionState.Stay;
            }
        }
        if (state == ExpansionState.Shrink)
        {
            expansion -= Time.deltaTime * shrinkingSpeed;
            if (expansion < 0)
            {
                expansion = 0;
                state = ExpansionState.Stay;
            }
        }

        target.localScale = new Vector3(1, 1, 1) * expansion;
    }
}
