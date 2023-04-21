using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class ProjectData {
    public string           Name;         
    public BodyType         BodyType;
    public List<SketchData> SketchDataList;

    public ProjectData(string name, BodyType bodyType) { 
        this.Name = name; this.BodyType = bodyType;
        this.SketchDataList = new List<SketchData>();
    }
}

