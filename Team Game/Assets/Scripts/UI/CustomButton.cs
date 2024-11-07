using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IUpdateSelectedHandler, ISubmitHandler, IMoveHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerClickHandler {
	public event Action OnClickAsync = null;

	[SerializeField] private Color _standardColor = Color.black;
	[SerializeField] private Color _selectedColor = Color.black;
	[SerializeField] private Color _pressedColor = Color.black;

	public CustomButton SelectOnLeft = null;
	public CustomButton SelectOnUp = null;
	public CustomButton SelectOnRight = null;
	public CustomButton SelectOnDown = null;

	private Image _image = null;

	public void OnSelect(BaseEventData eventData) {
		_image.color = _selectedColor;
	}
	public void OnDeselect(BaseEventData eventData) {
		_image.color = _standardColor;
	}
	public void OnUpdateSelected(BaseEventData eventData) {
		_image.color = _selectedColor;
	}
	public void OnSubmit(BaseEventData eventData) {
		_image.color = _pressedColor;

		EventSystem.current.sendNavigationEvents = false;

		OnClickAsync?.Invoke();

		EventSystem.current.sendNavigationEvents = true;

		_image.color = _standardColor;
	}
	public void OnMove(AxisEventData eventData) {
		CustomButton nextSelected;
		switch (eventData.moveDir) {
			case MoveDirection.Left:
				nextSelected = SelectOnLeft;
				break;

			case MoveDirection.Up:
				nextSelected = SelectOnUp;
				break;

			case MoveDirection.Right:
				nextSelected = SelectOnRight;
				break;

			case MoveDirection.Down:
				nextSelected = SelectOnDown;
				break;

			default:
				return;
		}

		if (nextSelected == null) {
			return;
		}

		EventSystem.current.SetSelectedGameObject(nextSelected.gameObject);
	}
	public void OnPointerEnter(PointerEventData eventData) {
		EventSystem.current.SetSelectedGameObject(gameObject);
	}
	public void OnPointerExit(PointerEventData eventData) {
		EventSystem.current.SetSelectedGameObject(null);
	}
	public void OnPointerDown(PointerEventData eventData) {
		_image.color = _pressedColor;
	}
	public void OnPointerClick(PointerEventData eventData) {
		EventSystem.current.sendNavigationEvents = false;

		OnClickAsync?.Invoke();

		EventSystem.current.sendNavigationEvents = true;

		_image.color = _selectedColor;
	}
	private void Awake() {
		_image = GetComponent<Image>();
	}
}