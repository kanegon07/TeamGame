using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
[RequireComponent(typeof(StaminaGauge))]
public class StaminaGaugeView : MonoBehaviour {
	[SerializeField] private Color NormalColor = Color.black;
	[SerializeField] private Color WarningColor = Color.black;
	[SerializeField] private Color DangerColor = Color.black;

	private static readonly float _maxStamina = 100F;
	private static readonly float _warningThreshold = 50F;
	private static readonly float _dangerThreshold = 25F;

	private Slider _slider = null;
	private Image _fillArea = null;
	private StaminaGauge _gauge = null;

	private void ReflectValue(float value) {
		if (value <= _dangerThreshold) {
			_fillArea.color = DangerColor;
		} else if (value <= _warningThreshold) {
			_fillArea.color = WarningColor;
		} else {
			_fillArea.color = NormalColor;
		}

		_slider.value = value;
	}

	private void Awake() {
		_slider = GetComponent<Slider>();
		_fillArea = transform.GetChild(0).GetChild(0).GetComponent<Image>();
		_gauge = GetComponent<StaminaGauge>();
	}

	private void Start() {
		_slider.maxValue = _maxStamina;

		_gauge.StaminaRP.Subscribe(x => ReflectValue(x))
			.AddTo(this.GetCancellationTokenOnDestroy());
	}
}