using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObject/VivantConfiguration")]
public class VivantConfiguration : ScriptableObject
{
    [Header("Apparition")]

    public Vector2 tailleRandom;
    public Vector2 masseRandom;

    [Header("Mouvements")]
    public Vector2 rayonMouvement;
    public Vector2 tempAttente;
    public float distanceArret;

    [Header("Vitesses")]
    public float acceleration;
    public Vector2 vitesseMaxRandom;

    [Header("Saut")]
    public Vector2 intervalSaut;
    public Vector2 puissanceSaut;

    [Header("Nourriture")]
    public float rayonNourriture;
    public float distanceConsommation;
    public float nourrirGrossissement;

    public List<Material> materiauxRandom = new();
}
