using System;

[Serializable]
public class ProjectDataDTO {

    public string sender, receiver, data;
    
    public ProjectDataDTO(string sender, string receiver, string data) {
        this.sender = sender; this.receiver = receiver; this.data = data;
    }
}

