using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class InitialDataDTO {
    public string Name;
    public BodyType BodyType;

    public InitialDataDTO(string name, BodyType bodyType) {
        this.Name = name; this.BodyType = bodyType;
    }
}

