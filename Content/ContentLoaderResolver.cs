using System;
using System.Globalization;
using System.Reflection;

namespace DeltaEngine.Content
{
	/// <summary>
	/// Simple factory to provide access to create content data on demand without any resolver.
	/// Once the <see cref="Resolver"/> is started it will replace this functionality.
	/// </summary>
	public class ContentLoaderResolver
	{
		internal ContentLoaderResolver() {}

		public virtual ContentLoader ResolveContentLoader(Type contentLoaderType)
		{
			return Activator.CreateInstance(contentLoaderType, PrivateBindingFlags, Type.DefaultBinder,
				null, CultureInfo.CurrentCulture) as ContentLoader;
		}

		private const BindingFlags PrivateBindingFlags =
			BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

		public virtual ContentData Resolve(Type contentType, string contentName)
		{
			return Activator.CreateInstance(contentType, PrivateBindingFlags, Type.DefaultBinder,
					new object[] { contentName }, CultureInfo.CurrentCulture) as ContentData;
		}

		public virtual ContentData Resolve(Type contentType, ContentCreationData data)
		{
			return Activator.CreateInstance(contentType, data) as ContentData;
		}

		public virtual void MakeSureResolverIsInitializedAndContentIsReady() {}
	}
}