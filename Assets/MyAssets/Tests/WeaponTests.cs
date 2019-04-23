using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class WeaponTests
    {
        GameObject weaponObject;
        Weapon testWeapon;


        Vector3 targetPosition;

        GameObject enemy;


        [SetUp]
        public void UnitySetup()
        {
            weaponObject = new GameObject();
            weaponObject.tag = "Player";
            GameObject gun = Resources.Load<GameObject>("Simple Gun");
            gun = GameObject.Instantiate(gun);
            gun.transform.parent = weaponObject.transform;

            GameObject obj = new GameObject();
            Camera camera = obj.AddComponent<Camera>();

            testWeapon =  weaponObject.AddComponent<Weapon>();
            testWeapon.camera = camera;
            testWeapon.shootOrigin = testWeapon.transform;
            testWeapon.damage = new CustomValue(30);
            testWeapon.bulletPrefab = Resources.Load<GameObject>("Bullet");


            enemy = GameObject.CreatePrimitive(PrimitiveType.Plane);
            enemy.transform.position = testWeapon.transform.position + testWeapon.transform.forward * 5;
            enemy.transform.rotation = Quaternion.Euler(-90, 0, 0);
            enemy.AddComponent<Enemy>();
            enemy.GetComponent<MeshCollider>().convex = true;
            enemy.GetComponent<MeshCollider>().isTrigger = true;

            
            
            

            //Time.timeScale = 10;
        }

        [UnityTest]
        public IEnumerator CanShoot()
        {
            if(testWeapon.Shoot())
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator DoesDamageToEnemy()
        {

            float enemyStartHealth = 10;
            float enemyHealth = 10;

            if (!testWeapon.Shoot())
            {
                Assert.Fail("Failed to shoot");
            }
            yield return new WaitForSeconds(2);

            enemyHealth = enemy.GetComponent<Health>().GetCurrent();

            Debug.Log(enemyHealth);

            Assert.Less(enemyHealth, enemyStartHealth);

            yield return null;
        }
    }
}
