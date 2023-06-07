using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyControl : MonoBehaviour
{
    GameObject target_obj;
    bool inside = false;
    [SerializeField]
    float distance;
    public int magazine = 0;
    bool shot_wait = false;
    Vector3 now_distance;
    public int remain_bullet = 20;
    // Start is called before the first frame update
    void Start()
    {
        magazine = transform.GetChild(0).childCount;
        remain_bullet = magazine;
       // Debug.Log(transform.forward);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(remain_bullet);
        if (inside)
        {
            
            CherckDistance();
        }
        //Debug.Log((transform.position - enermy.transform.position));   
    }
    bool CherckDistance()
    {
        now_distance = (transform.position - target_obj.transform.position);
        Debug.Log(magazine);
        if(now_distance.magnitude < distance)
        {           
            Debug.Log("detect");
            //transform.LookAt(enermy.transform);
            if (remain_bullet == magazine && transform.GetChild(0).childCount == magazine)
            {
                for (int i = 0; i < remain_bullet; i++)
                {
                    Invoke("Fire", .2f * i);

                }
                remain_bullet = 0;
                if (!shot_wait)
                {
                    Invoke("Reloading", 10f);
                    shot_wait = true;
                }
            }
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        return true;
    }   

    void Fire()
    {

        if (transform.GetChild(0).childCount > 0)
        {
            transform.GetChild(0).GetChild(0).gameObject.GetComponent<BulletControl>().enermy = target_obj.transform.position;
            transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            
        }

    }

    private void CanShot()
    {
        if(remain_bullet == 0)
        {
            Invoke("Reloading", 1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            target_obj = other.gameObject;
            inside = true;            
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            inside = false;
        }
    }

    private void WaitTime()
    {
        shot_wait = false;
    }
    private void Reloading()
    {
        inside = true;
        remain_bullet = magazine;
        shot_wait = false;
    }
}