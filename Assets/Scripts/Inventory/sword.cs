using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

namespace DefaultNamespace
{
    public class sword : ItemBehaviour
    {
        private Coroutine attackRoutine;
        [SerializeField] private Collider hitbox;
        [SerializeField] private float damage;
        [SerializeField] private float attackCooldown = 1.1f;
        
        private bool isBlocking = false;
        private enum animationIds
        {
            attack1,
            block1
        }

        private void Awake()
        {
            id = 1;
            type = itemType.weapon;
            name = "Sword";
            attackRoutine = null;
            hitbox.enabled = false;
            hitbox.isTrigger = true;
            hitbox.excludeLayers = LayerMask.NameToLayer("Player");
            hitbox.includeLayers = LayerMask.NameToLayer("Enemy");
        }

        public override void Action1() //attack
        {
            if (isBlocking)
            {
                Action2();
            }
            if (attackRoutine == null)
                attackRoutine = StartCoroutine(Attack());
        }

        public override void Action2()
        {
            //Debug.Log("Blocking or smth");
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();

            if (!player)
                return;
            if (isBlocking)
            {
                player.RemoveArmor(id);
            }
            else
            {
                var a = new PlayerBehaviour.Armor()
                {
                    ItemId = id,
                    Multiplier = 0.4f
                };
                player.RegisterArmor(a);
            }
            isBlocking = !isBlocking;
        }

        private void OnDisable()
        {
            if (!isBlocking) 
                return;
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
            if (!player)
                return;
            player.RemoveArmor(id);
            isBlocking = false;
        }

        private IEnumerator Attack()
        {
            //Debug.Log("Attacking maybe");
            hitbox.enabled = true;
            animationHandler.playAnimation((int)animationIds.attack1);
            itemSfxHandler.PlayAction1();
            yield return new WaitForSeconds(attackCooldown);
            hitbox.enabled = false;
            attackRoutine = null;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Enemy")) return;
            Debug.Log("Hit [" + other.tag + "] " + other.name);
            if (other.TryGetComponent<AbstractEnemyBehaviour>(out var enemy))
            {
                enemy.AttackEnemy(damage);
            }
        }
    }
}
