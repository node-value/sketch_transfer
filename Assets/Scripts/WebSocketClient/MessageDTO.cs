using System;

[Serializable]
public class MessageDTO {
    public string sender, receiver, message;
    
    public MessageDTO(string sender, string receiver, string message) { 
        this.sender = sender; this.receiver = receiver; this.message = message;
    }
}

