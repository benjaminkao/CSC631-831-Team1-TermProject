using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public abstract class TowerTargeting: NetworkBehaviour, ITargetable
{
    public TowerFX towerFx;
    public TowerAudio towerAudio;
    [Header("Tower Spawning")]
    [Tooltip("All of the mesh renderers of the tower.")]
    [SerializeField] private List<Renderer> renderers;
    [SerializeField] private float craftTime;
    [SerializeField] private Material towerMaterial;
    [SerializeField] private Material spawnProgressMaterial;
   



    protected float _targetingRadius;
    protected float _cooldownTime;
    private bool _canShoot = true;


    public GameObject TargetPosition
    {
        get { return this._targetPosition; }
    }

    [Tooltip("The position that enemies will use to reference how far away they are from this tower.")]
    [SerializeField] private GameObject _targetPosition;


    [SerializeField] private Health health;



    public ContainmentPlayer Owner
    {
        get
        {
            return this._playerOwner;
        }

        set
        {
            this._playerOwner = value;
        }
    }

    public TowerSpawnerInteractable TowerSpawner
    {
        get
        {
            return this._towerSpawner;
        }

        set
        {
            this._towerSpawner = value;
        }
    }


    [SerializeField] private ContainmentPlayer _playerOwner;
    [SerializeField] private TowerSpawnerInteractable _towerSpawner;

    // TowerTargeting sub-classes will implement the reaction to collisions
    // for players, enemies, or both.
    public abstract void ApplyEffects(Collider[] collisions);


    private void OnEnable()
    {
        foreach(Renderer renderer in renderers)
        {

            StartCoroutine(SpawnRoutine(renderer));
        }

        this.RegisterTargetable();
    }

    private void OnDisable()
    {
        this.DeregisterTargetable();
    }



    // Update function for TowerTargeting sub-classes
    public void SearchForTargets()
    {
        if(!isServer)
        {
            return;
        }

        if (_canShoot)
        {
            ApplyEffects(GetEntitiesInRange());
            _canShoot = false;
            StartCoroutine(ShotCooldown());
        }
    }

    private Collider[] GetEntitiesInRange()
    {
        // possible optimization: add all entities that can be affected by towers
        // into a separate layer, that way Physics.OverlapSphere can ignore irrelevant
        // collisions. 
        return Physics.OverlapSphere(transform.position, _targetingRadius /* , layerMask*/);
    }

    private IEnumerator ShotCooldown()
    {
        yield return new WaitForSecondsRealtime(_cooldownTime);
        _canShoot = true;
    }

    public void Damage(float damage)
    {
        health.alterHealth(-damage);

        RpcUpdateTowerHealth(health.HealthValue);

        if(health.Died)
        {
            // Make sure that the tower spawner that this tower was on can be interacted with again.
            this._towerSpawner.CanInteract = true;
            Destroy(gameObject);
        }
    }

    public GameObject GetTargetPosition()
    {
        return this._targetPosition;
    }

    public IEnumerator SpawnRoutine(Renderer renderer)
    {
        towerAudio.PlaySpawnAudio();

        this._canShoot = false;

        renderer.material = spawnProgressMaterial;
        renderer.material.SetFloat("Progress", 0f);

        for (float progress = 0; progress < craftTime; progress += 0.1f)
        {
            renderer.material.SetFloat("Progress", progress / craftTime);
            yield return new WaitForSeconds(0.1f);
        }

        renderer.material.SetFloat("Progress", 1.1f);

        renderer.material = towerMaterial;

        this._canShoot = true;
    }


    #region Mirror Remote Actions

    [ClientRpc]
    void RpcUpdateTowerHealth(float healthValue)
    {
        this.health.SetHealth(healthValue);
    }


    #endregion Mirror Remote Actions
}