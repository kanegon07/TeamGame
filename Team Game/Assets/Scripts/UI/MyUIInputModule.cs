using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using VContainer;
using MessagePipe;

[RequireComponent(typeof(PlayerInput))]
public class MyUIInputModule : BaseInputModule {
	public struct SubmitInput { }
	public struct CancelInput { }
	public struct MoveInput {
		public MoveDirection Direction;

		public MoveInput(MoveDirection direction) {
			Direction = direction;
		}
	}
	public struct PointerEnterInput { }
	public struct PointerExitInput { }
	public struct PointerClickInput {
		public InputActionPhase Phase;

		public PointerClickInput(InputActionPhase phase) {
			Phase = phase;
		}
	}

	[Inject] private IPublisher<int, SubmitInput> _submitPublisher = null;
	[Inject] private IPublisher<int, CancelInput> _cancelPublisher = null;
	[Inject] private IPublisher<int, MoveInput> _movePublisher = null;
	[Inject] private IPublisher<int, PointerEnterInput> _pointerEnterPublisher = null;
	// [Inject] private IPublisher<int, PointerExitInput> _pointerExitPublisher = null;
	[Inject] private IPublisher<int, PointerClickInput> _pointerClickPublisher = null;

	private InputActionMap _inputActions = null;
	private Vector2 _currentPointerPosition = Vector2.down;
	private GameObject _currentEnteredObject = null;

	private bool ProcessNavigation() {
		GameObject selected = EventSystem.current.currentSelectedGameObject;
		if (selected == null) {
			return false;
		}

		int id = selected.GetInstanceID();

		if (_inputActions.FindAction("Submit").WasPerformedThisFrame()) {
			_submitPublisher.Publish(id, new SubmitInput());
			return true;
		}

		if (_inputActions.FindAction("Cancel").WasPerformedThisFrame()) {
			_cancelPublisher.Publish(id, new CancelInput());
			return true;
		}

		if (_inputActions.FindAction("Move").WasPerformedThisFrame()) {
			MoveDirection dir = MoveDirection.None;
			Vector2 axis = _inputActions.FindAction("Move").ReadValue<Vector2>();
			if (axis.x > 0F) {
				dir = MoveDirection.Right;
			} else if (axis.x < 0F) {
				dir = MoveDirection.Left;
			} else if (axis.y > 0F) {
				dir = MoveDirection.Up;
			} else if (axis.y < 0F) {
				dir = MoveDirection.Down;
			}

			_movePublisher.Publish(id, new MoveInput(dir));
			return true;
		}

		return false;
	}

	private void ProcessPointer() {
		List<RaycastResult> rayResults = new();
		PointerEventData eventData = new(EventSystem.current) {
			position = _currentPointerPosition
		};
		EventSystem.current.RaycastAll(eventData, rayResults);

		GameObject target = null;
		float minTargetDist = float.MaxValue;
		foreach (var result in rayResults) {
			if (result.distance < minTargetDist) {
				minTargetDist = result.distance;
				target = result.gameObject;
			}
		}

		if (target != _currentEnteredObject) {
			_pointerEnterPublisher.Publish(target.GetInstanceID(), new PointerEnterInput());

			if (_currentEnteredObject != null) {
				// _pointerExitPublisher.Publish(_currentEnteredObject.GetInstanceID(), new PointerExitInput());
			}

			_currentEnteredObject = target;

			return;
		}

		if (_currentEnteredObject == null) {
			return;
		}

		InputAction action = _inputActions.FindAction("LeftClick");
		if (action.WasPerformedThisFrame()) {
			_pointerClickPublisher.Publish(target.GetInstanceID(), new PointerClickInput(action.phase));
		}
	}

	public override void Process() {
		if (ProcessNavigation()) {
			return;
		}

		ProcessPointer();
	}

	protected override void Start() {
		PlayerInput input = GetComponent<PlayerInput>();
		_inputActions = input.currentActionMap;
	}
}