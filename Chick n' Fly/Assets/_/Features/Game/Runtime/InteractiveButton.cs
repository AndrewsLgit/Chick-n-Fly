/* todo: properly install PrimeTween to find Runtime assembly
using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveButton : MonoBehaviour, IPointerEnterHandler,  IPointerExitHandler
{
    #region Private Variables
    
    private RectTransform _rectTransform;
    [SerializeField] private float _highLightSize = 1.3f;
    [SerializeField] private float _highLightDuration = .2f;
    [SerializeField] private Ease _ease = Ease.Default;

    #endregion

    #region Unity API
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
    
    #endregion

    #region Main Methods
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _ease = Ease.InOutBounce;
        Debug.Log($"OnPointerEnter!");
        Tween.Scale(_rectTransform, Vector3.one * _highLightSize, _highLightDuration, _ease);
        _ease = Ease.Default;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"OnPointerExit!");
        Tween.Scale(_rectTransform, Vector3.one, _highLightDuration);
    }
    
    #endregion
} */