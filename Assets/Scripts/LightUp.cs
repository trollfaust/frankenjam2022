using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
enum ExpansionState
{
    Stay, Expand, Shrink
}

public class LightUp : MonoBehaviour
{

    [SerializeField]
    Transform target;

    [SerializeField]
    float openForSeconds = 10;
    [SerializeField]
    float maxSize = 40;
    [SerializeField]
    float expansionSpeed = 2;
    [SerializeField]
    float shrinkingSpeed = 2;
    [SerializeField] bool isMenu = false;
    [SerializeField] bool isGoal = false;

    [SerializeField]
    Animator animator;

    [SerializeField]
    AudioSource activationAudio;

    ExpansionState state = ExpansionState.Stay;
    float wasOpenFor = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (isMenu)
        {
            StartMenuAni();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        target.localScale = new Vector3(20, 20, 20);
    }

    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player")
            yield break;

        bool alreadyOpen = expansion != 1;

        wasOpenFor = 0;
        state = ExpansionState.Expand;
        if (!isGoal)
        {
            animator.SetBool("Active", true);

        }

        if(!alreadyOpen)
            activationAudio.Play();

        yield break;
    }
    float expansion = 0;

    // Update is called once per frame
    void Update()
    {
        wasOpenFor += Time.deltaTime;
        if(state == ExpansionState.Expand)
        {
            expansion += Time.deltaTime * expansionSpeed;
            if (expansion > maxSize)
            {
                expansion = maxSize;
                state = ExpansionState.Stay;
                if (isGoal)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
            }
        }
        if (state == ExpansionState.Shrink)
        {
            expansion -= Time.deltaTime * shrinkingSpeed;
            if (expansion < 1)
            {
                expansion = 1;
                state = ExpansionState.Stay;
            }
        }
        else
        {
            if(wasOpenFor>openForSeconds)
            {
                state = ExpansionState.Shrink;
                if (!isGoal)
                {

                    animator.SetBool("Active", false);
                }
            }
        }
        
        target.localScale = new Vector3(1,1,0)* expansion;
    }

    void StartMenuAni()
    {
        StartCoroutine(MenuAni());
    }

    IEnumerator MenuAni()
    {
        yield return new WaitForSeconds(2);
        target.localScale = new Vector3(20, 20, 20);
        wasOpenFor = 0;
        state = ExpansionState.Expand;
        animator.SetBool("Active", true);
        yield return new WaitForSeconds(10);
        StartMenuAni();
    }
}
