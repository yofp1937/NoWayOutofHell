using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string proptMessage; // 플레이어의 화면에 띄워즐 메시지(무기면 무기 이름, 상호작용 할수있는 객체면 객체 이름)

    public void BaseInteract()
    {
        Interact();
    }

    protected virtual void Interact()
    {
        // 자식 클래스에서 작성
    }
}
