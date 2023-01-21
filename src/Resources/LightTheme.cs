using Avalonia;
using Avalonia.Media;
using GradeManagement.UtilityCollection;

namespace GradeManagement.ResourcesNamespace
{
    public class LightTheme : Theme
    {
        public LightTheme()
        {
            SetResources();
        }

        protected internal override void SetResources()
        {
            OppositeAccentBrush = 
                Utilities.GetResourceFromStyle<SolidColorBrush, Application>(Application.Current, "OppositeAccent", StyleIndex) 
                ?? new SolidColorBrush(Color.Parse("#0A0A0A"));
            SameAccentBrush = 
                Utilities.GetResourceFromStyle<SolidColorBrush, Application>(Application.Current, "SameAccent", StyleIndex) 
                ?? new SolidColorBrush(Color.Parse("#FFFFFF"));
            
            MainBackgroundBrush = 
                Utilities.GetResourceFromStyle<SolidColorBrush, Application>(Application.Current, "MainBackground", StyleIndex) 
                ?? new SolidColorBrush(Color.Parse("#D8DDE6"));
            ElementBackgroundBrush = 
                Utilities.GetResourceFromStyle<SolidColorBrush, Application>(Application.Current, "ElementBackground", StyleIndex) 
                ?? new SolidColorBrush(Color.Parse("#C7CAD1"));
            VariantElementBackgroundBrush = 
                Utilities.GetResourceFromStyle<SolidColorBrush, Application>(Application.Current, "VariantElementBackground", StyleIndex) 
                ?? new SolidColorBrush(Color.Parse("#B8BABF"));
            
            StandardGreyBrush = 
                Utilities.GetResourceFromStyle<SolidColorBrush, Application>(Application.Current, "StandardGrey", StyleIndex) 
                ?? new SolidColorBrush(Color.Parse("#808080"));
            VariantStandardGreyBrush = 
                Utilities.GetResourceFromStyle<SolidColorBrush, Application>(Application.Current, "VariantStandardGrey", StyleIndex) 
                ?? new SolidColorBrush(Color.Parse("#919191"));
            DarkGreyBrush = 
                Utilities.GetResourceFromStyle<SolidColorBrush, Application>(Application.Current, "DarkGrey", StyleIndex) 
                ?? new SolidColorBrush(Color.Parse("#666666"));
            AlmostAccentBrush = 
                Utilities.GetResourceFromStyle<SolidColorBrush, Application>(Application.Current, "AlmostAccent", StyleIndex) 
                ?? new SolidColorBrush(Color.Parse("#E6E6E6"));
            DarkerAlmostAccentBrush = 
                Utilities.GetResourceFromStyle<SolidColorBrush, Application>(Application.Current, "DarkerAlmostAccent", StyleIndex) 
                ?? new SolidColorBrush(Color.Parse("#D4D4D4"));
            DarkenedDialogBackgroundBrush = 
                Utilities.GetResourceFromStyle<SolidColorBrush, Application>(Application.Current, "DarkenedDialogBackground", StyleIndex) 
                ?? new SolidColorBrush(Color.Parse("#66000000"));
            HighlyDarkenedBackgroundBrush =
                Utilities.GetResourceFromStyle<SolidColorBrush, Application>(Application.Current, "HighlyDarkenedBackground", StyleIndex)
                ?? new SolidColorBrush(Color.Parse("#99000000"));
            InfoButtonGreyBrush = 
                Utilities.GetResourceFromStyle<SolidColorBrush, Application>(Application.Current, "InfoButtonGrey", StyleIndex) 
                ?? new SolidColorBrush(Color.Parse("#8B8D92"));
            
            CalendarDayButtonHoverBrush = 
                Utilities.GetResourceFromStyle<SolidColorBrush, Application>(Application.Current, "CalendarDayButtonHover", StyleIndex) 
                ?? new SolidColorBrush(Color.Parse("#DCDCDC"));
            CalendarDayButtonInactiveBrush = 
                Utilities.GetResourceFromStyle<SolidColorBrush, Application>(Application.Current, "CalendarDayButtonInactive", StyleIndex) 
                ?? new SolidColorBrush(Color.Parse("#C9C9C9"));
            CalendarDayButtonInactiveHoverBrush = 
                Utilities.GetResourceFromStyle<SolidColorBrush, Application>(Application.Current, "CalendarDayButtonInactiveHover", StyleIndex) 
                ?? new SolidColorBrush(Color.Parse("#D1D1D1"));
        }
    }
}