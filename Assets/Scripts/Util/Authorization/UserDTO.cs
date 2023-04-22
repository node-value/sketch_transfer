using System;

[Serializable]
internal class UserDTO {
    public string name, password;

    public UserDTO(string name, string password) {
        this.name = name;
        this.password = password;
    }
}

