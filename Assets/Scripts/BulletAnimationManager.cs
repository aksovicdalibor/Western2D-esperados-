using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAnimationManager : MonoBehaviour
{
    public Animator animator;

    public void PlayAnimation(string parameter)
    {
        bool flag = false;
        foreach(AnimatorControllerParameter p in animator.parameters)
        {
            if (p.name.Equals(parameter))
            {
                flag = true;
                switch (parameter)
                {
                    case "HasHitPlayer":
                        transform.localScale /= 5;
                        break;
                    default:
                        Debug.Log("Other specific config");
                        break;
                }
                animator.SetBool(parameter, true);
                Debug.Log("Parameter set!:");
                break;
            }
        }

        if (!flag)
        {
            Debug.Log("Parameter with name " + parameter + " not found!");
        }


        
    }
    public void DestroyObject()
    {
        Destroy(gameObject);
    }

}
