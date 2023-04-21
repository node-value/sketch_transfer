using System;

[Serializable]
public class MessageDTO {
    public string receiver, message;
    
    public MessageDTO(string receiver, string message) { 
        this.receiver = receiver; this.message = message;
    }
}

