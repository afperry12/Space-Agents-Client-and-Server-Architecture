using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public float health;
    public float maxHealth = 100f;
    public int itemCount = 0;
    public SkinnedMeshRenderer model;
    private Animator animator;
    public string currentAnimation;

    void Start()
    {
        //--get components--
        animator = GetComponent<Animator>();
    }

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        // health = maxHealth;
        // animator = GetComponent<Animator>();
        // rigidBody = GetComponent<Rigidbody>();
        // capsuleCollider = GetComponent<CapsuleCollider>();
        // capsuleHalfHeight = capsuleCollider.height / 2;
    }

    public void Update()
    {
        if (currentAnimation == "run")
        {
            animator.SetFloat("Speed", 8);
            animator.SetBool("inAir", false);
            animator.SetBool("isCrouched", false);
        }
        else if (currentAnimation == "walk")
        {
            animator.SetFloat("Speed", 3);
            animator.SetBool("inAir", false);
            animator.SetBool("isCrouched", false);
        }
        else if (currentAnimation == "idle")
        {
            animator.SetFloat("Speed", 0);
            animator.SetBool("inAir", false);
            animator.SetBool("isCrouched", false);
        }
        else if (currentAnimation == "jump")
        {
            animator.SetFloat("Speed", 1);
            animator.SetBool("inAir", true);
            animator.SetBool("isCrouched", false);
        }
    }

    public void SetAnimation(string currentAnimation)
    {
        // animator.SetFloat("Speed", currentAnimation);
        // animator.SetBool("inAir", false);
        // animator.SetBool("isCrouched", false);
        // animator.SetBool("inAir", false);
        // animator.SetBool("isCrouched", false);
        
        // animator.SetBool("isCrouched", true);
        
        // if (!isGrounded)
        // {
        //     animator.SetBool("inAir", true);
        // }
        //
        // if (Input.GetButtonDown("Special"))
        // {
        //     animator.SetTrigger("Special");
        // }
        
        // CheckGround();
    }

    public void SetHealth(float _health)
    {
        health = _health;

        if (health <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        model.enabled = false;
    }

    public void Respawn()
    {
        model.enabled = true;
        SetHealth(maxHealth);
    }
}
