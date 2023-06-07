using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="Player Stats", menuName = "Scriptable Object/Player Stats", order = int.MaxValue)]
public class PlayerStat : ScriptableObject
{
    [SerializeField]
    new string name;
    [SerializeField]
    float hp;
    [SerializeField]
    float stamina;
    [SerializeField]
    float metal;

    public string Name { get => name; set => name = value; }
    public float Hp { get => hp; set => hp = value; }
    public float Stamina { get => stamina; set => stamina = value; }
    public float Metal { get => metal; set => metal = value; }
}
