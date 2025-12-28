namespace dotnetApi.Models
{
    public class UserSalary
    {
   

                public int SalaryId { get; set; }
        public int UserId { get; set; }
        public decimal Salary { get; set; }
        public string Currency { get; set; } = "USD";
    }
}