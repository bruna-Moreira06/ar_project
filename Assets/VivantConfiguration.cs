using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObject/VivantConfiguration")]
public class VivantConfiguration : ScriptableObject
{
    [Header("Apparition")]

    public Vector2 tailleRandom;
    public Vector2 masseRandom;

    [Header("Mouvements")]
    public float rayonMouvement;

    [Header("Vitesses")]
    public float acceleration;
    public float vitesseMax;
    public float distanceArret;

    public Vector2 tempAttente;

    [Header("Saut")]
    public Vector2 intervalSaut;
    public Vector2 puissanceSaut;

    public List<Material> materiauxRandom = new();
}
