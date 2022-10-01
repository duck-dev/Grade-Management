using Avalonia.Media;

namespace GradeManagement.ResourcesNamespace
{
    public abstract class Theme
    {
        protected const int StyleIndex = 2;
        
        // SolidColorBrush

        protected internal SolidColorBrush OppositeAccentBrush { get; protected set; } = null!;
        protected internal SolidColorBrush SameAccentBrush { get; protected set; } = null!;
        
        protected internal SolidColorBrush MainBackgroundBrush { get; protected set; } = null!;
        protected internal SolidColorBrush ElementBackgroundBrush { get; protected set; } = null!;
        protected internal SolidColorBrush VariantElementBackgroundBrush { get; protected set; } = null!;
        
        protected internal SolidColorBrush StandardGreyBrush { get; protected set; } = null!;
        protected internal SolidColorBrush VariantStandardGreyBrush { get; protected set; } = null!;
        protected internal SolidColorBrush DarkGreyBrush { get; protected set; } = null!;
        protected internal SolidColorBrush AlmostAccentBrush { get; protected set; } = null!;
        protected internal SolidColorBrush DarkerAlmostAccentBrush { get; protected set; } = null!;
        protected internal SolidColorBrush DarkenedDialogBackgroundBrush { get; protected set; } = null!;
        protected internal SolidColorBrush InfoButtonGreyBrush { get; protected set; } = null!;
        
        protected internal SolidColorBrush CalendarDayButtonHoverBrush { get; protected set; } = null!;
        protected internal SolidColorBrush CalendarDayButtonInactiveBrush { get; protected set; } = null!;
        protected internal SolidColorBrush CalendarDayButtonInactiveHoverBrush { get; protected set; } = null!;
        
        // Color

        protected internal Color OppositeAccent => OppositeAccentBrush.Color;
        protected internal Color SameAccent => SameAccentBrush.Color;
        
        protected internal Color MainBackground => MainBackgroundBrush.Color;
        protected internal Color ElementBackground => ElementBackgroundBrush.Color;
        protected internal Color VariantElementBackground => VariantElementBackgroundBrush.Color;
        
        protected internal Color StandardGrey => StandardGreyBrush.Color;
        protected internal Color VariantStandardGrey => VariantStandardGreyBrush.Color;
        protected internal Color DarkGrey => DarkGreyBrush.Color;
        protected internal Color AlmostAccent => AlmostAccentBrush.Color;
        protected internal Color DarkerAlmostAccent => DarkerAlmostAccentBrush.Color;
        protected internal Color DarkenedDialogBackground => DarkenedDialogBackgroundBrush.Color;
        protected internal Color InfoButtonGrey => InfoButtonGreyBrush.Color;
        
        protected internal Color CalendarDayButtonHover => CalendarDayButtonHoverBrush.Color;
        protected internal Color CalendarDayButtonInactive => CalendarDayButtonInactiveBrush.Color;
        protected internal Color CalendarDayButtonInactiveHover => CalendarDayButtonInactiveHoverBrush.Color;

        protected internal abstract void SetResources();
    }
}