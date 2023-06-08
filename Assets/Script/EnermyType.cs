using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Enermy Type", menuName = "Scriptable Object/Enermy Type", order = int.MaxValue)]
public class EnermyType : ScriptableObject
{
    [SerializeField]
    new string type;
    [SerializeField]
    int hp;
    [SerializeField]
    float atk;
    [SerializeField]
    float damaged_chance;
    [SerializeField]
    float mental;
    [SerializeField]
    float check_distance;
    public string Type { get => type; set => type = value; }
    public int Hp { get => hp; set => hp = value; }
    public float Atk { get => atk; set => atk = value; }
    public float DamagedChance { get => damaged_chance; set => damaged_chance = value; }
    public float Mental { get => mental; set => mental = value; }
    public float CheckDistance { get => check_distance; set => check_distance = value; }
}
