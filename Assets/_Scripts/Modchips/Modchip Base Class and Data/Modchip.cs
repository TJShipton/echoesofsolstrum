using UnityEngine;

public abstract class Modchip : MonoBehaviour
{

    public string modchipName;
    public ModchipData modchipData;

    protected Animator modchipAnimator;



    public void InitModchipAnimator(Animator animator)
    {
        this.modchipAnimator = animator;
    }



    public virtual void ModAttack()
    {


    }


}
