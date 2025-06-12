namespace OmegaFY.Chat.API.Application.Result;

public abstract record class GenericResult : IResult
{
    private readonly List<ValidationError> _errors;

    public GenericResult() => _errors = new List<ValidationError>(2);

    public void AddError(string code, string message) => _errors.Add(new ValidationError(code, message));

    public bool Succeeded() => _errors?.Count == 0;

    public bool Failed() => !Succeeded();

    public IReadOnlyCollection<ValidationError> Errors() => _errors.AsReadOnly();

    public string GetErrorsAsStringSeparatedByNewLine() => string.Join(Environment.NewLine, _errors.Select(error => error.Message));
}