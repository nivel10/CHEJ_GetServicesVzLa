namespace CHEJ_GetServicesVzLa.Models
{
	using System;
	using System.Collections.Generic;
	using Newtonsoft.Json;

	public class CneIvssData
    {
        #region Properties

		public int CneIvssDataId { get; set; }

		public int NationalityId { get; set; }

		public string IdentificationCard { get; set; }

		public DateTime BirthDate { get; set; }

		public bool IsCne { get; set; }

		public bool IsIvss { get; set; }

		public List<NationalityData> NationalityDatas { get; set; }

		#endregion Properties

		#region Methods

		[JsonIgnore]
		public object GetCneIvssFull
		{
			get
			{
				if (NationalityDatas.Count > 0)
				{
					return string.Format(
						"{0} - {1}",
						NationalityDatas[0].Abbreviation,
						IdentificationCard);
				}
				else
				{
					return "";
				}
			}
		}

		#endregion Methods
    }
}