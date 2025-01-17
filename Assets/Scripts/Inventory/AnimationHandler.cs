using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Animator))]
    public class AnimationHandler : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private AnimatorController controller;
        [SerializeField] private List<Animation> animations;

        public bool playAnimation(int id)
        {
            //Debug.Log("playing animation " + id);
            if (id >= animations.Count || animations[id] is null) return false; 
            
            animator.SetTrigger(animations[id].name);
            return true;
        }
    }
}