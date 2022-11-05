using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [SerializeField] GameObject path;
    [SerializeField][Tooltip("If false will move the Path backwards, if true will make a Loop from last point to first")] bool isLoop;
    [SerializeField] GameObject platform;
    [SerializeField] float speed;

    private Rigidbody2D rb;
    private List<Transform> pathTransforms = new List<Transform>();
    private int index;
    private bool isReturning;

    private void Awake()
    {
        rb = platform.GetComponent<Rigidbody2D>();
        foreach (Transform pathPoint in path.GetComponentsInChildren<Transform>())
        {
            pathTransforms.Add(pathPoint);
        }

        rb.position = pathTransforms[0].position;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(rb.position, pathTransforms[index].position) < 0.1f)
        {
            if (!isReturning)
            {
                index++;
            } else
            {
                index--;

                if (index < 0)
                {
                    index = 0;
                    isReturning = false;
                }
            }

            if (index >= pathTransforms.Count)
            {
                index = 0;
                if (!isLoop)
                {
                    index = pathTransforms.Count - 1;
                    isReturning = true;
                }
            }
        }

        Vector2 dir = (Vector2)pathTransforms[index].position - rb.position;

        rb.MovePosition(rb.position + (dir.normalized * speed / 100));
    }

}
