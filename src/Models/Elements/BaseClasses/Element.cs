using System;
using System.Text.Json.Serialization;
using GradeManagement.Interfaces;
using GradeManagement.Views.Lists.ElementButtonControls;
using JetBrains.Annotations;
using ReactiveUI;

// ReSharper disable once CheckNamespace
namespace GradeManagement.Models.Elements;

public abstract class Element : ReactiveObject, IGradable, ICloneable
{
    private const int MaxNameLength = 64;
    private const double EnabledOpacity = 1.0;
    private const double DisabledOpacity = 0.4;
    private const double DisabledOpacityGrade = 0.4;
    
    private string _name = string.Empty;
    private float _weighting;
    private bool _counts;
    private ButtonStyleBase? _buttonStyle;

    // ReSharper disable VirtualMemberCallInConstructor
    protected Element(string name, float weighting = 1f, bool counts = true)
    {
        if (name.Length > MaxNameLength)
            name = name.Substring(0, MaxNameLength);
        this.Name = name;
        this.Weighting = weighting;
        this.Counts = counts;
    }

    [JsonInclude]
    public string Name
    {
        get => _name; 
        protected set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    [UsedImplicitly] 
    public virtual float GradeValue { get; protected set; }

    [JsonInclude]
    public virtual float Weighting
    {
        get => _weighting; 
        protected set => this.RaiseAndSetIfChanged(ref _weighting, value);
    }
    
    [JsonInclude]
    public virtual bool Counts
    {
        get => _counts;
        protected set
        {
            this.RaiseAndSetIfChanged(ref _counts, value);
            this.RaisePropertyChanged(nameof(ElementsOpacity));
            this.RaisePropertyChanged(nameof(GradeTextOpacity));
        }
    }

    [JsonIgnore] 
    public abstract int ElementCount { get; }

    protected internal ButtonStyleBase? ButtonStyle
    {
        get => _buttonStyle;
        set => this.RaiseAndSetIfChanged(ref _buttonStyle, value);
    }
    
    internal double ElementsOpacity => Counts ? EnabledOpacity : DisabledOpacity;
    internal double GradeTextOpacity => Counts ? EnabledOpacity : DisabledOpacityGrade;

    public abstract object Clone();

    protected internal T? Duplicate<T>(bool save = true) where T : Element
    {
        if (Clone() is not T duplicate)
            return null;
            
        if(save)
            Save(duplicate);

        return duplicate;
    }
    
    protected internal abstract void Save<T>(T? element = null) where T : Element;
}