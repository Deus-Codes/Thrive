﻿using System;
using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;

/// <summary>
///   Manages the multicellular HUD scene
/// </summary>
[JsonObject(MemberSerialization.OptIn)]
public partial class MacroscopicHUD : CreatureStageHUDBase<MacroscopicStage>
{
#pragma warning disable CA2213
    [Export]
    private CustomWindow moveToLandPopup = null!;

    [Export]
    private Button toLandButton = null!;

    [Export]
    private Button awakenButton = null!;

    [Export]
    private CustomWindow awakenConfirmPopup = null!;

    [Export]
    private ActionButton interactAction = null!;

    [Export]
    private ActionButton inventoryButton = null!;

    [Export]
    private ActionButton buildButton = null!;

    [Export]
    private InventoryScreen inventoryScreen = null!;
#pragma warning restore CA2213

    private float? lastBrainPower;

    [Signal]
    public delegate void OnInteractButtonPressedEventHandler();

    [Signal]
    public delegate void OnOpenInventoryPressedEventHandler();

    [Signal]
    public delegate void OnOpenBuildPressedEventHandler();

    [JsonIgnore]
    public bool IsInventoryOpen => inventoryScreen.IsOpen;

    protected override string? UnPauseHelpText => null;

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (stage == null)
            return;

        if (stage.HasPlayer)
        {
            UpdateAwakenButton(stage.Player!);

            // If the player is already in the awakening stage they can't move to land anymore
            if (stage.Player!.Species.MacroscopicType == MacroscopicSpeciesType.Awakened)
            {
                toLandButton.Visible = false;
            }
            else
            {
                // Hide the land button when already on the land in the prototype
                toLandButton.Visible = stage.Player!.MovementMode == MovementMode.Swimming;
            }
        }
        else
        {
            awakenButton.Visible = false;
            toLandButton.Visible = false;
        }
    }

    public void OpenInventory(MacroscopicCreature creature, IEnumerable<IInteractableEntity> groundObjects,
        bool playerTechnologies = true)
    {
        inventoryScreen.OpenInventory(creature, playerTechnologies ? stage!.CurrentGame!.TechWeb : null);
        inventoryScreen.UpdateGroundItems(groundObjects);
    }

    public void CloseInventory()
    {
        inventoryScreen.Close();
    }

    public void SelectItemForCrafting(IInteractableEntity target)
    {
        inventoryScreen.AddItemToCrafting(target);
    }

    protected override void UpdateFossilisationButtonStates()
    {
    }

    protected override void ShowFossilisationButtons()
    {
    }

    protected override void ReadPlayerHitpoints(out float hp, out float maxHP)
    {
        // TODO: player hitpoints
        hp = 100;
        maxHP = 100;
    }

    protected override float? ReadPlayerStrainFraction()
    {
        // TODO: strain
        return null;
    }

    protected override bool CanSprint()
    {
        // TODO: sprinting
        return false;
    }

    protected override CompoundBag GetPlayerUsefulCompounds()
    {
        return stage!.Player!.ProcessCompoundStorage;
    }

    protected override Func<Compound, bool> GetIsUsefulCheck()
    {
        var bag = stage!.Player!.ProcessCompoundStorage;
        return c => bag.IsUseful(c);
    }

    protected override bool ShouldShowAgentsPanel()
    {
        throw new NotImplementedException();
    }

    protected override void CalculatePlayerReproductionProgress(Dictionary<Compound, float> gatheredCompounds,
        Dictionary<Compound, float> totalNeededCompounds)
    {
        // TODO: reproduction process for multicellular
        gatheredCompounds.Clear();
        gatheredCompounds[Compound.Ammonia] = 1;
        gatheredCompounds[Compound.Phosphates] = 1;

        totalNeededCompounds.Clear();
        totalNeededCompounds[Compound.Ammonia] = 1;
        totalNeededCompounds[Compound.Phosphates] = 1;
    }

    protected override ICompoundStorage GetPlayerStorage()
    {
        return stage!.Player!.ProcessCompoundStorage;
    }

    protected override ProcessStatistics? GetPlayerProcessStatistics()
    {
        return stage!.Player!.ProcessStatistics;
    }

    protected override void UpdateHoverInfo(float delta)
    {
        // TODO: implement looked at entities
    }

    protected override void UpdateAbilitiesHotBar()
    {
        // This button is visible when the player is in the awakening stage
        bool isAwakened = stage?.Player?.Species.MacroscopicType == MacroscopicSpeciesType.Awakened;
        interactAction.Visible = isAwakened;
        inventoryButton.Visible = isAwakened;
        buildButton.Visible = isAwakened;

        // TODO: figure out why this doesn't display correctly in the UI
        inventoryButton.ButtonPressed = IsInventoryOpen;
    }

    private void OnMoveToLandPressed()
    {
        GUICommon.Instance.PlayButtonPressSound();

        moveToLandPopup.PopupCenteredShrink();

        // TODO: make the cursor visible while this popup is open
    }

    private void OnMoveToLandConfirmed()
    {
        if (stage?.Player == null)
        {
            GD.Print("Player is missing to move to land");
            return;
        }

        GD.Print("Moving player to land");

        toLandButton.Disabled = true;

        EnsureGameIsUnpausedForEditor();

        // TODO: this is entirely placeholder feature
        TransitionManager.Instance.AddSequence(ScreenFade.FadeType.FadeOut, 0.3f, stage.TeleportToLand, false);
    }

    private void OnAwakenPressed()
    {
        GUICommon.Instance.PlayButtonPressSound();

        awakenConfirmPopup.PopupCenteredShrink();

        // TODO: make the cursor visible while this popup is open
    }

    private void UpdateAwakenButton(MacroscopicCreature player)
    {
        if (player.Species.MacroscopicType == MacroscopicSpeciesType.Awakened)
        {
            awakenButton.Visible = false;
            return;
        }

        float brainPower = player.Species.BrainPower;

        // TODO: require being ready to reproduce? Or do we want the player first to play as an awakened creature
        // before getting to the editor where they can still make some changes?

        // Doesn't matter as this is just for updating the GUI
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (lastBrainPower == brainPower)
            return;

        lastBrainPower = brainPower;

        var limit = Constants.BRAIN_POWER_REQUIRED_FOR_AWAKENING;

        awakenButton.Disabled = brainPower < limit;
        awakenButton.Text = Localization.Translate("ACTION_AWAKEN").FormatSafe(brainPower, limit);
    }

    private void OnAwakenConfirmed()
    {
        GUICommon.Instance.PlayButtonPressSound();

        stage!.MoveToAwakeningStage();
    }

    private void ForwardInteractButton()
    {
        EmitSignal(SignalName.OnInteractButtonPressed);
    }

    private void ForwardOpenInventory()
    {
        EmitSignal(SignalName.OnOpenInventoryPressed);
    }

    private void ForwardBuildPressed()
    {
        EmitSignal(SignalName.OnOpenBuildPressed);
    }
}
