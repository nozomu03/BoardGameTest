using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    public Vector3 enermy;
    public float reload_time = 2f;
    public GameObject bullets;
    EnermyControl root_control;
    float angle;
    Vector3 vector;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        //Debug.Log(enermy);
        transform.LookAt(enermy);
        root_control = transform.GetComponentInParent<EnermyControl>();
    }

    // Update is called once per frame
    void Update()
    {        
        transform.parent = null;        
       // transform.position = Vector3.MoveTowards(transform.position, enermy, .1f);       
        vector = (enermy - transform.position);
        angle = vector.magnitude;
        vector = vector.normalized;
        transform.Translate(Vector3.forward * Time.deltaTime * 10f);        
       // Debug.Log(angle);
      
        Invoke("KillBullet", 4f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        CancelInvoke();
        KillBullet();

    }

    private void KillBullet()
    {
        gameObject.SetActive(false);
        transform.parent = bullets.transform;
        transform.localPosition = Vector3.zero;
    }
        
}
