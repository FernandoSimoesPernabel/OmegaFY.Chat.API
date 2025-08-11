using OmegaFY.Chat.API.Application.Shared;

namespace OmegaFY.Chat.API.WebAPI.Models;

public class ApiResponse : ApiResponse<object>
{
    public ApiResponse() : base() { }

    public ApiResponse(object data) : base(data) { }

    public ApiResponse(IReadOnlyCollection<ValidationError> erros) : base(erros) { }

    public ApiResponse(string codigo, string mensagem) : base(codigo, mensagem) { }
}