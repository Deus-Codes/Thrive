﻿using System;
using Godot;
using Newtonsoft.Json;

/// <summary>
///   Label in the world on a fleet, can be pressed to select it
/// </summary>
public partial class FleetNameLabel : Button, IEntityNameLabel
{
    private string translationTemplate = null!;

    private string? previousName;
    private float previousStrength;

    public FleetNameLabel()
    {
        UpdateTranslationTemplate();
    }

    public event IEntityNameLabel.OnEntitySelected? OnEntitySelectedHandler;

    [JsonIgnore]
    public Control LabelControl => this;

    public override void _EnterTree()
    {
        base._EnterTree();
        Localization.Instance.OnTranslationsChanged += UpdateTranslationTemplate;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        Localization.Instance.OnTranslationsChanged -= UpdateTranslationTemplate;
    }

    public void UpdateFromEntity(IEntityWithNameLabel entity)
    {
        string newText;

        switch (entity)
        {
            case SpaceFleet fleet:

                if (fleet.UnitName == previousName &&
                    Math.Abs(fleet.CombatPower - previousStrength) < MathUtils.EPSILON)
                {
                    return;
                }

                previousName = fleet.UnitName;
                previousStrength = fleet.CombatPower;

                newText = translationTemplate.FormatSafe(previousName, previousStrength);
                break;

            default:
                throw new ArgumentException("Unsupported entity type", nameof(entity));
        }

        Text = newText;
    }

    private void UpdateTranslationTemplate()
    {
        translationTemplate = Localization.Translate("NAME_LABEL_FLEET");
    }

    private void ForwardSelection()
    {
        OnEntitySelectedHandler?.Invoke();
    }
}
