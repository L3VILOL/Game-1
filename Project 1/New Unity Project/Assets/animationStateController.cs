using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateController : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isRunningHash;
    int isAimingHash;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isAimingHash = Animator.StringToHash("isAiming");
    }

    // Update is called once per frame
    void Update()
    {
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isAiming = animator.GetBool(isAimingHash);

        if (Input.GetKey("w"))
        {
            animator.SetBool(isWalkingHash, true);
        }

        if (!Input.GetKey("w"))
        {
            animator.SetBool(isWalkingHash, false);
        }

        if (Input.GetKey("left shift") && Input.GetKey("w"))
        {
            animator.SetBool(isRunningHash, true);
        }

        if (!Input.GetKey("left shift"))
        {
            animator.SetBool(isRunningHash, false);
        }

        if (!Input.GetKey("w") || !Input.GetKey("left shift"))
        {
            animator.SetBool(isRunningHash, false);
        }

        if (Input.GetKey("l"))
        {
            animator.SetBool(isAimingHash, true);
        }

        if (!Input.GetKey("l"))
        {
            animator.SetBool(isAimingHash, false);
        }
    }
}
