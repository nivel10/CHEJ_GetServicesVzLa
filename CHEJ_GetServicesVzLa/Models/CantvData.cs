namespace CHEJ_GetServicesVzLa.Models
{
	public class CantvData
	{      
		#region Properties

		public int CantvDataId 
        {
            get;
            set;
        }

		public string CodePhone 
        {
            get;
            set;
        }

		public string NumberPhone 
        {
            get;
            set;
        }

		#endregion Properties

		#region Methods

		public string GetFullCantvData
		{
			get

			{
				return string.Format(
					"{0}-{1}",
					CodePhone,
					NumberPhone);
			}
		}

		public override int GetHashCode()
		{
            return CantvDataId;
		}

		#endregion Methods
	}
}