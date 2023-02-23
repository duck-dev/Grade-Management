using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.VisualTree;
using GradeManagement.CustomDragDrop;
using GradeManagement.Models.Elements;
using GradeManagement.ViewModels;

namespace GradeManagement.Views.CustomControls;

public class CustomDragPanel : Panel, IStyleable, IDragObject, IDropTarget
{
    private const double MinDragOffset = 10; // DragOBJECT
    
    private bool _isPressed; // DragOBJECT
    private Point _previousPosition; // DragOBJECT
    private Point? _initialPosition; // DragOBJECT
    private TranslateTransform? _transform; // DragOBJECT
    private ContentPresenter? _visualParent; // Set ZIndex of ContentPresenter, otherwise it will have no effect
    private Element? _element;
    private IDropTarget? _dropTarget; // DragOBJECT

    public CustomDragPanel()
    {
        InitializeComponent();
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        
        // The following handlers are necessary for recognizing left-click as well:
        // DragOBJECT
        AddHandler(PointerPressedEvent, OnPointerPressed, RoutingStrategies.Tunnel, true);
        AddHandler(PointerMovedEvent, OnPointerMoved, RoutingStrategies.Tunnel, true);
        AddHandler(PointerReleasedEvent, OnPointerReleased, RoutingStrategies.Tunnel, true);

        this.Initialized += (sender, args) =>
        {
            _initialPosition = Bounds.Position;
            _visualParent = this.GetVisualParent<ContentPresenter>();
            if (this.DataContext is Element element)
                _element = element;
        };
    }

    Type IStyleable.StyleKey => typeof(Panel);
    
    // DropTARGET
    private bool IsDragDropValid
        => DragDropService.DataObject is { } dataObject && dataObject.Data is Element element && !ReferenceEquals(_element, element);
    
    // DragOBJECT
    public void OnDragOverTarget(IDropTarget target)
    {
        _dropTarget = target;
    }

    // DragOBJECT
    public void OnDragLeaveTarget(IDropTarget target)
    {
        if (ReferenceEquals(target, _dropTarget))
            _dropTarget = null;
        else
            throw new ArgumentException("The drag target doesn't match the currently assigned drag target.", nameof(target));
    }

    // DropTARGET
    public void DropData(object data)
    {
        if (data is not Element element)
            return;
        // TODO: Handle dropping data
    }

    // DragOBJECT
    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        // Only proceed if the left mouse button was pressed
        if (e.Pointer.Type == PointerType.Mouse && !e.GetCurrentPoint(Parent).Properties.IsLeftButtonPressed)
            return;
        
        Point currentPos = e.GetPosition(Parent);
        _isPressed = true;
        MainWindowViewModel.IsElementDragged = false;
        
        if(_visualParent != null)
            _visualParent.ZIndex = int.MaxValue;

        _previousPosition = currentPos;
        if (_transform != null)
            _previousPosition = new Point(_previousPosition.X - _transform.X, _previousPosition.Y - _transform.Y);
        
        if (_element is null)
            return;
        var dataObject = new DragDataObject(_element, this);
        DragDropService.StartDragDrop(dataObject);
        
        base.OnPointerPressed(e);
    }
    
    // DragOBJECT
    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        // Only proceed if the left mouse button was pressed
        if (!_isPressed || Parent is null || (e.Pointer.Type == PointerType.Mouse && !e.GetCurrentPoint(Parent).Properties.IsLeftButtonPressed))
            return;

        Point currentPos = e.GetPosition(Parent);
        double offsetX = currentPos.X - _previousPosition.X;
        double offsetY = currentPos.Y - _previousPosition.Y;
        if(Math.Abs(offsetX) >= MinDragOffset || Math.Abs(offsetY) >= MinDragOffset)
            MainWindowViewModel.IsElementDragged = true;
        
        SetTransform(offsetX, offsetY);
        base.OnPointerMoved(e);
    }
    
    // DragOBJECT
    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _isPressed = false;
        if(_visualParent != null)
            _visualParent.ZIndex = 0;
        SetTransform(_initialPosition!.Value.X, _initialPosition.Value.Y); // Reset position

        if (DragDropService.DataObject is { } dataObject && ReferenceEquals(dataObject.DragObject, this) && ReferenceEquals(dataObject.Data, _element))
            _dropTarget?.DropData(dataObject.Data);
        DragDropService.StopDragDrop();
        // TODO: Drop action related to DragObject: either REORDER or MOVE (delete from parent, move to grade group)
        
        base.OnPointerReleased(e);
    }

    // DropTARGET
    protected override void OnPointerEnter(PointerEventArgs e)
    {
        if (!IsDragDropValid)
            return;
        DragDropService.DataObject!.DragObject.OnDragOverTarget(this);
        // TODO: Setup visual drag over effects (this is the drop TARGET)
        base.OnPointerEnter(e);
    }

    // DropTARGET
    protected override void OnPointerLeave(PointerEventArgs e)
    {
        if (IsDragDropValid)
            DragDropService.DataObject!.DragObject.OnDragLeaveTarget(this);
        ResetDragOverEffects();
        base.OnPointerLeave(e);
    }

    // DropTARGET
    private void ResetDragOverEffects()
    {
        // TODO: Reset visual drag over effects
    }

    private void SetTransform(double offsetX, double offsetY)
    {
        _transform = new TranslateTransform(offsetX, offsetY);
        RenderTransform = _transform;
    }
}