using WebUIOver.Shared.Dto.Message;

namespace WebUIOver.Client.Services.CustomMessage;

public interface ICustomMessageTemplateService
{
    public Task InitializeAsync();
    public MessageTemplates GetMessageTemplates();
}