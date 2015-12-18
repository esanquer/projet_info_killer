using UnityEngine;
using System.Collections;

public class Agent_behaviour_with_nav_mesh : MonoBehaviour {

    public Transform target;
    NavMeshAgent agent;
    Animator animator;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        //animator.SetTrigger(Animator.StringToHash("humanoidWalk"));
        agent.SetDestination(target.position);
        agent.updatePosition = false;
    }
	
	// Update is called once per frame
	void Update () {

       
        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;
        
        // Update animation parameters
        animator.SetBool("Move", shouldMove);
        animator.SetFloat("velx", velocity.x);
        animator.SetFloat("vely", velocity.y);
        animator.SetFloat("Speed", 8.0f);
        Debug.Log(animator.GetFloat("Speed"));

        //animator.SetFloat("Speed", agent.velocity.magnitude);
        // Pull character towards agent

        if (worldDeltaPosition.magnitude > agent.radius)
            transform.position = agent.nextPosition - 0.9f * worldDeltaPosition;
        //GetComponent<LookAt>().lookAtTargetPosition = agent.steeringTarget + transform.forward;
    }
    void OnAnimatorMove()
    {
        // Update position based on animation movement using navigation surface height
        Vector3 position = animator.rootPosition;
        position.y = agent.nextPosition.y;
        transform.position = position;
    }


}
