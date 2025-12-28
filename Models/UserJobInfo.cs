namespace dotnetApi.Models
{
    public class UserJobInfo
  {
         public int JobId { get; set; }
        public int UserId { get; set; }
        public string JobTitle { get; set; } = "";
        public string Department { get; set; } = "";
  }
}