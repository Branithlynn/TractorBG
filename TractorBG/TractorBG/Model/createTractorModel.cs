namespace TractorBG.Model
{
	public class createTractorModel
	{
		public string brand { get; set; }
		public string model { get; set; }
		public string description { get; set; }
		public int year { get; set; }
		public IFormFile file { get; set; }
	}
}
