namespace dotnetApi.DTOs
{
    public class UserLoginConfirmationDto
    {
        public byte[] Email { get; set; } = new byte[0];
        public byte[] ConfirmationCode { get; set; } = new byte[0];
    }
}