﻿using System;
using Godot;
using Newtonsoft.Json;

/// <summary>
///   Base HUD class for stages where the player uses the strategic view
/// </summary>
/// <typeparam name="TStage">The type of the stage this HUD is for</typeparam>
[JsonObject(MemberSerialization.OptIn)]
[GodotAbstract]
public partial class StrategyStageHUDBase<TStage> : HUDWithPausing, IStrategyStageHUD
    where TStage : GodotObject, IStrategyStage
{
#pragma warning disable CA2213
    [Export]
    protected Label hintText = null!;

    [Export]
    protected HUDBottomBar bottomLeftBar = null!;

    [Export]
    protected ResearchScreen researchScreen = null!;
#pragma warning restore CA2213

    /// <summary>
    ///   Access to the stage to retrieve information for display
    /// </summary>
    protected TStage? stage;

    // These are private so this is a separate block
#pragma warning disable CA2213
    [Export]
    private HBoxContainer hotBar = null!;

    [Export]
    private ResourceDisplayBar resourceDisplay = null!;
#pragma warning restore CA2213

    protected StrategyStageHUDBase()
    {
    }

    // These signals need to be copied to inheriting classes for Godot editor to pick them up
    [Signal]
    public delegate void OnOpenMenuEventHandler();

    [Signal]
    public delegate void OnOpenMenuToHelpEventHandler();

    [Signal]
    public delegate void OnStartResearchingEventHandler(string technology);

    /// <summary>
    ///   Gets and sets the text that appears at the upper HUD.
    /// </summary>
    public string HintText
    {
        get => hintText.Text;
        set => hintText.Text = value;
    }

    public virtual void Init(TStage containedInStage)
    {
        stage = containedInStage;
    }

    public override void OnEnterStageLoadingScreen(bool longerDuration, bool returningFromEditor)
    {
        if (stage == null)
            throw new InvalidOperationException("Stage not setup for HUD");

        ShowLoadingScreen(stage);
    }

    public void OpenResearchScreen()
    {
        if (researchScreen.Visible)
        {
            researchScreen.Close();
        }
        else
        {
            researchScreen.AvailableTechnologies = stage?.CurrentGame?.TechWeb ??
                throw new InvalidOperationException("HUD not initialized");

            // This is not opened centered to allow the player to move the window and for that to be remembered
            researchScreen.Open();

            // TODO: update the hot bar state
        }
    }

    public void UpdateResourceDisplay(SocietyResourceStorage resourceStorage)
    {
        resourceDisplay.UpdateResources(resourceStorage);
    }

    public void UpdateScienceSpeed(float speed)
    {
        resourceDisplay.UpdateScienceAmount(speed);
    }

    public void UpdateResearchProgress(TechnologyProgress? currentResearch)
    {
        researchScreen.DisplayProgress(currentResearch);
    }

    public override void PauseButtonPressed(bool buttonState)
    {
        base.PauseButtonPressed(buttonState);

        bottomLeftBar.Paused = Paused;

        // if (!menu.Visible)
        // {
        //     // TODO: fossilisation if still wanted in this stage
        // }
    }

    protected void OpenMenu()
    {
        EmitSignal(SignalName.OnOpenMenu);
    }

    protected void OpenHelp()
    {
        EmitSignal(SignalName.OnOpenMenuToHelp);
    }

    private void StatisticsButtonPressed()
    {
        ThriveopediaManager.OpenPage("CurrentWorld");
    }

    private void ResearchScreenClosed()
    {
        // TODO: update the hot bar state
    }

    private void ForwardStartResearch(string technology)
    {
        EmitSignal(SignalName.OnStartResearching, technology);
    }
}
