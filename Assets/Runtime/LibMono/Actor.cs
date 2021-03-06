//  Project  : ACTORS
//  Contacts : Pixeye - ask@pixeye.games


using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#endif


namespace Pixeye
{
	/// <summary>
	/// <para>A base class of mono entity composer.</para>
	/// </summary>
	public abstract class Actor : MonoEntity, IRequireStarter
	{
		[FoldoutGroup("Main"), TagFilter(typeof(Pool))]
		public int pool;

		[NonSerialized]
		public bool conditionManualDeploy;

		#region METHODS

		protected virtual void Awake()
		{
			ProcessingEntities.Create(this);
			conditionManualDeploy = this is IManualDeploy;
			var cObject = Add<ComponentObject>();
			cObject.transform = transform;
			if (Starter.initialized == false) return;
			Setup();
		}


		/// <summary>
		/// <para>Setup is used to add components and tags to the entity. Use it instead of the Awake/Start methods.</para>
		/// </summary>
		protected abstract void Setup();


		public void SetupAfterStarter()
		{
			Setup();
			if (conditionManualDeploy) return;
			OnEnable();
		}


		public virtual void OnEnable()
		{
			if (Starter.initialized == false) return;
			if (conditionManualDeploy) return;
			conditionEnabled = true;
			ProcessingEntities.Default.CheckGroups(entity, true);
		}

		public virtual void OnDisable()
		{
			conditionEnabled = false;
			ProcessingEntities.Default.CheckGroups(entity, false);
		}

		protected void OnDestroy()
		{
			int len = Storage.all.Count;

			for (int j = 0; j < len; j++)
			{
				Storage.all[j].RemoveNoCheck(entity);
			}

			Tags.Clear(entity);
			ProcessingEntities.prevID.Push(entity);
		}

		/// <summary>
		/// <para>Destroys the object. If the poolID is defined, deactivates the object instead.</para>
		/// </summary>
		public override void Release()
		{
			if (pool == Pool.None)
			{
				Destroy(gameObject, 0.03f);
				return;
			}

			gameObject.Release(pool);
		}

		#endregion

		#region ADD/REMOVE

		/// <summary>
		/// <para>Adds the tag to the entity.</para>
		/// </summary>
		/// <param name="tags"></param>
		protected void Add(int tags) { Tags.AddTagsRaw(entity, tags); }

		/// <summary>
		/// <para>Adds tags to the entity.</para>
		/// </summary>
		/// <param name="tags"></param>
		protected void Add(params int[] tags) { Tags.AddTagsRaw(entity, tags); }

		/// <summary>
		/// <para>Adds the component to the entity. Triggers Setup method on the components inherited from ISetup.</para>
		/// </summary>
		/// <param name="component"></param>
		/// <typeparam name="T"></typeparam>
		protected void Add<T>(T component) where T : IComponent, new()
		{
			var setupable = component as ISetup;
			if (setupable != null)
			{
				setupable.Setup(entity);
			}

			Storage<T>.Instance.AddWithNoCheck(component, entity);
		}

		/// <summary>
		/// <para>Adds the component to the entity by type. Triggers Setup method on the components inherited from ISetup.</para>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns>Returns created component.</returns>
		protected T Add<T>() where T : IComponent, new()
		{
			var component = Storage<T>.Instance.GetOrCreate(entity);
			var setupable = component as ISetup;
			if (setupable != null)
			{
				setupable.Setup(entity);
			}

			return component;
		}

		#endregion
	}
}