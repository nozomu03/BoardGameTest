using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControl : MonoBehaviour
{
    BoxCollider colider;
    BoxCollider collision_colider;

    // Start is called before the first frame update
    void Start()
    {
        colider = gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "object")
        {
            collision_colider = collision.gameObject.GetComponent<BoxCollider>();
            //collision.transform.parent.gameObject.SetActive(false);
        }
    }

}
