﻿using System;
using Godot;

/// <summary>
///   A window dialog for tutorials with custom behaviors such as show (pop-up) delay and animation.
/// </summary>
/// TODO: see https://github.com/Revolutionary-Games/Thrive/issues/2751
/// [Tool]
public partial class TutorialDialog : CustomWindow
{
#pragma warning disable CA2213
    [Export]
    private CustomRichTextLabel? label;
#pragma warning restore CA2213

    private string description = string.Empty;
    private string controllerDescription = string.Empty;

    private ActiveInputMethod showingTextForInput;

    [Export]
    public string Description
    {
        get => description;
        set
        {
            description = value;
            UpdateLabel();
        }
    }

    /// <summary>
    ///   Alternative description used when a controller is used for the game
    /// </summary>
    [Export]
    public string DescriptionForController
    {
        get => controllerDescription;
        set
        {
            controllerDescription = value;
            UpdateLabel();
        }
    }

    /// <summary>
    ///   Tweakable delay to make tutorial sequences flow more naturally.
    /// </summary>
    [Export]
    public float ShowDelay { get; set; } = 0.5f;

    public override void _Ready()
    {
        CheckShownTextVersion();
        UpdateLabel();
    }

    public override void _EnterTree()
    {
        base._EnterTree();

        if (label != null)
            CheckShownTextVersion();

        KeyPromptHelper.IconsChanged += OnInputTypeChanged;
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        KeyPromptHelper.IconsChanged -= OnInputTypeChanged;
    }

    protected override void OnOpen()
    {
        base.OnOpen();

        // Don't animate if currently running inside the editor
        if (Engine.IsEditorHint())
            return;

        PivotOffset = Size / 2;
        Scale = Vector2.Zero;

        var tween = CreateTween();
        tween.SetTrans(Tween.TransitionType.Expo);
        tween.SetEase(Tween.EaseType.Out);

        tween.TweenProperty(this, "scale", Vector2.One, 0.3).From(Vector2.Zero).SetDelay(ShowDelay);
    }

    private void UpdateLabel()
    {
        if (label == null)
            return;

        var oldText = label.ExtendedBbcode;

        string newText;

        if (showingTextForInput == ActiveInputMethod.Controller && !string.IsNullOrWhiteSpace(DescriptionForController))
        {
            newText = Localization.Translate(DescriptionForController);
        }
        else
        {
            newText = Localization.Translate(Description);
        }

        // We only set the text if it is actually different to avoid an extra parsing
        if (oldText != newText)
            label.ExtendedBbcode = newText;
    }

    private void OnInputTypeChanged(object? sender, EventArgs e)
    {
        CheckShownTextVersion();
    }

    private void CheckShownTextVersion()
    {
        var activeMethod = KeyPromptHelper.InputMethod;

        if (showingTextForInput == activeMethod)
            return;

        showingTextForInput = activeMethod;

        UpdateLabel();
    }
}
