using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows;

namespace Uranium.Classes
{
    public class Brushanimation : AnimationTimeline
    {
        public Brush From { get; set; }
        public Brush To { get; set; }
        public IEasingFunction EasingFunction { get; set; }

        public override Type TargetPropertyType => typeof(Brush);

        protected override Freezable CreateInstanceCore() => new Brushanimation();

        public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            if (animationClock.CurrentProgress == null)
                return From ?? defaultOriginValue;

            double progress = animationClock.CurrentProgress.Value;

            if (EasingFunction != null)
                progress = EasingFunction.Ease(progress);

            SolidColorBrush fromBrush = From as SolidColorBrush ?? defaultOriginValue as SolidColorBrush;
            SolidColorBrush toBrush = To as SolidColorBrush ?? defaultDestinationValue as SolidColorBrush;

            if (fromBrush == null || toBrush == null)
                return defaultOriginValue; // Fallback

            Color fromColor = fromBrush.Color;
            Color toColor = toBrush.Color;

            Color result = ColorExtensions.Lerp(fromColor, toColor, progress);
            return new SolidColorBrush(result);
        }
    }
}
