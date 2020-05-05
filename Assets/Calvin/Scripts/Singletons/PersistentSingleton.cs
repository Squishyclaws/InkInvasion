/// <summary>
/// A <see cref="Singleton{T}"/> that persists between scenes
/// </summary>
/// <typeparam name="T">The type of Singleton GameObject</typeparam>
public abstract class PersistentSingleton<T> : Singleton<T> where T : Singleton<T>
{
	/// <summary>
	/// Set the <see cref="T"/> to not destroy on scene loads.
	/// </summary>
	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(gameObject);
	}
}
