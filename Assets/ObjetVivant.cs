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
    private Transform _nourritureTransform;

    bool TryPickTarget(out Vector3 t)
    {
        for (int i = 0; i < 20; i++)
        {
            float mouvementX = Random.Range(configuration.rayonMouvement.x, configuration.rayonMouvement.y) * (Random.value < 0.5f ? -1f : 1f);
            float mouvementZ = Random.Range(configuration.rayonMouvement.x, configuration.rayonMouvement.y) * (Random.value < 0.5f ? -1f : 1f);
            Vector3 p = transform.position + new Vector3(mouvementX, 2f, mouvementZ);
            
            if (Physics.Raycast(p, Vector3.down, out var hit, 10f, layerSol))
            {
                continue;
            }

            if (Physics.SphereCast(p, 0.05f, Vector3.down, out var hit2, 10f, layerVivant))
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
            rigidbody.linearVelocity = Vector3.Lerp(rigidbody.linearVelocity, Vector3.zero, Time.fixedDeltaTime * 5f);
        }
    }

    bool TryFindFood(out Transform foodTransform){
        foreach (var col in Physics.OverlapSphere(transform.position, configuration.rayonNourriture))
        {
            if (!col.CompareTag("Nourriture"))
                continue;
            foodTransform = col.transform;
            return true;
        }
        foodTransform = null;
        return false;
    }

    private void Start()
    {
        _target = transform.position;
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
        _sautTimer -= Time.deltaTime;

        if (_nourritureTransform != null)
        {
            float distance = Vector3.Distance(transform.position, _nourritureTransform.position);
            
            if (distance <= configuration.distanceConsommation)
            {
                Destroy(_nourritureTransform.parent.gameObject);
                _nourritureTransform = null;
                _targetTimer = 0f;
            }
            else
            {
                _target = _nourritureTransform.position;
                return;
            }
        }
        
        if (TryFindFood(out _nourritureTransform))
        {
            _target = _nourritureTransform.position;
            return;
        }
        
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
        
        if (_sautTimer <= 0f)
        {
            float puissance = Random.Range(configuration.puissanceSaut.x, configuration.puissanceSaut.y);
            rigidbody.AddForce(Vector3.up * puissance, ForceMode.Acceleration);
            _sautTimer = Random.Range(configuration.intervalSaut.x, configuration.intervalSaut.y);
        }
    }
}
