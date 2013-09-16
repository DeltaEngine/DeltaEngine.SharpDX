using System;

namespace DeltaEngine.Platforms
{
	internal class ConsoleCommandResolver
	{
		public ConsoleCommandResolver(Resolver resolver)
		{
			this.resolver = resolver;
		}
		private readonly Resolver resolver;

		public object Resolve(Type type)
		{
			return resolver.Resolve(type);
		}
	}
}