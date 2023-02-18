using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using GradeManagement.ViewModels;

namespace GradeManagement.Views.CustomControls;

public class CustomDragPanel : Panel, IStyleable
{
    private const double MinDragOffset = 10;
    
    private bool _isPressed;
    private Point _previousPosition;
    private Point? _initialPosition;
    private TranslateTransform? _transform;

    public CustomDragPanel()
    {
        InitializeComponent();
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        // The following actions are necessary for recognizing left-click as well
        AddHandler(PointerPressedEvent, OnPointerPressed, RoutingStrategies.Tunnel, true);
        AddHandler(PointerMovedEvent, OnPointerMoved, RoutingStrategies.Tunnel, true);
        AddHandler(PointerReleasedEvent, OnPointerReleased, RoutingStrategies.Tunnel, true);
        this.Initialized += (sender, args) => _initialPosition = Bounds.Position;
    }

    Type IStyleable.StyleKey => typeof(Panel);

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        // Only proceed if the left mouse button was pressed
        if (e.Pointer.Type == PointerType.Mouse && !e.GetCurrentPoint(Parent).Properties.IsLeftButtonPressed)
            return;
        
        Point currentPos = e.GetPosition(Parent);
        _isPressed = true;
        MainWindowViewModel.IsElementDragged = false;
        _previousPosition = currentPos;
        if (_transform != null)
            _previousPosition = new Point(_previousPosition.X - _transform.X, _previousPosition.Y - _transform.Y);
        
        base.OnPointerPressed(e);
    }
    
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
    
    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _isPressed = false;
        SetTransform(_initialPosition!.Value.X, _initialPosition.Value.Y); // Reset position
        base.OnPointerReleased(e);
    }

    private void SetTransform(double offsetX, double offsetY)
    {
        _transform = new TranslateTransform(offsetX, offsetY);
        RenderTransform = _transform;
    }
}