using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
internal class UserDTO {
    public string name, password;

    public UserDTO(string name, string password) {
        this.name = name;
        this.password = password;
    }
}

