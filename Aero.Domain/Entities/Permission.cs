using Aero.Domain.Helpers;
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Aero.Domain.Entities;

public sealed class Permission
{
        public int SourceId { get; set; }  
        public string Name { get; set; }
        // public string Path { get; set; } = string.Empty;
        // public List<SubFeature> SubItems { get; set; } = new List<SubFeature>();
        public bool IsAllow { get; set; }
        public bool IsCreate { get; set; }
        public bool IsModify { get; set; }
        public bool IsDelete { get; set; }
        public bool IsAction { get; set; }

        public Permission(int source_id,string name,bool allow,bool create,bool modify,bool delete,bool action)
    {

        SetId(source_id);
        SetName(name);
        // this.Path = path;
        // this.SubItems = dub;
        this.IsAllow = allow;
        this.IsCreate = create;
        this.IsModify = modify;
        this.IsDelete = delete;
        this.IsAction = action;

    } 

    private void SetId(int id)
    {
        if (id < 0) throw new ArgumentException("Id invalid.");
        this.SourceId = id;
    }

    private void SetName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        this.Name = name;
    }


}
