using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public class StatesManager : MonoBehaviour
    {
    
    
    public ControllerStats stats;
    public ControllerStates states;
    public InputVariables inp;

    

    [System.Serializable]
    public class InputVariables
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public Vector3 moveDirection;
        public Vector3 aimPosition;
        public Vector3 rotateDirection;


    }

    [System.Serializable]
    public class ControllerStates 
    {
        public bool OnGround;
        public bool isAiming;
        public bool isRunning;
        public bool isCrouching;
        public bool isInteracting;

    }


    public Animator anim;
        public GameObject activeModel;
       [HideInInspector]
    public Rigidbody rigid;
    [HideInInspector]
    public Collider controllerCollider;

        List<Collider> ragdollColliders = new List<Collider>();
        List<Rigidbody> ragdollRigids = new List<Rigidbody>();
        public LayerMask ignoreLayers;
    public LayerMask ignoreForGround;

    public Transform mTransform;
    public CharState curState;
    public float delta;


        public void Init()
        {
        mTransform = this.transform;
            SetupAnimator();
            rigid = GetComponent<Rigidbody>();
            rigid.isKinematic = false;
            rigid.drag = 4;
        rigid.angularDrag = 999;
        rigid.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        controllerCollider = GetComponent<Collider>();

        SetupRagdoll();

        ignoreLayers = ~(1 << 9);
        ignoreForGround = ~(1 << 9 | 1 << 10);
        }

        void SetupAnimator()
        {
            if (activeModel == null)
            {
                anim = GetComponentInChildren<Animator>();
                activeModel = anim.gameObject;
            }

        anim.applyRootMotion = false;

        }

         void SetupRagdoll()
         {
        Rigidbody[] rigids = activeModel.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody r in rigids)
        {
            if (r == rigid)
            {
                continue;
            }

            Collider c = r.gameObject.GetComponent<Collider>();
            c.isTrigger = true;
            ragdollRigids.Add(r);
            ragdollColliders.Add(c);
            r.isKinematic = true;
            r.gameObject.layer = 10;

        }

         }   
    
        public void FixedTick(float d)

        
        {
        delta = d;

         switch (curState)
        {

            case CharState.normal:
                states.OnGround = OnGround();
                if (states.isAiming)
                {

                }
                else
                {
                    MovementNormal();
                    RotationNormal();
                }
                break;
            case CharState.onAir:
                rigid.drag = 0;
                states.OnGround = OnGround();
                break;
            case CharState.cover:
                break;
            case CharState.vaulting:
                break;
            default:
                break;
        }

        }
    void MovementNormal ()
    {
        if (inp.moveAmount > 0.05f)
        {
            rigid.drag = 0;

        } else
        {
            rigid.drag = 4;
        }

        float speed = stats.walkSpeed;
        if (states.isRunning)
        {
            speed = stats.runSpeed;
        }
        if (states.isCrouching)
        {
            speed = stats.crouchSpeed;
        }
        Vector3 dir = Vector3.zero;
        dir = mTransform.forward * (speed * inp.moveAmount);
        rigid.velocity = dir; 
    }

    void RotationNormal()
    {
        if (inp.moveAmount > 0.05f)
        {
            rigid.drag = 0;

        }
        else
        {
            rigid.drag = 4;
        }
        inp.rotateDirection = inp.moveDirection;
        Vector3 targetDir = inp.rotateDirection;
        targetDir.y = 0;

        if (targetDir == Vector3.zero )
        {
            targetDir = mTransform.forward;

            Quaternion lookDir = Quaternion.LookRotation(targetDir);
            Quaternion targetRot = Quaternion.Slerp(mTransform.rotation, lookDir, stats.rotateSpeed * delta);
            mTransform.rotation = targetRot;
        }
    }

    void HandleAnimationNormal()
    {
        float anim_v = inp.moveAmount;
        anim.SetFloat("vertical", anim_v, 0.15f, delta);

    }
    void MovementAiming()
    {
        float speed = stats.aimSpeed;
        Vector3 v = inp.moveDirection * speed;
        rigid.velocity = v;

    }
        public void Tick (float d)
        {
        delta = d;
         switch (curState)
        {
            case CharState.normal:
                states.OnGround = OnGround();
                HandleAnimationNormal();
                break;
            case CharState.onAir:
                states.OnGround = OnGround();
                break;
            case CharState.cover:
                break;
            case CharState.vaulting:
                break;
            default:
                break;

        }

        }
    bool OnGround()
    {
        Vector3 origin = mTransform.position;
        origin.y += 0.6f;
        Vector3 dir = -Vector3.up;
        float dis = 0.7f;
        RaycastHit hit;
        if (Physics.Raycast(origin,dir,out hit ,dis , ignoreForGround))
        {
            Vector3 tp = hit.point;
            mTransform.position = tp;
            return true;

        }

        return false;
    }

         public enum CharState
      {
        normal,onAir,cover,vaulting

      }
    }
