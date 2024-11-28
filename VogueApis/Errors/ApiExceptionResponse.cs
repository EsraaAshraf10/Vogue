namespace VogueApis.Errors
{
    public class ApiExceptionResponse : ApiResponse
    {
        public string Details { get; set; }
        public ApiExceptionResponse(int StutusCode , string? Message = null ,string? details = null):base(StutusCode , Message)
        {
            Details = details;
        }
    }
}
