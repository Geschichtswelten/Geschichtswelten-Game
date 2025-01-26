using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Animator))]
    public class AnimationHandler : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private List<string> animations;

        public bool playAnimation(int id)
        {
            //Debug.Log("playing animation " + id);
            if (id >= animations.Count || animations[id] is null) return false; 
            
            animator.SetTrigger(animations[id]);
            return true;
        }
    }
}