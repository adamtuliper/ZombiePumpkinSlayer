using UnityEngine;
using System.Collections;


/// <summary>
/// This class goes on the weapon itself. It tracks hitting pumpkins
/// </summary>
public class PlayerAttack : MonoBehaviour
{
    
    //We need to store the hashes of the animation names.
    //We need the hash to check the name of the current animation, just how it is :)
    private int _smash1Hash;
    private int _smash2Hash;
    private int _smash3Hash;
    void Start()
    {
        //Generate the hashes from the animation names
        _smash1Hash = Animator.StringToHash("Base Layer.smash");
        _smash2Hash = Animator.StringToHash("Base Layer.smash2");
        _smash3Hash = Animator.StringToHash("Base Layer.smash3");
    }


    void OnTriggerEnter(Collider collider)
    {
        //If we've collided with a pumpkin
        if (collider.tag == "Pumpkin")
        {
            //Deal damage only if we're playing an attack animation.
            //We don't want a pumpkin just to run into our idle hammer and get hurt. Not fair.
            var parentAnimator = this.GetComponentInParent<Animator>();
            var stateInfo = parentAnimator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.nameHash == _smash1Hash ||
                stateInfo.nameHash == _smash2Hash ||
                stateInfo.nameHash == _smash3Hash)
            {
                //We are in the middle of a smash animation (ie, we are hitting)
                //Now lets apply damage to the pumpkin
                var pumpkinController = collider.gameObject.GetComponent<PumpkinController>();
                pumpkinController.ApplyDamage();
                //yield return new WaitForSeconds(1);
            }
            
        }
    }

    //IEnumerator ApplyPumpkinDamage(Collider collider)
    //{
    //    //Deal damage only if we're playing an attack animation.
    //    //We don't want a pumpkin just to run into our idle hammer and get hurt. Not fair.
    //    var parentAnimator = this.GetComponentInParent<Animator>();
    //    var stateInfo = parentAnimator.GetCurrentAnimatorStateInfo(0);
    //    if (stateInfo.nameHash == _smash1Hash ||
    //        stateInfo.nameHash == _smash2Hash ||
    //        stateInfo.nameHash == _smash3Hash)
    //    {
    //        //We are in the middle of a smash animation (ie, we are hitting)
    //        //Now lets apply damage to the pumpkin
    //        var pumpkinController = collider.gameObject.GetComponent<PumpkinController>();
    //        pumpkinController.ApplyDamage();
    //        yield return new WaitForSeconds(1);
    //    }
    //    else
    //    {
    //        //We're no longer in an attack, exit out.We probably died (the pumpkin that is)
    //        yield break;
    //    }
        
    //}
}
