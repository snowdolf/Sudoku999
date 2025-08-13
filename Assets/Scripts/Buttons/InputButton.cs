using UnityEngine;
using UnityEngine.EventSystems;

public class InputButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        UIManager.Instance.OpenInputPanel();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        System.Collections.Generic.List<RaycastResult> results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.name.Contains("InputCell"))
            {
                string cellName = result.gameObject.name;
                int number = int.Parse(cellName.Replace("InputCell", ""));

                UIManager.Instance.UpdateCellValue(number);
                UIManager.Instance.CloseInputPanel();

                return;
            }
        }

        UIManager.Instance.CloseInputPanel();
    }
}
