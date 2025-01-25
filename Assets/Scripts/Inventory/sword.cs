using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

namespace DefaultNamespace
{
    public class sword : ItemBehaviour
    {
        private Coroutine attackRoutine;
        [SerializeField] private Collider hitbox;
        [SerializeField] private float damage;
        [SerializeField] private float envDamageMod = .5f;
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

        public override void action1() //attack
        {
            if (attackRoutine == null)
                attackRoutine = StartCoroutine(attack());
        }

        public override void action2()
        {
            //Debug.Log("Blocking or smth");
        }
        
        private IEnumerator attack()
        {
            //Debug.Log("Attacking maybe");
            hitbox.enabled = true;
            animationHandler.playAnimation((int)animationIds.attack1);
            itemSfxHandler.PlayAction1();
            yield return new WaitForSeconds(0.5f);
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
