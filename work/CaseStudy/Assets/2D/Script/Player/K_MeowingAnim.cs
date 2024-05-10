using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_MeowingAnim : MonoBehaviour
{
    private AnimatorStateInfo stateInfo;

    // Start is called before the first frame update
    void Start()
    {
        Animator animator;
        animator = GetComponent<Animator>();
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    }

    // Update is called once per frame
    void Update()
    {
        //再生終わってたら消す
        if (!stateInfo.IsName("鳴き声"))
        {
            Destroy(gameObject);
        }
    }
}
