using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool useEvents; // 해당 변수로 InteractionEvent를 추가하거나 제거할수있음
    public string promptMessage; // 플레이어의 화면에 띄워즐 메시지(무기면 무기 이름, 상호작용 할수있는 객체면 객체 이름)

    public virtual string OnLook()
    {
        return promptMessage;
    }

    public void BaseInteract()
    {
        if(useEvents)
        {
            GetComponent<InteractionEvent>().OnInteract.Invoke();
        }
        Interact();
    }

    protected virtual void Interact()
    {
        // 자식 클래스에서 작성
    }
}
