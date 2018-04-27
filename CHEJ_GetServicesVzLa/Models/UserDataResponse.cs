namespace CHEJ_GetServicesVzLa.Models
{
	using System.Collections.Generic;

	public class UserDataResponse
    {
		public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public object Telephone { get; set; }

        public object ImagePath { get; set; }

        public int UserTypeId { get; set; }

        public object ImageArray { get; set; }

        public object Password { get; set; }

        public string ImageFullPath { get; set; }

        public string FullName { get; set; }

        public string AppName { get; set; }

        public List<CantvData> CantvDatas { get; set; }

        public List<CneIvssData> CneIvssDatas { get; set; }

        public List<ZoomData> ZoomDatas { get; set; }
    }
}