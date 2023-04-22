using System;

[Serializable]
public class ProjectDataDTO {

    public ProjectDataMsgType type;
    public string sender, receiver, data;
    
    public ProjectDataDTO(ProjectDataMsgType type, string sender, string receiver, string data) {
        this.type = type; this.sender = sender; this.receiver = receiver; this.data = data;
    }
}

