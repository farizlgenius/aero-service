using Aero.Domain.Helpers;
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Aero.Domain.Entities;

public sealed class Feature
{
        public int Id { get; set; }  
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public List<SubFeature> SubItems { get; set; } = new List<SubFeature>();
        public bool IsAllow { get; set; }
        public bool IsCreate { get; set; }
        public bool IsModify { get; set; }
        public bool IsDelete { get; set; }
        public bool IsAction { get; set; }

        public Feature(int id,string name,string path,List<SubFeature> dub,bool allow,bool create,bool modify,bool delete,bool action)
    {

        SetId(id);
        SetName(name);
        SetPath(path);
        this.SubItems = dub;
        this.IsAllow = allow;
        this.IsCreate = create;
        this.IsModify = modify;
        this.IsDelete = delete;
        this.IsAction = action;

    } 

    private void SetId(int id)
    {
        if (id <= 0) throw new ArgumentException("Id invalid.");
        this.Id = id;
    }

    private void SetName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (!RegexHelper.IsValidName(name)) throw new ArgumentException("Name invalid.",nameof(name));
        this.Name = name;
    }

    private void SetPath(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        this.Path = path;
    }

}
