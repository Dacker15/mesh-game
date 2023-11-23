using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuRotation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject rotationObject;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float direction;

    private Quaternion quaternion;
    private float actualRotation;
    private bool isRotated;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        isRotated = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isRotated = false;
        rotationObject.transform.rotation = quaternion;
    }

    private void Awake()
    {
        actualRotation = rotationSpeed * direction * Time.deltaTime;
        quaternion = rotationObject.transform.rotation;
    }

    private void Update()
    {
        if (isRotated)
        {
            rotationObject.transform.Rotate(0, actualRotation, 0);   
        }
    }
}
