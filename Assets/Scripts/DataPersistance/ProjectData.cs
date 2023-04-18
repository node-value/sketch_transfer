using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class ProjectData {
    private string           Name           { get; }
    private BodyType         BodyType       { get; }
    private List<SketchData> SketchDataList { get; }

    public ProjectData(string name, BodyType bodyType) { 
        this.Name = name; this.BodyType = bodyType;
        this.SketchDataList = new List<SketchData>();
    }
}

