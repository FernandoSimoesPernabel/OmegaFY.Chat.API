namespace OmegaFY.Chat.API.Infra.Constants;

public static class OpenTelemetryConstants
{
    public const string ACTIVITY_APPLICATION_HANDLER_NAME = "ApplicationHandlers";

    public const string ACTIVITY_BASE_NAME = "omegafy";

    public const string REQUEST_CONTENT_KEY = $"{ACTIVITY_BASE_NAME}.request_content";

    public const string RESULT_CONTENT_KEY = $"{ACTIVITY_BASE_NAME}.result_content";

    public const string HANDLER_NAME_KEY = $"{ACTIVITY_BASE_NAME}.handler_name";

    public const string API_ROUTE = "api/";
}