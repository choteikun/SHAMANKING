using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random_Idle : StateMachineBehaviour
{
    [Tooltip("�]�mIdle�ƶq")]
    public int NumberOfIdleStates;

    [Tooltip("�]�m�̤p�H������ɶ��A��ĳ5��H�W�ݰ_�Ӥ�����`")]
    [Range(5, 10)]
    public float minNormTime;
    [Tooltip("�]�m�̤j�H������ɶ�")]
    [Range(11, 15)]
    public float maxNormTime;

    //�p��Idle����h�[
    protected float IdleTimer;
    //�üƭp�ɾ�
    protected float randomTimer;

    readonly int m_HashRandomIdle = Animator.StringToHash("RandomIdle");

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //�@�ӽd�򤺪��H���üƭp�ɾ�
        randomTimer = Random.Range(minNormTime, maxNormTime);//�@�ӽd�򤺪��H���üƭp�ɾ�
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("randomIdleTime:" + randomIdleTime/60 + "     randomIdleTimer:" + randomIdleTimer);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !animator.IsInTransition(0))//�p�G��e���A�Oidle�B���B��L����U
        {

            IdleTimer++;
            if (IdleTimer >= randomTimer * 60)
            {
                animator.SetInteger(m_HashRandomIdle, Random.Range(0, NumberOfIdleStates));//�]�m�H��idle1,2,3,4��...
            }
            else
            {
                animator.SetInteger(m_HashRandomIdle, -1);//�ѼƳ]��-1
            }
        }
        else
        {
            IdleTimer = 0;
        }

        ////�p�GIdle���A���W�X�H���M�w���k�@�Ʈɶ��åB�|���ഫ�A�h�]�m�H��Idle
        //if (stateInfo.normalizedTime > m_RandomNormTime && !animator.IsInTransition(0))
        //{
        //    animator.SetInteger(m_HashRandomIdle, Random.Range(0, numberOfStates));
        //}
    }
}
