using UnityEngine;
using System.Collections;

public class munition_behaviour : MonoBehaviour {

    public float distance_travelled = 0.0f;
    private Vector3 pos_init;

	// Use this for initialization
	void Start () {
        pos_init = transform.position;

       
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        /* Vector3 deplacement = new Vector3(0,0,1) * 300 * Time.deltaTime;
        distance_travelled += deplacement.magnitude;
         transform.Translate(deplacement);
         */
        gameObject.GetComponent<Rigidbody>().MovePosition(transform.position+ transform.forward * 600 * Time.deltaTime);
        distance_travelled = (transform.position - pos_init).magnitude;

        if (distance_travelled > 500.0f)
        {
            Destroy(this.gameObject);
        }
	}
    
        
    
}
