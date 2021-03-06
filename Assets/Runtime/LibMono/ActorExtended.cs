//  Project  : ACTORS
//  Contacts : Pixeye - ask@pixeye.games

 
namespace Pixeye
{
	/// <summary>
	/// <para>An extended class of mono entity composer with update and signals.</para>
	/// </summary>
	public abstract class ActorExtended : Actor
	{
		bool conditionSignals;


		protected override void Awake()
		{
			conditionSignals = ProcessingSignals.Check(this);
			conditionManualDeploy = this is IManualDeploy;

			ProcessingEntities.Create(this);

			var cObject = Add<ComponentObject>();
			cObject.transform = transform;
		 
		}

		public override void OnEnable()
		{
			if (Starter.initialized == false) return;
			if (conditionManualDeploy)
				return;

			ProcessingUpdate.Default.Add(this);

			if (conditionSignals)
				ProcessingSignals.Default.Add(this);

			ProcessingEntities.Default.CheckGroups(entity, true);
		}

		public override void OnDisable()
		{
			if (conditionSignals)
				ProcessingSignals.Default.Remove(this);


			ProcessingUpdate.Default.Remove(this);
			ProcessingEntities.Default.CheckGroups(entity, false);
		}
	}
}