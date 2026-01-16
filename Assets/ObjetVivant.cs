using UnityEngine;

public class ObjetVivant : MonoBehaviour
{
    public VivantConfiguration configuration;

    public MeshRenderer renderer;

    public Rigidbody rigidbody;

    private void Start()
    {
        float random = Random.value;
        float randomSize = Mathf.Lerp(configuration.tailleRandom.x, configuration.tailleRandom.y, random);
        transform.localScale = Vector3.one * randomSize;

        rigidbody.mass = Mathf.Lerp(configuration.masseRandom.x, configuration.masseRandom.y, random);
        renderer.sharedMaterial = configuration.materiauxRandom[Random.Range(0, configuration.materiauxRandom.Count)];
    }
}
