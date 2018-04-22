namespace CHEJ_GetServicesVzLa.Infrastructure
{
	using CHEJ_GetServicesVzLa.ViewModels;

	public class InstanceLocator
    {
		#region Properties

		public MainViewModel Main
		{
			get;
			set;
		}

		#endregion Properties

		#region Constructor

		public InstanceLocator()
		{
			this.Main = new MainViewModel();
		}

		#endregion Constructor
    }
}
