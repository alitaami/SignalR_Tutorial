// Model classes used in the controller actions
public class ChatMessageModel
{
    public string User { get; set; }
    public string Message { get; set; }
}

public class PrivateMessageModel
{
    public string Sender { get; set; }
    public string Receiver { get; set; }
    public string Message { get; set; }
}

public class UserMessageModel
{
    public string Sender { get; set; }
    public string UserId { get; set; }
    public string Message { get; set; }
}

public class ConnectionMessageModel
{
    public string Sender { get; set; }
    public string ConnectionId { get; set; }
    public string Message { get; set; }
}

public class GroupModel
{
    public string ConnectionId { get; set; }
    public string GroupName { get; set; }
}

public class GroupMessageModel
{
    public string Sender { get; set; }
    public string GroupName { get; set; }
    public string Message { get; set; }
}

public class CallerMessageModel
{
    public string Sender { get; set; }
    public string Message { get; set; }
}
