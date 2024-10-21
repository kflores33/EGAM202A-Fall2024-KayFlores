using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShoot : MonoBehaviour
{
    // references this video: https://www.youtube.com/watch?v=EwiUomzehKU

    public Transform projectileSpawnPoint;
    public GameObject projectilePrefab;
    public float projectileSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShootProjectile()
    {
        var projectile = Instantiate(projectilePrefab, projectileSpawnPoint);
        projectile.GetComponent<Rigidbody>().velocity = projectileSpawnPoint.forward * projectileSpeed;
    }
}
