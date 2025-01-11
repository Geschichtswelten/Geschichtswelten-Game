using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class sword : ItemBehaviour
    {
        private Coroutine attackRoutine;
        [SerializeField] private Collider hitbox;
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
        }

        public override void action1() //attack
        {
            if (attackRoutine == null) 
                attackRoutine = StartCoroutine(attack());
        }

        private IEnumerator attack()
        {
            Debug.Log("Attacking maybe");
            hitbox.enabled = true;
            animationHandler.playAnimation((int) animationIds.attack1);
            itemSfxHandler.PlayAction1();
            yield return new WaitForSeconds(0.5f);
            hitbox.enabled = false;
            attackRoutine = null;
        }

        public override void action2()
        {
            Debug.Log("Blocking or smth");
        }


        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Enemy")) return;
            Debug.Log("Hit an Enemy");
        }
    }
}