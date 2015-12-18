using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RandomObjectsCreator : MonoBehaviour
{
    private List<Transform> created = new List<Transform>();
    public Transform _munition;
    public Transform _capsuleVie;
    public float _radius;
    private float delay = 1f;
    private float delay2 = 1f;
    private float timeCurrent = 0.0f;
    private float timeCurrent2 = 0.0f;

    bool run = false;
    public void demarrer()
    {
        run = true;
    }
    // Use this for initialization
    void Start()
    {
        
    }
    public void arreter()
    {
        run = false;
        foreach(Transform t in created)
        {
           // Destroy(t.gameObject);
        }
        created = new List<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (run)
        {

            if (timeCurrent + delay < Time.fixedTime)//(Input.GetKey("space"))
            {
                //Create a random position.
                //The insideUnitSphere return a random position inside a sphere of radius 1.
                Quaternion rndRotation = Random.rotation;
                Vector3 randomDirection = Random.insideUnitSphere * _radius;
                randomDirection += transform.position;
                NavMeshHit hit;
                while (!NavMesh.SamplePosition(randomDirection, out hit, _radius, 1)) { }
                Vector3 finalPosition = hit.position;
                finalPosition.y = 0.5f;
                //Create a random rotation.

                //Instantiate a new object at a random position with a random rotation.
                Transform newGameObj = Instantiate(_capsuleVie, finalPosition, rndRotation) as Transform;
                created.Add(newGameObj);
                timeCurrent = Time.fixedTime;
            }

            else if (timeCurrent2 + delay2 < Time.fixedTime)
            {
                //Create a random position.
                //The insideUnitSphere return a random position inside a sphere of radius 1.
                //Create a random rotation.
                Quaternion rndRotation = Random.rotation;
                Vector3 randomDirection = Random.insideUnitSphere * _radius;
                randomDirection += transform.position;
                NavMeshHit hit;
                while (!NavMesh.SamplePosition(randomDirection, out hit, _radius, 1)) { }
                Vector3 finalPosition = hit.position;
                finalPosition.y = 0.5f;
                //Instantiate a new object at a random position with a random rotation.
                Transform newGameObj = Instantiate(_munition, finalPosition, rndRotation) as Transform;
                created.Add(newGameObj);
                timeCurrent2 = Time.fixedTime;
            }
        }
    }
}
