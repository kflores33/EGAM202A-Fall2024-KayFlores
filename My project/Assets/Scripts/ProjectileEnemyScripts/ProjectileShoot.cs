using System.Collections;
using System.Collections.Generic;
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

    public Vector3 aimBoundsMin;
    public Vector3 aimBoundsMax;

    public bool repeatable;

    public Coroutine shootProjectilesCoroutine;

    // Start is called before the first frame update
    void Awake()
    {
        // grab variable references
        player = GameObject.FindObjectOfType<ProjectileTarget>().transform;
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
        //projectile.GetComponent<Rigidbody>().velocity = projectileSpawnPoint.forward * projectileSpeed;
    }

    // aim projectiles towards player
    private void LookAtPlayer()
    {
        // defines lookPos
        Vector3 lookPos = player.position - projectileSpawnPoint.position;

        // converts lookPos (direction) into a rotation with quaternion
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        //rotation = ClampQuaternionX(rotation, aimBoundsMin, aimBoundsMax);

        // interpolates between current rotation and the player's position
        projectileSpawnPoint.rotation = Quaternion.Slerp(projectileSpawnPoint.rotation, rotation, 0.2f);
    }


    // clamping angles: https://discussions.unity.com/t/how-do-i-clamp-a-quaternion/606650/6
    #region angle functions
    // used to clamp rotations using vectors
    public static Vector3 ClampVector(Vector3 direction, Vector3 center, float maxAngle)
    {
        float angle = Vector3.Angle(center, direction);
        if (angle > maxAngle)
        {
            direction.Normalize();
            center.Normalize();
            Vector3 rotation = (direction - center) / angle;
            return (rotation * maxAngle) + center;
        }
        return direction;
    }

    // used to clamp rotations using quaternions (mostly for future reference)
    public static Quaternion ClampQuaternion(Quaternion q, Vector3 bounds)
    {
        q.x /= q.w; q.y /= q.w; q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, -bounds.x, bounds.x);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.y);
        angleY = Mathf.Clamp(angleY, -bounds.y, bounds.y);
        q.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleY);

        float angleZ = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.z);
        angleZ = Mathf.Clamp(angleZ, -bounds.z, bounds.z);
        q.z = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleZ);

        return q.normalized;

        // How to use:
        //      Vector3 bounds = new Vector3(x, y, z); // ie: x axis will have a range of -x to x
        //          var dif = transform.position - target.position; // difference between positions of target and this object
        //          targetRot = Quaternion.LookRotation(dif);
        //      targetRot = ClampQuaternion(targetRot, bounds); // <---this part is the important one!!!!
        //      transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, time.deltaTime * 0.75f);
    }

    public static Quaternion ClampQuaternionX(Quaternion q, Vector3 boundsMin, Vector3 boundsMax)
    {
        q.x /= q.w; q.y /= q.w; q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, boundsMin.x, boundsMax.x);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        q.y = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.y);

        q.z = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.z);

        return q.normalized;

        // How to use:
        //      Vector3 bounds = new Vector3(x, y, z); // ie: x axis will have a range of -x to x
        //          var dif = transform.position - target.position; // difference between positions of target and this object
        //          targetRot = Quaternion.LookRotation(dif);
        //      targetRot = ClampQuaternion(targetRot, bounds); // <---this part is the important one!!!!
        //      transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, time.deltaTime * 0.75f);
    }
    #endregion
}
