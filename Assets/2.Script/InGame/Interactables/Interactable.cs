using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [Header("# Interactable's Main Data")]
    public bool useEvents; // 해당 변수로 InteractionEvent를 추가하거나 제거할수있음
    public string promptMessage; // 플레이어의 화면에 띄워즐 메시지(무기면 무기 이름, 상호작용 할수있는 객체면 객체 이름)

    public virtual string OnLook()
    {
        return promptMessage;
    }

    public void BaseInteract(GameObject player)
    {
        if(useEvents)
        {
            GetComponent<InteractionEvent>().OnInteract.Invoke();
        }
        
        if(player != null)
        {
            Interact(player);
        }
        else
        {
            Interact();
        }
    }

    /// <summary>
    /// UnityEvent 실행후 독립적으로 실행할 스크립트를 작성
    /// </summary>
    protected virtual void Interact()
    {
        // 자식 클래스에서 작성
    }

    /// <summary>
    /// UnityEvent 실행후 독립적으로 실행할 스크립트를 작성
    /// </summary>
    protected virtual void Interact(GameObject player)
    {
        // 자식 클래스에서 작성
    }
}
