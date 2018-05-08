namespace CHEJ_GetServicesVzLa.Models
{
	public class ZoomDataItem : ZoomData
    {
		#region Properties

		public int UserId
		{
			get;
			set;
		}

		#endregion Properties

		#region Constructor

		public ZoomDataItem()
		{
		}

		#endregion Constructor
              
		#region Methods

		public override int GetHashCode()
		{
			return this.ZoomDataId;
		}

		#endregion Methods
	}
}