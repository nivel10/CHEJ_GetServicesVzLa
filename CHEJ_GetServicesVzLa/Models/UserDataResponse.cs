namespace CHEJ_GetServicesVzLa.Models
{
	using System.Collections.Generic;
	using CHEJ_GetServicesVzLa.Helpers;

	public class UserDataResponse
    {
		public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

		public string Telephone { get; set; }

		public string ImagePath { get; set; }

        public int UserTypeId { get; set; }

        public object ImageArray { get; set; }

		public string Password { get; set; }
        
		//public string ImageFullPath 
		//{ 
		//	get
		//	{
		//		var ulrImageFullPath = string.Empty;

		//		if(!string.IsNullOrEmpty(this.ImagePath))
		//		{
		//			ulrImageFullPath = MethodsHelper.GetUrlAPI();
		//			ulrImageFullPath = ulrImageFullPath + ImagePath.Substring(1);
		//		}
		//		return ulrImageFullPath;            
		//	} 
		//}

		public string ImageFullPath
        { 
			get
			{
				var ulrImageFullPath = this.ImagePath;

                if(this.UserTypeId == 1)
                {
					if (!string.IsNullOrEmpty(this.ImagePath))
                    {
                        ulrImageFullPath = MethodsHelper.GetUrlAPI();
                        ulrImageFullPath = ulrImageFullPath + ImagePath.Substring(1);
                    }
                }
                
                return ulrImageFullPath;            
            } 
        }
        
        public string FullName { get; set; }

        public string AppName { get; set; }

        public List<CantvData> CantvDatas { get; set; }

        public List<CneIvssData> CneIvssDatas { get; set; }

        public List<ZoomData> ZoomDatas { get; set; }
    }
}