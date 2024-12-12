using Cysharp.Threading.Tasks;
using MessagePipe;
using System;
using System.Threading;
using UnityEngine;
using VContainer;

public class WipeEffectController : MonoBehaviour {
	public struct WipeMessage : IEquatable<WipeMessage> {
		public bool WipesOut;

		public WipeMessage(bool wipesOut) {
			WipesOut = wipesOut;
		}

		public readonly bool Equals(WipeMessage other)
			=> WipesOut == other.WipesOut;
	}

	[SerializeField] private Material Material = null;

	[Inject] private IAsyncSubscriber<WipeMessage> _wipeAsyncSubscriber = null;

	private static readonly int WipeSize = Shader.PropertyToID("_WipeSize");

	private float _wipeSize = 1F;

	private async UniTask Wipe(WipeMessage msg, CancellationToken ct) {
		Time.timeScale = 0F;

		while (!ct.IsCancellationRequested) {
			bool finished;
			// ˆê’èŠÔŠu‚ÅƒƒCƒv—Ê‚ğ•Ï‰»‚³‚¹‚é
			if (msg.WipesOut) {
				_wipeSize = 0F;

				while (_wipeSize < 1F) {
					_wipeSize += 0.00390625F;
					Material.SetFloat(WipeSize, _wipeSize);
					await UniTask.Yield();
				}

				finished = true;
			} else {
				_wipeSize = 1F;

				while (_wipeSize > 0F) {
					_wipeSize -= 0.00390625F;
					Material.SetFloat(WipeSize, _wipeSize);
					await UniTask.Yield();
				}

				finished = true;
			}

			if (finished) {
				Time.timeScale = 1F;
				return;
			}
		}

		Time.timeScale = 1F;
	}

	private void Awake() {
		_wipeAsyncSubscriber.Subscribe(async (x, ct) => await Wipe(x, ct)).AddTo(this.GetCancellationTokenOnDestroy());
	}
}