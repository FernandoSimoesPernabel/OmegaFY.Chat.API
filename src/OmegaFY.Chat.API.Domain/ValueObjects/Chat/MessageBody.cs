using OmegaFY.Chat.API.Common.Exceptions;
using OmegaFY.Chat.API.Domain.Constants;

namespace OmegaFY.Chat.API.Domain.ValueObjects.Chat;

public readonly record struct MessageBody
{
    public string Content { get; }

    public MessageBody(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new DomainArgumentException("Não foi informado nenhum conteudo para o corpo.");

        if (content.Length > ChatConstants.MESSAGE_BODY_MAX_LENGTH)
            throw new DomainArgumentException($"A mensagem não pode exceder o limite de {ChatConstants.MESSAGE_BODY_MAX_LENGTH} caracteres.");

        Content = content.Trim();
    }

    public override int GetHashCode() => Content.GetHashCode();

    public override string ToString() => Content.ToString();

    public static implicit operator string(MessageBody body) => body.Content;

    public static implicit operator MessageBody(string body) => new MessageBody(body);
}