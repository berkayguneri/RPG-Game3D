using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;


namespace RPG.Combat {
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        
        
        [SerializeField] Weapon defaultWeapon = null;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        

        Health targetObject;
        float timeSinceLastAttack;
        

        private void Start()
        {
            SpawnSword(defaultWeapon);
        }
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (targetObject == null)
            {
                return;
            }
            if (targetObject.IsDead() == true)
            {
                GetComponent<Animator>().ResetTrigger("attack");
                Cancel();
                return;
            }
            

            if (GetIsInRange() == false)
            {
                GetComponent<Mover>().MoveTo(targetObject.transform.position,1f );
            }
            else
            {
                AttackMethod();
                GetComponent<Mover>().Cancel();
            }
        }


        public void SpawnSword(Weapon weapon)
        {
            defaultWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            defaultWeapon.Spawn(rightHandTransform,leftHandTransform, animator);
        }


        private void AttackMethod()
        {

            transform.LookAt(targetObject.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                TriggerAttack();
                timeSinceLastAttack = 0;

            }

        }


        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null)
            {
                return false;
            }
            Health healthToTest = GetComponent<Health>();
            return healthToTest != null && !healthToTest.IsDead();
            
        }
        public void Attack(GameObject target)
        {

            GetComponent<SchedulerAction>().StartAction(this);
            targetObject = target.GetComponent<Health>();
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        void Hit()
        {
            if (targetObject == null)
            {
                return;
            }
            if (defaultWeapon.HasProjectTile())
            {
                defaultWeapon.LaunchProjectTile(rightHandTransform, leftHandTransform,targetObject);
            }
            else
            {
                targetObject.TakeDamage(defaultWeapon.GetDamage());
            }
            
        }
        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, targetObject.transform.position) < defaultWeapon.GetRange();
        }

        
        public void Cancel()
        {
            StopAttack();
            targetObject = null;
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

    }

}
