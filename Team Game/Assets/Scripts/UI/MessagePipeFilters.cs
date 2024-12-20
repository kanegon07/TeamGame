using Cysharp.Threading.Tasks;
using MessagePipe;
using System.Threading;
using System;

/*
 * MessagePipe用のフィルターを集めたもの
 * 使うかは分からない...
 */

public class WhereFilter<T> : MessageHandlerFilter<T> {
	private readonly Func<T, bool> _predicate;

	public WhereFilter(Func<T, bool> predicate) {
		_predicate = predicate;
	}

	public override void Handle(T message, Action<T> next) {
		if (!_predicate(message)) {
			return;
		}

		next(message);
	}
}

public class AsyncWhereFilter<T> : AsyncMessageHandlerFilter<T> {
	private readonly Func<T, bool> _predicate;

	public AsyncWhereFilter(Func<T, bool> predicate) {
		_predicate = predicate;
	}

	public override async UniTask HandleAsync(T message, CancellationToken cancellationToken, Func<T, CancellationToken, UniTask> next) {
		if (!_predicate(message)) {
			return;
		}

		await next(message, cancellationToken);
	}
}

public class SelectFilter<T> : MessageHandlerFilter<T> {
	private Func<T, T> _selectFunc;
	public SelectFilter(Func<T, T> selectFunc) {
		_selectFunc = selectFunc;
	}

	public override void Handle(T message, Action<T> next) {
		var selected = _selectFunc(message);
		next(selected);
	}
}

public class AsyncSelectFilter<T> : AsyncMessageHandlerFilter<T> {
	private readonly Func<T, T> _selectFunc;
	public AsyncSelectFilter(Func<T, T> selectFunc) {
		_selectFunc = selectFunc;
	}

	public override async UniTask HandleAsync(T message, CancellationToken cancellationToken, Func<T, CancellationToken, UniTask> next) {
		var selected = _selectFunc(message);
		await next(selected, cancellationToken);
	}
}

public class ThrottleFirstFilter<T> : MessageHandlerFilter<T> {
	private readonly TimeSpan _interval;
	private readonly object _lock = new object();
	private DateTime _lastInvocation;

	public ThrottleFirstFilter(TimeSpan interval) {
		_interval = interval;
		_lastInvocation = DateTime.MinValue;
	}

	public override void Handle(T message, Action<T> next) {
		lock (_lock) {
			var now = DateTime.UtcNow;
			if ((now - _lastInvocation) >= _interval) {
				_lastInvocation = now;
				next(message);
			}
		}
	}
}

public class AsyncThrottleFirstFilter<T> : AsyncMessageHandlerFilter<T> {
	private readonly TimeSpan _interval;
	private readonly SemaphoreSlim _semaphore = new(1, 1);
	private DateTime _lastInvocation;

	public AsyncThrottleFirstFilter(TimeSpan interval) {
		_interval = interval;
		_lastInvocation = DateTime.MinValue;
	}

	public override async UniTask HandleAsync(T message, CancellationToken cancellationToken, Func<T, CancellationToken, UniTask> next) {
		await _semaphore.WaitAsync();

		try {
			var now = DateTime.UtcNow;
			if ((now - _lastInvocation) >= _interval) {
				_lastInvocation = now;
				await next(message, cancellationToken);
			}
		} finally {
			_semaphore.Release();
		}
	}
}