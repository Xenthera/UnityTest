using System;
using UnityEngine;

/// <summary>
///	Provides a common base for creating any <see cref="MonoBehaviour"/>-derived class
///	which needs to act as a singleton, resulting in only one instance per game.
/// </summary>
/// <typeparam name="T"> The derived type of <see cref="MonoBehaviour"/> to use. </typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	/// <summary>
	///	Backing field for the <see cref="Instance"/> property.
	/// </summary>
	private static T _instance;

	/// <summary>
	///	Gets or sets the single instance of the specified type <typeparamref name="T"/> available.
	///	This will destroy additional instances of the singleton in the scene upon first access, if any.
	/// </summary>
	public static T Instance
	{
		get
		{
			if (_instance != null)
				return _instance;

			Type instanceType = typeof(T);
			var instances = FindObjectsOfType(instanceType);

			// Let the developer know if they are attempting to access a singleton that has no current instances in the scene.
			if (instances == null)
				throw new MissingComponentException($"An instance of {instanceType.Name} was requested, but no instance exists in the scene.");

			int instanceCount = instances.Length;

			// Let the developer know if they are attempting to access a singleton that has no current instances in the scene.
			if (instanceCount == 0)
				throw new MissingComponentException($"An instance of {instanceType.Name} was requested, but no instance exists in the scene.");

			_instance = instances[0] as T;

			if (instanceCount == 1)
				return _instance;

			// Destroy additional instances of the singleton to avoid confusion.
			for (int instanceIndex = 1; instanceIndex < instanceCount; instanceIndex++)
			{
				Debug.LogWarning($"Destroying an extra insance of {instanceType.Name}.");
				DestroyImmediate(instances[instanceIndex]);
			}

			string instanceTypeName = instanceType.Name;

			// Let the developer know that additional instances of the singleton were destroyed so they can correct their scene.
			Debug.LogWarning($"Destroyed {instanceCount - 1} additional instances of {instanceTypeName} in the scene.\r\nOnly one instance of {instanceTypeName} is permitted per scene. Remaining: {Instance.name}", Instance);

			return _instance;
		}

		protected set
		{
			_instance = value;
		}
	}

	/// <summary>
	///	Initializes the instance on Awake to prevent later spikes from delayed initialization. 
	/// </summary>
	protected virtual void Awake()
	{
		_instance = Instance;
	}
}