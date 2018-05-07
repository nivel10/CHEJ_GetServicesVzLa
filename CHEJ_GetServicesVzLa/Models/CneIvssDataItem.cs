namespace CHEJ_GetServicesVzLa.Models
{
	public class CneIvssDataItem : CneIvssData
    {
		#region Properties

		public int UserId
		{
			get;
			set;
		}

		#endregion Properties

		#region Methods

		public override int GetHashCode()
		{
			return this.CneIvssDataId;
		}

		#endregion Methods
	}
}