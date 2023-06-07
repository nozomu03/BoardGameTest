using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

class Coordinate
{

    public Coordinate(int _x, int _y)
    {
        this.x = _x;
        this.y = _y;
    }

    public int x { get; set; }
    public int y { get; set; }
}
public class GenerateMap : MonoBehaviour
{
    [SerializeField]
    float alive_percent = .45f;
    [SerializeField]
    int map_x = 10;
    [SerializeField]
    int map_y = 10;
    [SerializeField]
    int death_limit = 10;
    [SerializeField]
    int birth_limit = 10;
    [SerializeField]
    int loop_count = 5;
    [SerializeField]
    int outline_x=4;
    [SerializeField]
    int outline_z=6;
    [SerializeField]
    float floor_height_chance = .2f;
    [SerializeField]
    GameObject[] object_list;
    [SerializeField]
    float object_chance = .3f;
    int loop_count_org = 0;
    bool[,] now_map;
    public GameObject tile;
    public GameObject tile2;
    public GameObject real_map;
    int[,] object_map;
    List<Coordinate> correct_list;
    List<Coordinate> temp_list;
    Queue<Coordinate> route_list;
    // Start is called before the first frame update
    void Start()
    {
        GameObject tmp;
        GameObject tmp2;
        correct_list = new List<Coordinate>();
        loop_count_org = loop_count;
        now_map = new bool[map_y, map_x];
        //PrintMap(now_map);
        now_map = ResetMap(now_map);
        //PrintMap(now_map);
        //now_map[2, 2] = true;
        //now_map[2, 3] = true;
        //now_map[3, 2] = true;
        //now_map[3, 3] = true;
        //PrintMap(now_map);

        now_map = CalculateLife(now_map);
        CalcObject(now_map);

        PrintMap(now_map);
        //if (!calculate_delay)
        //{
        //    now_map = CalculateLife(now_map);
        //}                
    }

    public bool[,] ResetMap(bool[,] map)
    {
        GameObject cell;
        float now_chance = 0f;
        for(int i =0; i< map_y; i++)
        {
            for(int j = 0; j < map_x; j++)
            {               
                now_chance = Random.Range(0f, 1f);
                //Debug.Log(now_chance);
                if(now_chance <= alive_percent)
                {
                    map[i, j] = true;
                    //cell = Instantiate(tile);
                    //cell.transform.Translate(new Vector3(i, j, 0));
                    //cell.transform.parent = transform;
                }
                else
                {
                    map[i, j] = false;
                    //cell = Instantiate(tile2);
                    //cell.transform.Translate(new Vector3(i, j, 0));
                    //cell.transform.parent = transform;
                }
                
            }
        }
        return map;
    }

    private void CalcObject(bool[,] map)
    {
        float chance_val = 0f;
        int object_index = 0;
        int object_x = 0;
        int object_y = 0;
        object_map = new int[map_y, map_x];
        for(int i = 0; i < map_y; i++)
        {
            for(int j = 2; j < map_x-1; j++)
            {
                chance_val = Random.Range(0f, 1f);
                if (chance_val <= object_chance)
                {
                    object_index = Random.Range(0, object_list.Length);
                    object_x = (int)object_list[object_index].transform.localScale.x - 1;
                    object_y = (int)object_list[object_index].transform.localScale.z - 1;
                    if (j + object_x < map_x-2 && i + object_y < map_y-2)
                    {
                        if (object_map[i, j + object_x] == -1 || object_map[i + object_y, j] == -1 || object_map[i + object_y, j + object_x] == -1)
                        {
                            if (object_map[i, j + object_x] == 0 || object_map[i + object_y, j] == 0 || object_map[i + object_y, j + object_x] == 0)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            object_map[i, j] = object_index + 1;
                            if (object_x > 0 || object_y > 0)
                            {
                                object_map[i, j + object_x] = -1;
                                object_map[i + object_y, j] = -1;
                                object_map[i + object_y, j + object_x] = -1;
                            }
                        }
                    }
                }
            }
        }
        //object_map[3, 2] = 1;
        //object_map[6, 2] = 1;
        Debug.Log(object_map);
    }

    public void PrintMap(bool[,] map)
    {
        GameObject tmp;
        GameObject tmp2;
        GameObject tmp3;
        float now_chance = 0;
        tmp = Instantiate(real_map);
        tmp.transform.position = new Vector3(0, 0, 0);
        for (int i = 0; i < map_y; i++)
        {
            for (int j = 0; j < map_x; j++)
            {
                if (j - 1 > 0 && j + 1 < map_x)
                {
                    if (!now_map[i, j])
                    {
                        now_map[i, j - 1] = false;
                        //now_map[i, j + 1] = true;
                    }
                }
            }
        }

        for (int i = 0; i < map_y; i++)
        {
            for (int j = 0; j < map_x; j++)
            {
                if (now_map[i, j])
                {
                    tmp2 = Instantiate(tile);
                    tmp2.transform.parent = tmp.transform;
                    tmp2.transform.position = new Vector3(j, 0, i);
                }
                else
                {                    
                    tmp2 = Instantiate(tile2);
                    tmp2.transform.parent = tmp.transform;
                    if (j <= 1 || j >= map_x - 2)
                    {
                        tmp2.transform.localScale = new Vector3(1, outline_z, 1);
                        tmp2.transform.position = new Vector3(j, (outline_z - 1) * 0.5f, i);
                    }
                    else
                    {
                        now_chance = Random.Range(0f, 1f);
                        if (floor_height_chance >= now_chance && object_map[i,j] == 0)
                        {
                            now_chance = Random.Range(1f, 1.5f);
                            tmp2.transform.localScale = new Vector3(1, now_chance, 1);
                            tmp2.transform.position = new Vector3(j, (now_chance - 1) * 0.5f, i);
                        }
                        else
                        {
                            tmp2.transform.localScale = new Vector3(1, 1, 1);
                            tmp2.transform.position = new Vector3(j, 0, i);
                        }
                    }
                }
                if(object_map[i,j] > 0)
                {
                    tmp3 = Instantiate(object_list[object_map[i, j] - 1]);
                    tmp3.transform.parent = tmp.transform;
                    tmp3.transform.position = new Vector3(j + tmp3.transform.localPosition.x, tmp3.transform.localPosition.y, i + tmp3.transform.localPosition.z);
                }

            }
        }
        tmp2 = Instantiate(tile2);
        tmp2.transform.localScale = new Vector3(outline_x, outline_z, map_y - 1);
        tmp2.transform.position = new Vector3(outline_x / -2 - .5f, (outline_z - 1) * 0.5f, (map_y / 2 -1));
        tmp2.transform.parent = tmp.transform;
        tmp2 = Instantiate(tile2);
        tmp2.transform.localScale = new Vector3(outline_x, outline_z, map_y - 1);
        tmp2.transform.position = new Vector3(map_x + (outline_x - 1) * 0.5f, (outline_z - 1) * 0.5f, map_y/2 - 1);
        tmp2.transform.parent = tmp.transform;
        gameObject.SetActive(false);
//10 4.5 4:1
//24.5   21.5
    }

    private bool[,] CalculateLife(bool[,] map) 
    {
        bool[,] temp_map = new bool[map_y, map_x];
        int result_neighbor = 0;
        for(int i = 0; i<map_y; i++)
        {
            for(int j = 2; j<map_x-2; j++)
            {
                result_neighbor = GetNeighbor(j,i, map);
                if(map[i,j] == true)
                {
                    if(result_neighbor < death_limit)
                    {
                        temp_map[i, j] = false;
                    }
                    else
                    {
                        temp_map[i, j] = true;
                    }
                }
                else
                {
                    if(result_neighbor > birth_limit)
                    {
                        temp_map[i, j] = true;
                    }
                    else
                    {
                        temp_map[i, j] = false;
                    }
                }
                //Debug.Log(i + ":" + j + ": " + map[i, j]);
            }
        }
        loop_count--;
        if (loop_count > 0)
        {
            now_map = temp_map;
            now_map = CalculateLife(now_map);
        }
        //else
        //{
        //    //PrintMap(now_map);
        //    now_map = CheckRightMap(now_map);

        //}
        return temp_map;
    }

    private int GetNeighbor(int x, int y, bool[,] map)
    {
        int alive_cell = 0;
        for (int i = -1; i <= 1; i++) {
            for(int j = -1; j <=1; j++)
                if (x + j == -1 || x+j >= map_x || y + i == -1 || y+i >= map_y)
                {
                    continue;
                }
                else if(j==0 && i == 0)
                {
                    continue;
                }
                else
                {
                    if (map[y+i, x + j] == true)
                    {
                        alive_cell++;
                    }
                }
        }
        //Debug.Log(x + ":" + y + ": " + alive_cell);
        return alive_cell;
    }

    private bool[,] CheckRightMap_inner(bool[,] map, int x, int y)
    {
        temp_list = new List<Coordinate>();
        Coordinate coord;
        route_list.Enqueue(new Coordinate(x, y));
        while(route_list.Count > 0)
        {
            coord = route_list.Dequeue();
            Debug.Log("Now route_list: " + route_list.Count);
            Debug.Log("Now coord: " + coord.x + ":" + coord.y);
            if (map[coord.y, coord.x] == false)
                continue;
            if (coord.x + 1 < map_x) {
                if (map[coord.y, coord.x + 1])
                {
                    //  [coord.y, coord.x + 1] = false;
                    route_list.Enqueue(new Coordinate(coord.x + 1, coord.y));
                }
            }
            if (coord.x - 1 >= 0)
            {
                if (map[coord.y, coord.x - 1])
                {
                    //map[coord.y, coord.x - 1] = false;
                    route_list.Enqueue(new Coordinate(coord.x - 1, coord.y));
                }
            }
            if(coord.y +1 < map_y)
            {
                if(map[coord.y+1, coord.x])
                {
                   // map[coord.y + 1, coord.x] = false;
                    route_list.Enqueue(new Coordinate(coord.x, coord.y + 1));
                }
            }
            if(coord.y-1 >= 0)
            {
                if(map[coord.y-1, coord.x])
                {
                   // map[coord.y - 1, coord.x] = false;
                    route_list.Enqueue(new Coordinate(coord.x, coord.y - 1));
                }
            }
            map[coord.y, coord.x] = false;
            temp_list.Add(coord);
            Debug.Log("Check Error: " + x + ":" + y + ":" + map[coord.y, coord.x]);
        }
        Debug.Log("End Searching");
        if (correct_list.Count < temp_list.Count)
            correct_list = temp_list;
        return map;
    }

    private bool[,] CheckRightMap(bool[,] map)
    {
        route_list = new Queue<Coordinate>();
        //for(int i = 0; i <map_y; i++)
        //{
        //    for(int j = 0; j < map_x; j++)
        //    {
        //        if(map[i,j] == true)
        //        {
        //            if (!first_found)
        //            {
        //                start_x = j;
        //                start_y = i;
        //                first_found = true;                        
        //            }
        //            end_x = j;
        //            end_y = i;
        //            true_cell++;
        //        }
        //    }
        //}
        //Debug.Log("Start " + start_x + ":" + start_y);
        //Debug.Log("End " + end_x + ":" + end_y);

        for(int i = 0; i < map_y; i++)
        {
          
            for (int j = 0; j < map_x; j++)
            {
                if (map[i, j])
                {
                    map = CheckRightMap_inner(map, j, i);
                }
            }
 
        }           
        Debug.Log(correct_list.Count);
        now_map = new bool[map_y, map_x];
        bool[,] new_map = new bool[map_y, map_x];
        foreach(Coordinate coord in correct_list)
        {
            new_map[coord.y, coord.x] = true;
        }
        GameObject tmp;
        GameObject tmp2;
        tmp = Instantiate(real_map);
        tmp.transform.position = new Vector3(0, 0, 0);
        for(int i = 0; i < map_y; i++)
        {
            for(int j = 0; j < map_x; j++)
            {
                if (new_map[j, i])
                {
                    tmp2 = Instantiate(tile);
                    tmp2.transform.parent = tmp.transform;
                    tmp2.transform.position = new Vector3(j, 0, i);
                }
                else
                {
                    tmp2 = Instantiate(tile2);
                    tmp2.transform.parent = tmp.transform;
                    tmp2.transform.position = new Vector3(j, 0, i);
                }

            }
        }
        Destroy(gameObject);
        return new_map;
        //if(chunk_count > 1)
        //{
        //    //Instantiate(gameObject);
        //    //gameObject.AddComponent<GenerateMap>();
        //    //Destroy(gameObject);
        //    loop_count = loop_count_org;
        //    now_map = new bool[map_y, map_x];

        //    PrintMap(now_map);
        //    //StartCoroutine(WaitSecond());
        //}
        //for(int i = start_y; i< map_y; i++)
        //{
        //    for (int j = start_x; j < map_x; j++)
        //    {
        //        if (map[i, j])
        //        {
        //            if (j - 1 < 0 || j + 1 > map_x || i - 1 < 0 || i + 1 > map_y)
        //                continue;
        //            else
        //            {
        //                if (map[i, j - 1] == true)
        //                {
        //                    route_list.Add(new Coordinate(j - 1, i));
        //                }
        //                if (map[i - 1, j] == true)
        //                {
        //                    route_list.Add(new Coordinate(j, i-1));
        //                }
        //                if(map[i + 1, j] == true)
        //                {
        //                    route_list.Add(new Coordinate(j, i+1));
        //                }
        //                if(map[i, j + 1] == true)
        //                {
        //                    route_list.Add(new Coordinate(j + 1, i));
        //                }
        //            }
        //        }
        //    }
        //}
        
        //for(int i = 0; i< map_y; i++)
        //{
        //    for(int j = 0; j < map_x; j++)
        //    {
        //        Debug.Log(i + ":" + j);
        //        if (map[i, j])
        //        {
        //            if (j - 1 < 0 || j + 1 > map_x || i - 1 < 0 || i + 1 > map_y)
        //                continue;
        //            else
        //            {
        //                if (map[i, j - 1] == false && map[i - 1, j] == false && map[i + 1, j] == false && map[i, j + 1] == false)
        //                {
        //                    loop_count = loop_count_org;
        //                    now_map = ResetMap(map);
        //                    StartCoroutine(WaitSecond());
        //                    return false;
        //                }
        //            }
        //        }
        //    }

    }

    private void Update()
    {
        //if (start_calc && !calculate_delay)
        //{
        //    now_map = CalculateLife(now_map);
        //    PrintMap(now_map);
        //}
    }
    // Update is called once per frame

}
