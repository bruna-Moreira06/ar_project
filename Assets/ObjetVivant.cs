using UnityEngine;

public class ObjetVivant : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask layerSol;
    public LayerMask layerVivant;

    public VivantConfiguration configuration;

    public MeshRenderer renderer;

    public Rigidbody rigidbody;

    private Vector3 _target;
    private float _targetTimer;
    private float _sautTimer;

    bool TryPickTarget(out Vector3 t)
    {
        for (int i = 0; i < 20; i++)
        {
            Vector3 p = transform.position + 
                new Vector3(Random.Range(-configuration.rayonMouvement, configuration.rayonMouvement), 2f, 
                            Random.Range(-configuration.rayonMouvement, configuration.rayonMouvement));
            if (Physics.Raycast(p, Vector3.down, out var hit, 10f, layerSol))
            {
                continue;
            }

            if (Physics.SphereCast(p, 0.5f, Vector3.down, out hit, 10f, layerVivant) == false)
            {
                continue;
            }

            t = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            return true;
        }
        t = Vector3.zero;
        return false;
    }

    private void FixedUpdate()
    {
        var to = (_target - rigidbody.position);
        to.y = 0f;
        float distance = to.magnitude;
        
        if (distance > configuration.distanceArret)
        {
            rigidbody.AddForce(to.normalized * configuration.acceleration, ForceMode.Acceleration);
            rigidbody.linearVelocity = Vector3.ClampMagnitude(rigidbody.linearVelocity, configuration.vitesseMax);
        }
        else
        {
            // Ralentir progressivement quand on est proche
            rigidbody.linearVelocity = Vector3.Lerp(rigidbody.linearVelocity, Vector3.zero, Time.fixedDeltaTime * 5f);
        }
    }

    private void Start()
    {
        float random = Random.value;
        float randomSize = Mathf.Lerp(configuration.tailleRandom.x, configuration.tailleRandom.y, random);
        transform.localScale = Vector3.one * randomSize;

        rigidbody.mass = Mathf.Lerp(configuration.masseRandom.x, configuration.masseRandom.y, random);
        renderer.sharedMaterial = configuration.materiauxRandom[Random.Range(0, configuration.materiauxRandom.Count)];
        
        _sautTimer = Random.Range(configuration.intervalSaut.x, configuration.intervalSaut.y);
    }

    private void Update()
    {
        _targetTimer -= Time.deltaTime;
        if (_targetTimer <= 0f)
        {
            if (TryPickTarget(out _target))
            {
                _targetTimer = Random.Range(configuration.tempAttente.x, configuration.tempAttente.y);
            }
            else
            {
                _targetTimer = 0.1f;
            }
        }
        
        _sautTimer -= Time.deltaTime;
        if (_sautTimer <= 0f)
        {
            float puissance = Random.Range(configuration.puissanceSaut.x, configuration.puissanceSaut.y);
            rigidbody.AddForce(Vector3.up * puissance, ForceMode.Acceleration);
            _sautTimer = Random.Range(configuration.intervalSaut.x, configuration.intervalSaut.y);
        }
    }
}
