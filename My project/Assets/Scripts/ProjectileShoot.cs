using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ProjectileShoot : MonoBehaviour
{
    // references this video: https://www.youtube.com/watch?v=EwiUomzehKU

    [Header("References")]
    public Transform projectileSpawnPoint;
    public GameObject projectilePrefab;
    public ProjectileEnemyAi enemy;
    public Transform player;

    public float projectileSpeed;
    public float firingSpeed;

    public bool repeatable;

    public Coroutine shootProjectilesCoroutine;

    // Start is called before the first frame update
    void Awake()
    {
        // grab variable references
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        LookAtPlayer();

        if (enemy.currentState == ProjectileEnemyAi.EnemyStates.Shoot) 
        {
            if (shootProjectilesCoroutine == null) 
            {
                // start shoot coroutine
                shootProjectilesCoroutine = StartCoroutine(ShootProjectileCoroutine());
                repeatable = true;
            }
        }
        else 
        {
            if (shootProjectilesCoroutine != null)
            {
                // end coroutine
                repeatable = false;

                StopCoroutine(shootProjectilesCoroutine);
                shootProjectilesCoroutine = null;
            }
        }

    }

    IEnumerator ShootProjectileCoroutine()
    {
        while (repeatable)
        {
            ShootProjectile();

            yield return new WaitForSeconds(firingSpeed);
        }
    }

    public void ShootProjectile()
    {
        var projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        projectile.GetComponent<Rigidbody>().velocity = projectileSpawnPoint.forward * projectileSpeed;
    }

    // aim projectiles towards player
    private void LookAtPlayer()
    {
        // defines lookPos
        Vector3 lookPos = player.position - projectileSpawnPoint.position;
        // converts lookPos (direction) into a rotation with quaternion

        Quaternion rotation = Quaternion.LookRotation(lookPos);
        // interpolates between current rotation and the player's position
        projectileSpawnPoint.rotation = Quaternion.Slerp(projectileSpawnPoint.rotation, rotation, 0.2f);

    }
}
