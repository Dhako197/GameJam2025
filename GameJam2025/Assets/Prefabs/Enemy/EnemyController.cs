using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int seed = 0;
    [SerializeField] private int distance = 6;
    [SerializeField] private AudioClip damageClip;
    private float life = 60;
    private float counter = 0;
    private float timeLimit = 0;
    private bool onCooldown = false;
    private bool needsReposition = false;
    private Vector3 destination = Vector3.zero;
    private Animator animator;
    private AudioSource audioController;
    private WinController winController;
    private GameObject respawn;

    private readonly float randomRange = 3;
    private readonly float baseBubbleTime = 5.0f;
    private readonly float cooldownTime = 5;
    private readonly float speed = 3f;

    private readonly string takeDamageAction = "takeDamage";
    private readonly string isTrappedAction = "isTrapped";
    private readonly string startGumAction = "startGum";
    private readonly string repositionAction = "reposition";

    private void Awake()
    {
        audioController = GetComponent<AudioSource>();
        audioController.loop = false;
        audioController.clip = damageClip;
    }

    void Start()
    {
        respawn = GameObject.FindGameObjectWithTag("Respawn");
        winController = GetComponentInParent<WinController>();
        animator = GetComponentInChildren<Animator>();
        UnityEngine.Random.InitState(seed);
    }

    void Update()
    {
        counter += Time.deltaTime;

        CheckResetBubbleTimer();
        CheckReposition();
        CheckRepositionChange();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Vector3 inverted = GetInvertedDestinationUnit();
            destination = inverted;
        }
    }

    void CheckResetBubbleTimer()
    {
        if (onCooldown)
        {
            if (counter < cooldownTime)
            {
                return;
            }

            onCooldown = false;
            SetBubbleTimeLimit();
        }

        if (counter > timeLimit)
        {
            animator.SetBool(startGumAction, true);
            counter = 0;
            onCooldown = true;
        }
    }


    void SetBubbleTimeLimit()
    {
        animator.SetBool(startGumAction, false);
        counter = 0;
        float randomizer = UnityEngine.Random.value;
        bool isAddition = randomizer > 0.5;
        timeLimit = baseBubbleTime + (randomRange * (randomizer) * (isAddition ? 1 : -1));
    }

    void CheckReposition()
    {
        if (!needsReposition)
        {
            return;
        }
        
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(destination.x, transform.position.y, destination.z), Time.deltaTime * speed);
        Vector3 diff = destination - transform.position;

        if (Math.Abs(diff.x) < 0.25f && Math.Abs(diff.z) < 0.25f)
        {
            if (life == 20)
            {
               destination = GetDestinationRandom(distance);
               return;
            }

            animator.SetBool(repositionAction, false);
        }
    }

    void CheckRepositionChange()
    {
        bool isRepositioning = animator.GetBool(repositionAction);
        if (isRepositioning == needsReposition)
        {
            return;
        }

        needsReposition = isRepositioning;
        destination = GetDestinationRandom(distance);
    }

    Vector3 GetDestinationRandom(float fixedDistance)
    {
        float randomizerX = UnityEngine.Random.value;
        float randomizerZ = UnityEngine.Random.value;

        float x = (randomizerX > 0.3f ? (randomizerX > 0.6f ? 1 : -1) : 0) * fixedDistance;
        float z = (randomizerZ > 0.3f ? (randomizerX > 0.6f ? 1 : -1) : 0) * fixedDistance;
        return new Vector3(x, 0, z) + transform.position;
    }

    Vector3 GetInvertedDestinationUnit()
    {
        Vector3 direction = respawn.transform.position - destination;
        float x = direction.x == 0 ? 0 : (direction.x > 0 ? 1 : -1) * distance;
        float z = direction.x == 0 ? 0 : (direction.z > 0 ? 1 : -1) * distance;
        return new Vector3(x, 0, z) + transform.position;
    }

    public void TakeDamage(float damage)
    {
        if (life <= 0)
        {
            return;
        }

        // is not last strike and reposition
        if (life > 20 && needsReposition)
        {
            return;
        }

        audioController.Play();
        string action = life - damage > 0 ? takeDamageAction : isTrappedAction;
        life -= damage;
        
        animator.SetBool(action, true);

        if (life == 0)
        {
            winController.AddTrapped();
        }
    }
}
