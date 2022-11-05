using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightUp : MonoBehaviour
{
    enum ExpansionState
    {
        Stay, Expand, Shrink
    }
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

    ExpansionState state = ExpansionState.Stay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        target.localScale = new Vector3(20, 20, 20);
    }

    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player")
            yield break;

        state = ExpansionState.Expand;

        yield return new WaitForSeconds(openForSeconds);

        state = ExpansionState.Shrink;

        yield break;
    }
    float expansion = 0;

    // Update is called once per frame
    void Update()
    {
        if(state == ExpansionState.Expand)
        {
            expansion += Time.deltaTime * expansionSpeed;
            if (target.localScale.magnitude > maxSize)
                state = ExpansionState.Stay;
        }
        if (state == ExpansionState.Shrink)
        {
            expansion -= Time.deltaTime * shrinkingSpeed;
            if (expansion < 1)
                state = ExpansionState.Stay;
        }
        
        target.localScale = new Vector3(1,1,0)* expansion;
    }
}
