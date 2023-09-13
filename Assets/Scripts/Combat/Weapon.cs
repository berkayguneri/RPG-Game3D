using RPG.Core;
using System;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon",menuName = "Weapons/Make New Weapon",order =0)]

    public class Weapon : ScriptableObject
    {
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] AnimatorOverrideController animationOverride = null;
        [SerializeField] float weaponRange=2f;
        [SerializeField] float weaponDamage = 10f;
        [SerializeField] bool isRightHand = true;
        [SerializeField] ProjectTile procejtTile = null;
        const string weaponName = "Weapon";

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);
            if (weaponPrefab != null)
            {
                Transform handTransform;
                if (isRightHand == true)
                {
                    handTransform = rightHand;
                }
                else
                {
                    handTransform = leftHand;
                }
                GameObject weapon= Instantiate(weaponPrefab, handTransform);
                weapon.name = weaponName;
            }
            if (animationOverride != null)
            {
                animator.runtimeAnimatorController = animationOverride;
            }
            
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if (oldWeapon == null)
            {
                return;
            }
            oldWeapon.name = "Destroy";

            Destroy(oldWeapon.gameObject);
        }

        public bool HasProjectTile()
        {
            return procejtTile != null;
        }
        
        public void LaunchProjectTile(Transform rightHand, Transform leftHand,Health target)
        {
            Transform handTransform;
            if (isRightHand)
            {
                handTransform = rightHand;
            }
            else
            {
                handTransform = leftHand;
            }

            ProjectTile projectTileInstance = Instantiate(procejtTile, handTransform.position,Quaternion.identity);
            projectTileInstance.SetTarget(target,weaponDamage);
        }
        public float GetDamage()
        {
            return weaponDamage;
        }

        public float GetRange()
        {
            return weaponRange;
        }
    }

}