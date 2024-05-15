using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_Spring : MonoBehaviour
{
    [Header("ぱわー"), SerializeField]
    private float fPower = 6.0f;

    [Header("反応しやすさ(0〜1)"), SerializeField]
    private float fRange = 0.5f;

    [Header("反応タグ"), SerializeField]
    private string[] sReactObjTags = 
    {
        "Player",
        "Enemy",
        "EnemyBall"
    };

    [Header("音"), SerializeField]
    private AudioClip audioclip;

    private bool IsJumped;

    private void Start()
    {
        IsJumped = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        for(int i=0;i< sReactObjTags.Length;i++)
        {
            if(obj.tag==sReactObjTags[i])
            {
                float dir = Mathf.Abs(obj.transform.position.x - this.gameObject.transform.position.x);
                if (dir < fRange)
                {
                    obj.GetComponent<Rigidbody2D>().AddForce(new Vector2( 0.0f, fPower),ForceMode2D.Impulse);
                    IsJumped = true;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        for (int i = 0; i < sReactObjTags.Length; i++)
        {
            if (obj.tag == sReactObjTags[i])
            {
                float dir = Mathf.Abs(obj.transform.position.x - this.gameObject.transform.position.x);
                if (dir < fRange)
                {
                    obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, fPower), ForceMode2D.Impulse);
                    IsJumped = true;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(IsJumped)
        {
            AudioSource.PlayClipAtPoint(audioclip, transform.position);
        }
    }
}
