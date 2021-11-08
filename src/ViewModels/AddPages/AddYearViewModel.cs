﻿using Avalonia.Media;
using GradeManagement.ViewModels.BaseClasses;

namespace GradeManagement.ViewModels.AddPages
{
    public class AddYearViewModel : AddViewModelBase
    {
        public AddYearViewModel() => BorderBrushes = new SolidColorBrush[] { new(IncompleteColor) };

        protected override bool DataComplete => !string.IsNullOrEmpty(ElementName);
    }
}