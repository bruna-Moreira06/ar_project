using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObject/VivantConfiguration")]
public class VivantConfiguration : ScriptableObject
{
    [Header("Apparition")]

    public Vector2 tailleRandom;
    public Vector2 masseRandom;

    public List<Material> materiauxRandom = new();
}
