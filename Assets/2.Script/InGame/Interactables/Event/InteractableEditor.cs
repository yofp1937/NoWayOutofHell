using UnityEditor;

// Interactable 스크립트의 useEvents가 true면 Inspector에 Event 자동으로 추가해주는 스크립트
[CustomEditor(typeof(Interactable), true)]
public class InteractableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Interactable interactable = (Interactable)target;
        if(target.GetType() == typeof(EventOnlyInteractable))
        {
            // Editor를 개발자가 커스텀해서 보여주기때문에 EventOnlyInteractable을 부착해도 promptMessage는 Inspector에 표시되지않음
            // 하지만 EditorGUILayout.TextField로 promptMessage를 Inspector에 표시해줌으로써 사용자가 Inspector에서 promptMessage를 수정할수있음
            interactable.promptMessage = EditorGUILayout.TextField("Prompt Message", interactable.promptMessage);
            EditorGUILayout.HelpBox("EventOnlyInteract 스크립트는 이벤트 발행 용도로만 사용할수있습니다.", MessageType.Info);
            if(interactable.GetComponent<InteractionEvent>() == null)
            {
                interactable.useEvents = true;
                interactable.gameObject.AddComponent<InteractionEvent>();
            }
        }
        else
        {
            base.OnInspectorGUI();
            if(interactable.useEvents)
            {
                if(interactable.GetComponent<InteractionEvent>() == null)
                {
                    interactable.gameObject.AddComponent<InteractionEvent>();
                }
            }
            else
            {
                if(interactable.GetComponent<InteractionEvent>() != null)
                {
                    DestroyImmediate(interactable.GetComponent<InteractionEvent>(), true);
                }
            }
        }
    }
}
