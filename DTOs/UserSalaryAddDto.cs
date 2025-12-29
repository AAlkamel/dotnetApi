namespace dotnetApi.DTOs
{
    public class UserSalaryAddDto
    {
             public int UserId { get; set; }
        public decimal Salary { get; set; }
        public string Currency { get; set; } = "USD";
    }
}