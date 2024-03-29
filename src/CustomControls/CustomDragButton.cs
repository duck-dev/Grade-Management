﻿using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Styling;

namespace GradeManagement.CustomControls
{
    public sealed class CustomDragButton : Button, IStyleable
    {
        public event EventHandler<PointerPressedEventArgs> PointerPressedPreview = delegate {  };
        
        Type IStyleable.StyleKey => typeof(Button);

        protected override void OnPointerPressed(PointerPressedEventArgs args)
        {
            PointerPressedPreview(this, args);
            base.OnPointerPressed(args);
        }
    }
}