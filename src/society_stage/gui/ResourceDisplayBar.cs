﻿using System.Linq;
using Godot;

/// <summary>
///   Displays the amount of stored resource in the strategy stages
/// </summary>
public partial class ResourceDisplayBar : HBoxContainer
{
#pragma warning disable CA2213
    [Export]
    public LabelSettings AmountLabelFont = null!;

    [Export]
    public LabelSettings AmountLabelFontFull = null!;

    [Export]
    public LabelSettings AmountLabelFontCritical = null!;

    [Export]
    private Container earlyResourcesContainer = null!;

    [Export]
    private Container lateResourcesContainer = null!;

    [Export]
    private Control scienceIndicatorContainer = null!;

    [Export]
    private Label scienceAmountLabel = null!;
#pragma warning restore CA2213

    private ChildObjectCache<WorldResource, DisplayAmount> resourceDisplayCache = null!;

    public override void _Ready()
    {
        scienceIndicatorContainer.Visible = false;
        scienceAmountLabel.LabelSettings = AmountLabelFont;

        // TODO: remove once this is used
        lateResourcesContainer.Visible = false;

        resourceDisplayCache =
            new ChildObjectCache<WorldResource, DisplayAmount>(earlyResourcesContainer, CreateDisplayAmount);
    }

    public void UpdateResources(SocietyResourceStorage resourceStorage)
    {
        // TODO: some resources should not always be shown in the bar
        // TODO: and some resources should be in the late resources container

        resourceDisplayCache.UnMarkAll();

        // The resources are sorted here to ensure consistent order of icons in the GUI, this would not really be
        // necessary each frame so TODO something better to only sort when required (for example when new items are
        // created)
        foreach (var pair in resourceStorage.GetAllResources().OrderBy(t => t.Key.InternalName))
        {
            var display = resourceDisplayCache.GetChild(pair.Key);

            display.SetAmount(pair.Value, pair.Value >= resourceStorage.Capacity);
        }

        resourceDisplayCache.DeleteUnmarked();
    }

    public void UpdateScienceAmount(float amount)
    {
        scienceIndicatorContainer.Visible = true;

        if (amount <= 0)
        {
            scienceAmountLabel.LabelSettings = AmountLabelFontCritical;
            scienceAmountLabel.Text = "0";
            return;
        }

        scienceAmountLabel.LabelSettings = AmountLabelFont;
        scienceAmountLabel.Text =
            StringUtils.FormatPositiveWithLeadingPlus(StringUtils.ThreeDigitFormat(amount), amount);
    }

    private DisplayAmount CreateDisplayAmount(WorldResource resource)
    {
        return new DisplayAmount(resource, AmountLabelFontFull, AmountLabelFont);
    }

    // Instances are created only through code
    // ReSharper disable once Godot.MissingParameterlessConstructor
    private partial class DisplayAmount : HBoxContainer
    {
        private readonly LabelSettings maxColour;
        private readonly LabelSettings normalColour;

#pragma warning disable CA2213
        private readonly Label amountLabel;
#pragma warning restore CA2213

        private string? previousAmount;
        private bool previousMax;

        public DisplayAmount(WorldResource resource, LabelSettings maxColour, LabelSettings normalColour)
        {
            this.maxColour = maxColour;
            this.normalColour = normalColour;

            amountLabel = new Label
            {
                Text = "0",
                VerticalAlignment = VerticalAlignment.Center,
            };

            amountLabel.LabelSettings = normalColour;

            // TODO: reserving space for the characters would help to have the display jitter less

            AddChild(amountLabel);

            AddChild(new TextureRect
            {
                StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
                CustomMinimumSize = new Vector2(16, 16),
                Texture = resource.Icon,
                ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
            });

            // TODO: tooltips showing the resource name and where it comes from and what consumes it

            // TODO: click callbacks
        }

        public void SetAmount(float amount, bool max)
        {
            // TODO: would be nice to not round up, but floor the values for display
            var newAmountString = StringUtils.ThreeDigitFormat(amount);

            // A bit of an early skip to skip some operations if nothing has changed
            if (previousAmount == newAmountString && max == previousMax)
                return;

            previousAmount = newAmountString;
            previousMax = max;

            amountLabel.Text = newAmountString;

            amountLabel.LabelSettings = max ? maxColour : normalColour;
        }
    }
}
