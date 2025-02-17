using MessagePipe;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope {
	protected override void Configure(IContainerBuilder builder) {
		builder.RegisterMessagePipe();

		builder.RegisterComponentInHierarchy<BerryList>().AsSelf().AsImplementedInterfaces();
	}
}