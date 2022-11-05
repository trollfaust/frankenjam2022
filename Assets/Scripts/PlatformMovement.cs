using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [SerializeField] GameObject path;
    [SerializeField][Tooltip("If false will move the Path backwards, if true will make a Loop from last point to first")] bool isLoop;
    [SerializeField] GameObject platform;
    [SerializeField] float speed;

    private List<Transform> pathTransforms = new List<Transform>();
    private int index;
    private bool isReturning;

    private void Awake()
    {
        foreach (Transform pathPoint in path.GetComponentsInChildren<Transform>())
        {
            pathTransforms.Add(pathPoint);
        }

        platform.transform.position = pathTransforms[0].position;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(platform.transform.position, pathTransforms[index].position) < 0.1f)
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

        Vector2 dir = pathTransforms[index].position - platform.transform.position;

        platform.transform.Translate(dir.normalized * speed / 100);
    }

    private void OnDrawGizmos()
    {
        if (path == null)
            return;
        pathTransforms = new List<Transform>();
        foreach (Transform pathPoint in path.GetComponentsInChildren<Transform>())
        {
            pathTransforms.Add(pathPoint);
        }

        for (int i = 0; i < pathTransforms.Count; i++)
        {
            if (i != pathTransforms.Count - 1)
            {
                Gizmos.DrawLine(pathTransforms[i].position, pathTransforms[i + 1].position);
            } else if (isLoop)
            {
                Gizmos.DrawLine(pathTransforms[i].position, pathTransforms[0].position);
            }
        }
    }
}
