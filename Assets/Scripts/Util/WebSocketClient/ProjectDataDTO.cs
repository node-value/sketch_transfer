using System;

[Serializable]
public class ProjectDataDTO {

    public ProjectDataMsgType type;
    public string receiver, data;
    
    public ProjectDataDTO(ProjectDataMsgType type, string receiver, string data) {
        this.type = type; this.receiver = receiver; this.data = data;
    }
}

