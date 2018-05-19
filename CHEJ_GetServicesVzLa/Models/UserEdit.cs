namespace CHEJ_GetServicesVzLa.Models
{
	public class UserEdit : User
    {
		#region Properties

		public int UserId
		{
			get;
			set;
		}

		public string NewEmail
		{
			get;
			set;
		}

		public string NewPassword
		{
			get;
			set;
		}

		public byte[] ImageArray
		{
			get;
			set;
		}

		#endregion Properties

		#region Methods

		public override int GetHashCode()
		{
			return this.UserId;
		}
        
		#endregion Methods
	}
}