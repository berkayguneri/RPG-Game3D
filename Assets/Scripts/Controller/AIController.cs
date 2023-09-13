using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Controller
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float aggroCoolDownTime = 5f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] float wayPointTolerence = 1f;
        [SerializeField] float wayPointLifeTime = 3f;
        [Range(0,1)]    
        [SerializeField] float patrolSpeedFraction = 0.2f;
        [SerializeField] float shoutDistance = 5f;

        [SerializeField] PatrolPath patrolPath;

        GameObject player;
        float timeSinceLastSawPlayer;
        float timeSinceArrivedWayPoint;
        float timeSinceAggrevate = Mathf.Infinity;
        int currentWayPointIndex = 0;

        Fighter fighter;
        Health health;
        Mover mover;

        Vector3 enemyLocation;
        

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            enemyLocation = transform.position;
        }


        private void Update()
        {
            if (health.IsDead())
            {
                return;
            }
            if (IsAggrevated() && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                fighter.Attack(player);
                AggrevatedNearByEnemies();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                GetComponent<SchedulerAction>().CancelCurrentAction();
            }
            else
            {
                Vector3 nextPosition = enemyLocation;
                if (patrolPath != null)
                {
                    if (AtWayPoint())
                    {
                        timeSinceArrivedWayPoint = 0;
                        CycleWayPoint();
                    }
                    nextPosition = GetNextWayPoint();

                }

                if (timeSinceArrivedWayPoint > wayPointLifeTime)
                {
                    mover.StartMoveAction(nextPosition, patrolSpeedFraction);
                }

            }
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedWayPoint += Time.deltaTime;
            timeSinceAggrevate += Time.deltaTime;
        }

        private void AggrevatedNearByEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach(RaycastHit hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController>();
                if (ai == null) continue;
                ai.Aggrevate();
            }
        }

        private bool IsAggrevated()
        {
            return DistanceToPlayer() < chaseDistance || timeSinceAggrevate< aggroCoolDownTime;
        }

        public void Aggrevate()
        {
            timeSinceAggrevate = 0;
        }

        private Vector3 GetNextWayPoint()
        {
            return patrolPath.GetWayPointPosition(currentWayPointIndex);
        }

        private void CycleWayPoint()
        {
            currentWayPointIndex = patrolPath.GetNextIndex(currentWayPointIndex);
        }

        private bool AtWayPoint()
        {
            float distanceWayPoint = Vector3.Distance(transform.position, GetNextWayPoint());
            return distanceWayPoint < wayPointTolerence;
        }

        private float DistanceToPlayer()
        {
            return Vector3.Distance(player.transform.position, transform.position);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position,chaseDistance);   
        }

    }
}

