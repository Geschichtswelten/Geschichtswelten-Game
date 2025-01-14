using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class Fists : ItemBehaviour
    {
        private Coroutine attackRoutine;
        [SerializeField] private Collider hitbox;
        [SerializeField] private float damage;
        private enum animationIds
        {
            attack1,
            block1
        }

        private void Awake()
        {
            id = 0;
            type = itemType.weapon;
            name = "Fists";
        }
        public override void action1()
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
        
        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Enemy")) return;
            Debug.Log("Hit an Enemy");
            other.gameObject.GetComponent<AbstractEnemyBehaviour>().AttackEnemy(damage);
        }

        public override void action2()
        {
            Debug.Log("Interacting or smth");
        }
    }
}