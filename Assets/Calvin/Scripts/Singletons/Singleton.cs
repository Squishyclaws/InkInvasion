using UnityEngine;
/// <summary>
/// A <see cref="MonoBehaviour"/> implementation of the Singleton Pattern
/// </summary>
/// <typeparam name="T">The type of the Singleton</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
	/// <summary>
	/// The instance of <see cref="T"/>
	/// </summary>
	public static T Instance { get; protected set; }

	/// <summary>
	/// Is there an existing instance of <see cref="T"/> in the scene?
	/// </summary>
	public static bool InstanceExists
	{
		get { return Instance != null; }
	}

	/// <summary>
	/// Assign the <see cref="Instance"/> and ensure there is only 1 of <see cref="T"/> in the scene
	/// </summary>
	protected virtual void Awake()
	{
		if(Instance != null)
		{
			//Debug.LogWarningFormat("There is another instance of <color=red>{0}</color>. There can only be one!", typeof(T));
			Destroy(gameObject);
		}
		else
		{
			Instance = (T)this;
			//print("Generate singleton: " + (T)this);
		}
	}

	/// <summary>
	/// Safely unassign <see cref="Instance"/>
	/// </summary>
	protected virtual void OnDestroy()
	{
		if(Instance == this)
		{
			Instance = null;
		}
	}
}
