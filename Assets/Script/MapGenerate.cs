using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerate : MonoBehaviour
{
    public bool[,] now_map;
    [SerializeField]
    int map_x = 10;
    [SerializeField]
    int map_y = 10;
    [SerializeField]
    float alive_chance = .5f;
    [SerializeField]
    int dead_limit = 2;
    [SerializeField]
    int alive_limit = 5;
    [SerializeField]
    int repeat_count = 5;
    [SerializeField]
    GameObject cell_org = null;
    [SerializeField]
    Material alive = null;
    [SerializeField]
    Material dead = null;
    List<Transform> child_list;
    // Start is called before the first frame update
    void Start()
    {      
        InitalizeMap();
        now_map = CalculateMap(now_map);
    }

    private bool[,] CalculateMap(bool[,] map)
    {
        bool[,] temp_map = new bool[map_y, map_x];

        for(int i=0; i<map_y; i++)
        {
            for(int j=0; j<map_x; j++)
            {
                temp_map[i, j] = GetNegiborState(map, j, i);
            }
        }
        if (repeat_count > 0)
        {
            StartCoroutine(RepeatCalc());
            repeat_count--;
        }
        else
        {
            PrintMap();
        }
        return temp_map;
    }

    private bool GetNegiborState(bool[,] map, int cell_x, int cell_y)
    {
        int alive_neighbor = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (cell_x + j < 0 || cell_x + j >= map_x || cell_y + i >= map_y || cell_y + i < 0)
                {
                    continue;
                }
                else if (i == 0 && j == 0)
                {
                    continue;
                }
                else
                {
                    //Debug.Log((cell_x + i) + ":" + (cell_y + j));
                    if (map[cell_y + i, cell_x + j])
                    {
                        alive_neighbor += 1;
                    }
                }
            }
        }
        Debug.Log(cell_x + ":" + cell_y + ":" + alive_neighbor + ":" + alive_limit);
        if (alive_neighbor >= alive_limit)
            return true;
        if (alive_neighbor <= dead_limit)
        {
            return false;
        }
        else
            return map[cell_y, cell_x];
    }

    private void PrintMap()
    {
        if(child_list.Count > 0)
        {
            for(int i = 0; i < map_y; i++)
            {
                for(int j = 0; j < map_x; j++)
                {
                    if (now_map[i, j])
                    {
                        //if(i == 0)
                        //{
                            child_list[i * map_y + j].GetComponent<MeshRenderer>().material = alive;
                            child_list[i * map_y + j].localScale = new Vector3(1, 0, 1);
                            child_list[i * map_y + j].localPosition = new Vector3(child_list[i * map_y + j].localPosition.x, -.5f, child_list[i * map_y + j].localPosition.z);
                        //}
                        //else
                        //{
                        //    child_list[i*j].GetComponent<MeshRenderer>().material = alive;
                        //    //child_list[i * j].localScale = new Vector3(1, 1, 1);
                        //    child_list[i * j].localPosition = new Vector3(j, -0, i);

                        //}
                    }
                    else
                    {
                        //if (i == 0)
                        //{
                            child_list[i * map_y + j].GetComponent<MeshRenderer>().material = dead;
                            child_list[i * map_y + j].localScale = new Vector3(1, 2, 1);
                            child_list[i * map_y + j].localPosition = new Vector3(child_list[i * map_y + j].localPosition.x, .5f, child_list[i * map_y + j].localPosition.z);
                        //child_list[j].gameObject.SetActive(true);

                        //}
                        //else
                        //{
                        //child_list[i * j].GetComponent<MeshRenderer>().material = dead;
                        ////child_list[i * j].localScale = new Vector3(1, 1, 1);
                        //child_list[i * j].localPosition = new Vector3(j, 0, i);
                        ////child_list[i * j].gameObject.SetActive(true);

                        //}
                    }
                }
            }
        }
    }

    private void InitalizeMap()
    {
        GameObject cell;
        child_list = new List<Transform>();
        now_map = new bool[map_y, map_x];
        for(int i = 0; i < map_y; i++)
        {
            for(int j =0; j < map_x; j++)
            {
                if(Random.Range(0f, 1f) < alive_chance)
                {
                    now_map[i, j] = true;
                    cell = Instantiate(cell_org);
                    cell.transform.parent = gameObject.transform;

                    cell.transform.localPosition = new Vector3(j, -0, i);
                   // cell.transform.localScale = new Vector3(1, 1, 1);
                    cell.gameObject.GetComponent<MeshRenderer>().material = alive;                    
                }
                else
                {
                    now_map[i, j] = false;
                    cell = Instantiate(cell_org);
                    cell.transform.parent = gameObject.transform;

                    cell.transform.localPosition = new Vector3(j, 0, i);
                   // cell.transform.localScale = new Vector3(1, 1, 1);
                    cell.gameObject.GetComponent<MeshRenderer>().material = dead;
                    cell.SetActive(true);
                }
                child_list.Add(cell.transform);
            }
        }
    }

    IEnumerator RepeatCalc()
    {
        yield return new WaitForSeconds(.2f);
        now_map = CalculateMap(now_map);
        PrintMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
