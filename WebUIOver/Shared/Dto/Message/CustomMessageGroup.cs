﻿namespace WebUIOver.Shared.Dto.Message;

public class CustomMessageGroup
{
    public CustomMessage UpMessage { get; set; } = new();
    public CustomMessage DownMessage { get; set; } = new();
    public CustomMessage LeftMessage { get; set; } = new();
    public CustomMessage RightMessage { get; set; } = new();
}