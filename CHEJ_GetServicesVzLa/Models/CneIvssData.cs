namespace CHEJ_GetServicesVzLa.Models
{
	using System;

	public class CneIvssData
    {
        public int CneIvssDataId { get; set; }

        public int NationalityId { get; set; }

        public string IdentificationCard { get; set; }

        public DateTime BirthDate { get; set; }

        public bool IsCne { get; set; }

        public bool IsIvss { get; set; }
    }
}
