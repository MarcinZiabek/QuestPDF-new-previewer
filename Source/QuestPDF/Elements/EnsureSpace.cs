﻿using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    internal sealed class EnsureSpace : ContainerElement, IStateful
    {
        public const float DefaultMinHeight = 150;
        public float MinHeight { get; set; } = DefaultMinHeight;

        internal override SpacePlan Measure(Size availableSpace)
        {
            var measurement = base.Measure(availableSpace);

            if (IsFirstPageRendered)
                return measurement;

            if (measurement.Type != SpacePlanType.PartialRender)
                return measurement;

            if (MinHeight <= measurement.Height)
                return measurement;
            
            return SpacePlan.PartialRender(Size.Zero);
        }
        
        internal override void Draw(Size availableSpace)
        {
            if (IsFirstPageRendered)
            {
                base.Draw(availableSpace);
                return;
            }

            var measurement = base.Measure(availableSpace);
            
            if (MinHeight <= measurement.Height)
                base.Draw(availableSpace);
            
            IsFirstPageRendered = true;
        }

        internal override string? GetCompanionHint() => $"at least {MinHeight}";
        
        #region IStateful
        
        private bool IsFirstPageRendered { get; set; }

        public void ResetState(bool hardReset = false)
        {
            if (hardReset)
                IsFirstPageRendered = false;
        }
        
        public object GetState() => IsFirstPageRendered;
        public void SetState(object state) => IsFirstPageRendered = (bool) state;
    
        #endregion
    }
}