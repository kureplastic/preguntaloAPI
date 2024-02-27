using System.ComponentModel.DataAnnotations;

namespace preguntaloAPI.Models
{
	public class PassView
	{
		[DataType(DataType.Password)]
		public string Password { get; set; } = "";
        [DataType(DataType.Password)]
		public string NewPassword { get; set; } = "";
        [DataType(DataType.Password)]
		public string ConfirmPassword { get; set; } = "";
	}
}