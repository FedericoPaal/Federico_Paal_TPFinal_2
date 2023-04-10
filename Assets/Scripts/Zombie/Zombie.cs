using Assets.Scripts.EntityFolder;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;


public class Zombie : MonoBehaviour
{ 
    [SerializeField] private Transform character;
    
    //Speeds and Distances
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float speed;
    [SerializeField] public float pursuitDistance;

    //Variables
    [SerializeField] public bool closeEnough;
    [SerializeField] public float detectionRange;
    [SerializeField] public bool takesDamage;

    //For Particles
    [SerializeField] private GameObject bloodParticles;
    [SerializeField] private GameObject pointOfParticles;

    //Animator
    [SerializeField] private Animator anim;

    //Scripts
    [SerializeField] public ScrZombie scrZombieScript;

    //Events
    public UnityEvent OnEnemyAproaching;
    public UnityEvent OnEnemyAttacking;

    private void LookAt()
    {
        var vectorToCharacter = character.position - transform.position;
        var newRotation = Quaternion.LookRotation(vectorToCharacter);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
    }

    public void Pursuit()
    {
        var vectorToCharacter = character.position - transform.position;

        var distance = vectorToCharacter.magnitude;

        closeEnough = false;

        if (Vector3.Distance(character.position, transform.position) <= detectionRange)
        {
            closeEnough = true;
        }

        if (closeEnough)
        {
            if (distance > pursuitDistance)
            {
                transform.position += vectorToCharacter.normalized * (speed * Time.deltaTime);
            }
        }

        //Attack
        var l_distanceAndPursuitDistance = distance <= pursuitDistance;
        var z_isAttacking = l_distanceAndPursuitDistance;
        if (z_isAttacking)
        {
            anim.SetTrigger("Attack");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Instantiate(bloodParticles, pointOfParticles.transform.position, pointOfParticles.transform.rotation);
        }
        
        //Destroy(bloodParticles);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Chris>(out Chris chrisComponent))
        {
            chrisComponent.PlayerTakeDamage(10);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        scrZombieScript.health -= damageAmount;

        var isTakingDamage = scrZombieScript.health > 0;

        if (isTakingDamage)
        {
            takesDamage = true;
        }
        else
        {
            takesDamage = false;
        }
        
        if (takesDamage)
        {
            anim.SetBool("Bullet Hit", true);
        }
        else
        {
            anim.SetBool("Bullet Hit", false);
        }


        if (scrZombieScript.health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void ExecutePursuit()
    {
         LookAt();
         Pursuit();
    }

    private void Start()
    {
        scrZombieScript.health = scrZombieScript.maxHealth;
    }

    void Update()
    {
        ExecutePursuit();
        
    }

    


}
