using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    PlayerStat stat;
    bool z_delay = false;
    bool x_delay = false;
    [SerializeField]
    float speed=.02f;
    float speed_org;
    public Slider hp_bar;
    public Slider stamina_bar;
    public Slider mental_bar;
    bool is_running = false;
    bool is_move = false;
    bool is_stamina_zero = false;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(stat.Name + ":" + stat.Hp + ":" + stat.Stamina + ":" + stat.Metal);
        hp_bar.maxValue = stat.Hp;
        hp_bar.value = stat.Hp;
        stamina_bar.maxValue = stat.Stamina;
        stamina_bar.value = stat.Stamina;
        mental_bar.maxValue = stat.Metal;
        mental_bar.value = stat.Metal;
        speed_org = speed;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        ChangeStamina();
    }

    private void Movement()
    {
        is_move = false;
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * speed);
            is_move = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0, 0, -1) * Time.deltaTime * speed);
            is_move = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * speed);
            is_move = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * speed);;
            is_move = true;
        }
        
        if (!is_stamina_zero)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed *= 2f;
                is_running = true;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = speed_org;
                is_running = false;
            }
        }
    }

    void ChangeStamina()
    {
        if (!is_running)
        {
            if(stat.Stamina < stamina_bar.maxValue)
            {
                stat.Stamina += Time.deltaTime;
                if (stat.Stamina >= stamina_bar.maxValue)
                {
                    stat.Stamina = stamina_bar.maxValue;
                    is_stamina_zero = false;
                }
                stamina_bar.value = stat.Stamina;
            }                        
        }
        else
        {
            if(stat.Stamina > 0f)
            {
                if (is_move)
                {
                    stat.Stamina -= Time.deltaTime;
                    if (stat.Stamina <= 0f)
                    {
                        stat.Stamina = 0f;                        
                    }
                    stamina_bar.value = stat.Stamina;
                }
            }
            else
            {
                is_stamina_zero = true;
                is_running = false;
                speed = speed / 2;
            }
        }
    }

    void ClearZDelay()
    {
        z_delay = false;
    }

    void ClearXDelay()
    {
        x_delay = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 6)
        {
            stat.Hp -= 10;
            hp_bar.value = stat.Hp;
            if(stat.Hp <= 0)
            {
                transform.GetChild(0).parent = null;
                gameObject.SetActive(false);

            }            
        }
    }
}
