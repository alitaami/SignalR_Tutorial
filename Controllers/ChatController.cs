using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR_Tutorial.Hubs;

[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IHubContext<ChatHub> _hubContext;

    public ChatController(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
    }

    [HttpPost("SendMessage")]
    public async Task<IActionResult> SendMessage([FromBody] ChatMessageModel model)
    {
        if (ModelState.IsValid)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", model.User, model.Message);
            return Ok();
        }

        return BadRequest("Invalid message model.");
    }

    [HttpPost("SendPrivateMessage")]
    public async Task<IActionResult> SendPrivateMessage([FromBody] PrivateMessageModel model)
    {
        if (ModelState.IsValid)
        {
            await _hubContext.Clients.User(model.Receiver).SendAsync("ReceivePrivateMessage", model.Sender, model.Message);
            return Ok();
        }

        return BadRequest("Invalid private message model.");
    }

    [HttpPost("SendToUser")]
    public async Task<IActionResult> SendToUser([FromBody] UserMessageModel model)
    {
        if (ModelState.IsValid)
        {
            await _hubContext.Clients.User(model.UserId).SendAsync("ReceiveMessage", model.Sender, model.Message);
            return Ok();
        }

        return BadRequest("Invalid user message model.");
    }

    [HttpPost("SendToConnection")]
    public async Task<IActionResult> SendToConnection([FromBody] ConnectionMessageModel model)
    {
        if (ModelState.IsValid)
        {
            await _hubContext.Clients.Client(model.ConnectionId).SendAsync("ReceiveMessage", model.Sender, model.Message);
            return Ok();
        }

        return BadRequest("Invalid connection message model.");
    }

    [HttpPost("AddToGroup")]
    public async Task<IActionResult> AddToGroup([FromBody] GroupModel model)
    {
        if (ModelState.IsValid)
        {
            await _hubContext.Groups.AddToGroupAsync(model.ConnectionId, model.GroupName);
            return Ok();
        }

        return BadRequest("Invalid group model.");
    }

    [HttpPost("RemoveFromGroup")]
    public async Task<IActionResult> RemoveFromGroup([FromBody] GroupModel model)
    {
        if (ModelState.IsValid)
        {
            await _hubContext.Groups.RemoveFromGroupAsync(model.ConnectionId, model.GroupName);
            return Ok();
        }

        return BadRequest("Invalid group model.");
    }

    [HttpPost("SendToGroup")]
    public async Task<IActionResult> SendToGroup([FromBody] GroupMessageModel model)
    {
        if (ModelState.IsValid)
        {
            await _hubContext.Clients.Group(model.GroupName).SendAsync("ReceiveGroupMessage", model.Sender, model.Message);
            return Ok();
        }

        return BadRequest("Invalid group message model.");
    }

    [HttpPost("SendToCaller")]
    public async Task<IActionResult> SendToCaller([FromBody] CallerMessageModel model)
    {
        if (ModelState.IsValid)
        {
            // Get the connectionId from the request
            var connectionId = HttpContext.Items["ConnectionId"] as string;

            // Send the message to the caller using the connectionId
            await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", model.Sender, model.Message);
            return Ok();
        }

        return BadRequest("Invalid caller message model.");
    }

    [HttpPost("SendToAllExceptCaller")]
    public async Task<IActionResult> SendToAllExceptCaller([FromBody] CallerMessageModel model)
    {
        if (ModelState.IsValid)
        {
            // Get the connectionId from the request
            var connectionId = HttpContext.Items["ConnectionId"] as string;

            // Send the message to all others except the caller
            await _hubContext.Clients.AllExcept(connectionId).SendAsync("ReceiveMessage", model.Sender, model.Message);
            return Ok();
        }

        return BadRequest("Invalid message model.");
    }

}
