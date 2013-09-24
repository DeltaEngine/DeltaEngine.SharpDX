using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Core.Activators.Reflection;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;
using DeltaEngine.Graphics;
using DeltaEngine.Rendering3D.Mocks;
using DeltaEngine.Rendering3D.Models;
using DeltaEngine.ScreenSpaces;

namespace DeltaEngine.Platforms
{
	/// <summary>
	/// Basic resolver functionality via Autofac, each configuration registers concrete types. For
	/// example GLFW uses GLFWGraphics, GLFWSound, GLFWKeyboard, etc. and makes them available.
	/// </summary>
	public abstract class Resolver : IDisposable
	{
		protected Resolver()
		{
			assemblyLoader = new AssemblyTypeLoader(this);
		}

		private readonly AssemblyTypeLoader assemblyLoader;

		public void Register<T>()
		{
			Register(typeof(T));
		}

		public void Register(Type typeToRegister)
		{
			if (alreadyRegisteredTypes.Contains(typeToRegister))
				return;
			if (IsAlreadyInitialized)
				throw new UnableToRegisterMoreTypesAppAlreadyStarted();
			RegisterNonConcreteBaseTypes(typeToRegister, RegisterType(typeToRegister));
		}

		protected readonly List<Type> alreadyRegisteredTypes = new List<Type>();

		protected internal class UnableToRegisterMoreTypesAppAlreadyStarted : Exception {}

		public void RegisterSingleton<T>()
		{
			RegisterSingleton(typeof(T));
		}

		public void RegisterSingleton(Type typeToRegister)
		{
			if (alreadyRegisteredTypes.Contains(typeToRegister))
				return;
			if (IsAlreadyInitialized)
				throw new UnableToRegisterMoreTypesAppAlreadyStarted();
			RegisterNonConcreteBaseTypes(typeToRegister,
				RegisterType(typeToRegister).InstancePerLifetimeScope());
		}

		private
			IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>
			RegisterType(Type t)
		{
			AddRegisteredType(t);
			if (typeof(ContentData).IsAssignableFrom(t))
				return
					builder.RegisterType(t).AsSelf().FindConstructorsWith(publicAndNonPublicConstructorFinder).
					        UsingConstructor(contentConstructorSelector);
			return builder.RegisterType(t).AsSelf();
		}

		private readonly PublicAndNonPublicConstructorFinder publicAndNonPublicConstructorFinder =
			new PublicAndNonPublicConstructorFinder();

		private class PublicAndNonPublicConstructorFinder : IConstructorFinder
		{
			public ConstructorInfo[] FindConstructors(Type targetType)
			{
				return
					targetType.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic |
						BindingFlags.Instance);
			}
		}

		private readonly ContentConstructorSelector contentConstructorSelector =
			new ContentConstructorSelector();

		private class ContentConstructorSelector : IConstructorSelector
		{
			public ConstructorParameterBinding SelectConstructorBinding(
				ConstructorParameterBinding[] constructorBindings)
			{
				foreach (var constructor in constructorBindings)
				{
					var parameters = constructor.TargetConstructor.GetParameters();
					if (parameters.Length > 0 && parameters[0].ParameterType == typeof(string))
						return constructor;
				}
				return constructorBindings.First();
			}
		}

		private void AddRegisteredType(Type type)
		{
			if (type.Namespace.StartsWith("System") || type.Namespace.StartsWith("Microsoft"))
				return;
			if (!alreadyRegisteredTypes.Contains(type))
			{
				alreadyRegisteredTypes.Add(type);
				return;
			}
			if (ExceptionExtensions.IsDebugMode && !type.IsInterface && !type.IsAbstract)
				Console.WriteLine("Warning: Type " + type + " already exists in alreadyRegisteredTypes");
		}

		private ContainerBuilder builder = new ContainerBuilder();

		protected internal void RegisterInstance(object instance)
		{
			var registration =
				builder.RegisterInstance(instance).SingleInstance().AsSelf().AsImplementedInterfaces();
			AddRegisteredType(instance.GetType());
			RegisterAllBaseTypes(instance.GetType().BaseType, registration);
		}

		private void RegisterAllBaseTypes(Type baseType,
			IRegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle> registration)
		{
			while (baseType != null && baseType != typeof(object))
			{
				AddRegisteredType(baseType);
				registration.As(baseType);
				baseType = baseType.BaseType;
			}
		}

		private void RegisterNonConcreteBaseTypes(Type typeToRegister,
			IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>
				registration)
		{
			foreach (var type in typeToRegister.GetInterfaces())
				AddRegisteredType(type);
			registration.AsImplementedInterfaces();
			var baseType = typeToRegister.BaseType;
			while (baseType != null && baseType != typeof(object))
			{
				if (baseType.IsAbstract)
				{
					AddRegisteredType(baseType);
					registration.As(baseType);
				}
				baseType = baseType.BaseType;
			}
		}

		public BaseType Resolve<BaseType>() where BaseType : class
		{
			MakeSureContainerIsInitialized();
			if (typeof(BaseType) == typeof(EntitiesRunner))
				return EntitiesRunner.Current as BaseType;
			if (typeof(BaseType) == typeof(GlobalTime))
				return GlobalTime.Current as BaseType;
			if (typeof(BaseType) == typeof(ScreenSpace))
				return ScreenSpace.Current as BaseType;
			if (typeof(BaseType) == typeof(Randomizer))
				return Randomizer.Current as BaseType;
			return (BaseType)container.Resolve(typeof(BaseType));
		}

		private void MakeSureContainerIsInitialized()
		{
			if (IsAlreadyInitialized)
				return; //ncrunch: no coverage
			assemblyLoader.RegisterAllTypesFromAllAssemblies<ContentData, UpdateBehavior, DrawBehavior>();
			container = builder.Build();
			if (AllRegistrationCompleted != null)
				AllRegistrationCompleted();
		}

		protected bool IsAlreadyInitialized
		{
			get { return container != null; }
		}
		private IContainer container;

		protected event Action AllRegistrationCompleted;

		internal void RegisterAllTypesInAssembly(Type[] assemblyTypes)
		{
			foreach (Type type in assemblyTypes)
				if (AssemblyTypeLoader.IsTypeResolveable(type) && !alreadyRegisteredTypes.Contains(type))
				{
					builder.RegisterType(type).AsSelf().InstancePerLifetimeScope();
					AddRegisteredType(type);
				}
		}

		internal object Resolve(Type baseType, object customParameter = null)
		{
			MakeSureContainerIsInitialized();
			return ResolveAndShowErrorBoxIfNoDebuggerIsAttached(baseType, customParameter);
		}

		private object ResolveAndShowErrorBoxIfNoDebuggerIsAttached(Type baseType, object parameter)
		{
			try
			{
				return TryResolve(baseType, parameter);
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				if (Debugger.IsAttached || baseType == typeof(Window) ||
					StackTraceExtensions.StartedFromNCrunch)
					throw;
				//ncrunch: no coverage start
				ShowInitializationErrorBox(baseType, ex.InnerException ?? ex);
				return null;
				//ncrunch: no coverage end
			}
		}

		private object TryResolve(Type baseType, object parameter)
		{
			if (parameter == null)
				return container.Resolve(baseType);
			if (baseType == typeof(MeshAnimation))
				if (parameter is ContentCreationData)
					return new MockMeshAnimation((MeshAnimationCreationData)parameter);
				else
					return new MockMeshAnimation((string)parameter);
			if (parameter is ContentCreationData &&
				(baseType == typeof(Image) || baseType == typeof(Shader) ||
					baseType.Assembly.GetName().Name == "DeltaEngine.Graphics"))
				return CreateTypeManually(FindConcreteType(baseType), parameter);
			return container.Resolve(baseType,
				new Parameter[] { new TypedParameter(parameter.GetType(), parameter) });
		}

		private object CreateTypeManually(Type concreteTypeToCreate, object parameter)
		{
			return Activator.CreateInstance(concreteTypeToCreate,
				BindingFlags.NonPublic | BindingFlags.Instance, default(Binder),
				concreteTypeToCreate.Name.EndsWith("MockImage")
					? new[] { parameter } : new[] { parameter, Resolve<Device>() }, default(CultureInfo));
		}

		/// <summary>
		/// When resolving loading the first ContentData instance we need to make sure all Content is
		/// available and can be loaded. Otherwise we have to wait to avoid content crashes.
		/// </summary>
		internal abstract void MakeSureContentManagerIsReady();

		private Type FindConcreteType(Type baseType)
		{
			return
				alreadyRegisteredTypes.FirstOrDefault(
					type => !type.IsAbstract && baseType.IsAssignableFrom(type));
		}

		// ncrunch: no coverage start
		private void ShowInitializationErrorBox(Type baseType, Exception ex)
		{
			var exceptionText = StackTraceExtensions.FormatExceptionIntoClickableMultilineText(ex);
			var window = Resolve<Window>();
			window.CopyTextToClipboard(exceptionText);
			if (
				window.ShowMessageBox("Fatal Initialization Error",
					"Unable to resolve class " + baseType + "\n" +
						(ExceptionExtensions.IsDebugMode ? exceptionText : ex.GetType().Name + " " + ex.Message) +
						"\n\nMessage was logged and copied to the clipboard. Click Ignore to try to continue.",
					new[] { "Ignore", "Abort" }) == "Ignore")
				return;
			Dispose();
			if (!StackTraceExtensions.StartedFromNCrunch)
				Environment.Exit((int)AppRunner.ExitCode.InitializationError);
		} // ncrunch: no coverage end

		public virtual void Dispose()
		{
			if (IsAlreadyInitialized)
				DisposeContainerOnlyOnce();
		}

		private void DisposeContainerOnlyOnce()
		{
			var remContainerToDispose = container;
			container = null;
			remContainerToDispose.Dispose();
			builder = null;
		}
	}
}