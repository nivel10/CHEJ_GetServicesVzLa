namespace CHEJ_GetServicesVzLa.Models
{
	public class NationalityData
    {
		public int NationalityId
		{
			get;
			set;
	
		}

		public string Abbreviation
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public object FullNationality
		{
			get
			{
				return string.Format("{0} - {1}", Abbreviation, Name);
			}
		}
	}
}