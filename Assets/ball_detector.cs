using UnityEngine;
using System.Collections;

public class ball_detector : MonoBehaviour {

    public Transform my_agent;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
       // Debug.Log("rencontre objet "+ other.gameObject.tag);

        switch (other.gameObject.tag)
        {
            case "munition_tiree":
                my_agent.gameObject.GetComponent<agent_behaviour>().prendreTir();
                Destroy(other.gameObject);
                break;
        }
    }
}
