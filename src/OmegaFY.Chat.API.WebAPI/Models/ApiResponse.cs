using OmegaFY.Chat.API.Application.Shared;

namespace OmegaFY.Chat.API.WebAPI.Models;

public class ApiResponse : ApiResponse<object>
{
    public ApiResponse() : base() { }

    public ApiResponse(object data) : base(data) { }

    public ApiResponse(IReadOnlyCollection<ValidationError> errors) : base(errors) { }

    public ApiResponse(string code, string mensagem) : base(code, mensagem) { }
}