﻿using System;
using Godot;

/// <summary>
///   Microbe editor tutorial
/// </summary>
public partial class MicrobeEditorTutorialGUI : Control, ITutorialGUI
{
    [Export]
    public NodePath? EditorEntryReportPath;

    [Export]
    public NodePath PatchMapPath = null!;

    [Export]
    public NodePath CellEditorIntroductionPath = null!;

    [Export]
    public NodePath CellEditorUndoPath = null!;

    [Export]
    public NodePath CellEditorUndoHighlightPath = null!;

    [Export]
    public NodePath CellEditorRedoPath = null!;

    [Export]
    public NodePath CellEditorRedoHighlightPath = null!;

    [Export]
    public NodePath CellEditorClosingWordsPath = null!;

    [Export]
    public NodePath AutoEvoPredictionPath = null!;

    [Export]
    public NodePath AutoEvoPredictionHighlightPath = null!;

    [Export]
    public NodePath StaySmallTutorialPath = null!;

    [Export]
    public NodePath ChemoreceptorPlacementTutorialPath = null!;

    [Export]
    public NodePath NegativeAtpBalanceTutorialPath = null!;

    [Export]
    public NodePath MadeNoChangesTutorialPath = null!;

    [Export]
    public NodePath FlagellumPlacementTutorialPath = null!;

    [Export]
    public NodePath ModifyOrganelleTutorialPath = null!;

    [Export]
    public NodePath AtpBalanceIntroductionPath = null!;

    [Export]
    public NodePath AtpBalanceBarHighlightPath = null!;

#pragma warning disable CA2213
    private CustomWindow editorEntryReport = null!;
    private CustomWindow patchMap = null!;
    private CustomWindow cellEditorIntroduction = null!;
    private CustomWindow cellEditorUndo = null!;
    private CustomWindow cellEditorRedo = null!;
    private CustomWindow cellEditorClosingWords = null!;
    private CustomWindow autoEvoPrediction = null!;
    private CustomWindow staySmallTutorial = null!;
    private CustomWindow negativeAtpBalanceTutorial = null!;
    private CustomWindow chemoreceptorPlacementTutorial = null!;
    private CustomWindow madeNoChangesTutorial = null!;
    private CustomWindow flagellumPlacementTutorial = null!;
    private CustomWindow modifyOrganelleTutorial = null!;
    private CustomWindow atpBalanceIntroduction = null!;

    [Export]
    private CustomWindow nucleusTutorial = null!;

    [Export]
    private CustomWindow tolerancesTabTutorial = null!;

    [Export]
    private CustomWindow openTolerancesTabTutorial = null!;

    [Export]
    private CustomWindow earlyGameGoalTutorial = null!;

    [Export]
    private CustomWindow compoundBalanceTutorial = null!;

    [Export]
    private CustomWindow migrationTutorial = null!;

    [Export]
    private CustomWindow foodChainTutorial = null!;

    [Export]
    private CustomWindow digestionStatTutorial = null!;
#pragma warning restore CA2213

    public MainGameState AssociatedGameState => MainGameState.MicrobeEditor;
    public ITutorialInput? EventReceiver { get; set; }
    public bool IsClosingAutomatically { get; set; }
    public bool AllTutorialsDesiredState { get; private set; } = true;
    public Node GUINode => this;

    public ControlHighlight? CellEditorUndoHighlight { get; private set; }

    public ControlHighlight? CellEditorRedoHighlight { get; private set; }

    public ControlHighlight? AutoEvoPredictionHighlight { get; private set; }

    public ControlHighlight? AtpBalanceBarHighlight { get; private set; }

    [Export]
    public ControlHighlight? CompoundBalanceHighlight { get; private set; }

    /// <summary>
    ///   This is used to ensure the scroll position shows elements related to active tutorials
    /// </summary>
    public ScrollContainer RightPanelScrollContainer { get; set; } = null!;

    public bool EditorEntryReportVisible
    {
        get => editorEntryReport.Visible;
        set
        {
            if (value == editorEntryReport.Visible)
                return;

            if (value)
            {
                editorEntryReport.Show();
            }
            else
            {
                editorEntryReport.Hide();
            }
        }
    }

    public bool PatchMapVisible
    {
        get => patchMap.Visible;
        set
        {
            if (value == patchMap.Visible)
                return;

            if (value)
            {
                patchMap.Show();
            }
            else
            {
                patchMap.Hide();
            }
        }
    }

    public bool CellEditorIntroductionVisible
    {
        get => cellEditorIntroduction.Visible;
        set
        {
            if (value == cellEditorIntroduction.Visible)
                return;

            if (value)
            {
                cellEditorIntroduction.Show();
            }
            else
            {
                cellEditorIntroduction.Hide();
            }
        }
    }

    public bool CellEditorUndoVisible
    {
        get => cellEditorUndo.Visible;
        set
        {
            if (value == cellEditorUndo.Visible)
                return;

            if (value)
            {
                cellEditorUndo.Show();
            }
            else
            {
                cellEditorUndo.Hide();
            }
        }
    }

    public bool CellEditorRedoVisible
    {
        get => cellEditorRedo.Visible;
        set
        {
            if (value == cellEditorRedo.Visible)
                return;

            if (value)
            {
                cellEditorRedo.Show();
            }
            else
            {
                cellEditorRedo.Hide();
            }
        }
    }

    public bool CellEditorClosingWordsVisible
    {
        get => cellEditorClosingWords.Visible;
        set
        {
            if (value == cellEditorClosingWords.Visible)
                return;

            if (value)
            {
                cellEditorClosingWords.Show();
            }
            else
            {
                cellEditorClosingWords.Hide();
            }
        }
    }

    public bool AutoEvoPredictionVisible
    {
        get => autoEvoPrediction.Visible;
        set
        {
            if (value == autoEvoPrediction.Visible)
                return;

            if (value)
            {
                autoEvoPrediction.Show();
            }
            else
            {
                autoEvoPrediction.Hide();
            }
        }
    }

    public bool StaySmallTutorialVisible
    {
        get => staySmallTutorial.Visible;
        set
        {
            if (value == staySmallTutorial.Visible)
                return;

            if (value)
            {
                staySmallTutorial.Show();
            }
            else
            {
                staySmallTutorial.Hide();
            }
        }
    }

    public bool ChemoreceptorPlacementTutorialVisible
    {
        get => chemoreceptorPlacementTutorial.Visible;
        set
        {
            if (value == chemoreceptorPlacementTutorial.Visible)
                return;

            if (value)
            {
                chemoreceptorPlacementTutorial.Show();
            }
            else
            {
                chemoreceptorPlacementTutorial.Hide();
            }
        }
    }

    public bool NegativeAtpBalanceTutorialVisible
    {
        get => negativeAtpBalanceTutorial.Visible;
        set
        {
            if (value == negativeAtpBalanceTutorial.Visible)
                return;

            if (value)
            {
                negativeAtpBalanceTutorial.Show();
            }
            else
            {
                negativeAtpBalanceTutorial.Hide();
            }
        }
    }

    public bool MadeNoChangesTutorialVisible
    {
        get => madeNoChangesTutorial.Visible;
        set
        {
            if (value == madeNoChangesTutorial.Visible)
                return;

            if (value)
            {
                madeNoChangesTutorial.Show();
            }
            else
            {
                madeNoChangesTutorial.Hide();
            }
        }
    }

    public bool FlagellumPlacementTutorialVisible
    {
        get => flagellumPlacementTutorial.Visible;
        set
        {
            if (value == flagellumPlacementTutorial.Visible)
                return;

            if (value)
            {
                flagellumPlacementTutorial.Show();
            }
            else
            {
                flagellumPlacementTutorial.Hide();
            }
        }
    }

    public bool ModifyOrganelleTutorialVisible
    {
        get => modifyOrganelleTutorial.Visible;
        set
        {
            if (value == modifyOrganelleTutorial.Visible)
                return;

            if (value)
            {
                modifyOrganelleTutorial.Show();
            }
            else
            {
                modifyOrganelleTutorial.Hide();
            }
        }
    }

    public bool NucleusTutorialVisible
    {
        get => nucleusTutorial.Visible;
        set
        {
            if (value == nucleusTutorial.Visible)
                return;

            if (value)
            {
                nucleusTutorial.Show();
            }
            else
            {
                nucleusTutorial.Hide();
            }
        }
    }

    public bool AtpBalanceIntroductionVisible
    {
        get => atpBalanceIntroduction.Visible;
        set
        {
            if (value == atpBalanceIntroduction.Visible)
                return;

            if (value)
            {
                atpBalanceIntroduction.Show();
            }
            else
            {
                atpBalanceIntroduction.Hide();
            }
        }
    }

    public bool TolerancesTabTutorialVisible
    {
        get => tolerancesTabTutorial.Visible;
        set
        {
            if (value == tolerancesTabTutorial.Visible)
                return;

            if (value)
            {
                tolerancesTabTutorial.Show();
            }
            else
            {
                tolerancesTabTutorial.Hide();
            }
        }
    }

    public bool OpenTolerancesTabTutorialVisible
    {
        get => openTolerancesTabTutorial.Visible;
        set
        {
            if (value == openTolerancesTabTutorial.Visible)
                return;

            if (value)
            {
                openTolerancesTabTutorial.Show();
            }
            else
            {
                openTolerancesTabTutorial.Hide();
            }
        }
    }

    public bool EarlyGameGoalVisible
    {
        get => earlyGameGoalTutorial.Visible;
        set
        {
            if (value == earlyGameGoalTutorial.Visible)
                return;

            if (value)
            {
                earlyGameGoalTutorial.Show();
            }
            else
            {
                earlyGameGoalTutorial.Hide();
            }
        }
    }

    public bool CompoundBalanceTutorialVisible
    {
        get => compoundBalanceTutorial.Visible;
        set
        {
            if (value == compoundBalanceTutorial.Visible)
                return;

            if (value)
            {
                compoundBalanceTutorial.Show();
                RightPanelScrollContainer.ScrollVertical = 150;
            }
            else
            {
                compoundBalanceTutorial.Hide();
            }
        }
    }

    public bool MigrationTutorialVisible
    {
        get => migrationTutorial.Visible;
        set
        {
            if (value == migrationTutorial.Visible)
                return;

            if (value)
            {
                migrationTutorial.Show();
            }
            else
            {
                migrationTutorial.Hide();
            }
        }
    }

    public bool FoodChainTutorialVisible
    {
        get => foodChainTutorial.Visible;
        set
        {
            if (value == foodChainTutorial.Visible)
                return;

            if (value)
            {
                foodChainTutorial.Show();
            }
            else
            {
                foodChainTutorial.Hide();
            }
        }
    }

    public bool DigestionStatTutorialVisible
    {
        get => digestionStatTutorial.Visible;
        set
        {
            if (value == digestionStatTutorial.Visible)
                return;

            if (value)
            {
                digestionStatTutorial.Show();

                // This is right at the bottom now, so scroll surely down enough
                RightPanelScrollContainer.ScrollVertical = 800;
            }
            else
            {
                digestionStatTutorial.Hide();
            }
        }
    }

    public override void _Ready()
    {
        editorEntryReport = GetNode<CustomWindow>(EditorEntryReportPath);
        patchMap = GetNode<CustomWindow>(PatchMapPath);
        cellEditorIntroduction = GetNode<CustomWindow>(CellEditorIntroductionPath);
        cellEditorUndo = GetNode<CustomWindow>(CellEditorUndoPath);
        cellEditorRedo = GetNode<CustomWindow>(CellEditorRedoPath);
        cellEditorClosingWords = GetNode<CustomWindow>(CellEditorClosingWordsPath);
        autoEvoPrediction = GetNode<CustomWindow>(AutoEvoPredictionPath);
        staySmallTutorial = GetNode<CustomWindow>(StaySmallTutorialPath);
        chemoreceptorPlacementTutorial = GetNode<CustomWindow>(ChemoreceptorPlacementTutorialPath);
        negativeAtpBalanceTutorial = GetNode<CustomWindow>(NegativeAtpBalanceTutorialPath);
        madeNoChangesTutorial = GetNode<CustomWindow>(MadeNoChangesTutorialPath);
        flagellumPlacementTutorial = GetNode<CustomWindow>(FlagellumPlacementTutorialPath);
        modifyOrganelleTutorial = GetNode<CustomWindow>(ModifyOrganelleTutorialPath);
        atpBalanceIntroduction = GetNode<CustomWindow>(AtpBalanceIntroductionPath);

        CellEditorUndoHighlight = GetNode<ControlHighlight>(CellEditorUndoHighlightPath);
        CellEditorRedoHighlight = GetNode<ControlHighlight>(CellEditorRedoHighlightPath);
        AutoEvoPredictionHighlight = GetNode<ControlHighlight>(AutoEvoPredictionHighlightPath);
        AtpBalanceBarHighlight = GetNode<ControlHighlight>(AtpBalanceBarHighlightPath);

        ProcessMode = ProcessModeEnum.Always;
    }

    public override void _Process(double delta)
    {
        TutorialHelper.ProcessTutorialGUI(this, (float)delta);
    }

    public void OnClickedCloseAll()
    {
        TutorialHelper.HandleCloseAllForGUI(this);
    }

    public void OnSpecificCloseClicked(string closedThing)
    {
        TutorialHelper.HandleCloseSpecificForGUI(this, closedThing);
    }

    public void OnTutorialEnabledValueChanged(bool value)
    {
        AllTutorialsDesiredState = value;
    }

    public void HandleShowingATPBarHighlight()
    {
        if (AtpBalanceBarHighlight == null)
            throw new InvalidOperationException("Balance bar highlight control is not set");

        bool eitherVisible = atpBalanceIntroduction.Visible || negativeAtpBalanceTutorial.Visible;
        AtpBalanceBarHighlight.Visible = atpBalanceIntroduction.Visible;

        // With the stats now not being that tall, we can scroll all the way to the top to still see the whole ATP
        // bar
        if (eitherVisible)
            RightPanelScrollContainer.ScrollVertical = 0;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (EditorEntryReportPath != null)
            {
                EditorEntryReportPath.Dispose();
                PatchMapPath.Dispose();
                CellEditorIntroductionPath.Dispose();
                CellEditorUndoPath.Dispose();
                CellEditorUndoHighlightPath.Dispose();
                CellEditorRedoPath.Dispose();
                CellEditorRedoHighlightPath.Dispose();
                CellEditorClosingWordsPath.Dispose();
                AutoEvoPredictionPath.Dispose();
                AutoEvoPredictionHighlightPath.Dispose();
                StaySmallTutorialPath.Dispose();
                ChemoreceptorPlacementTutorialPath.Dispose();
                NegativeAtpBalanceTutorialPath.Dispose();
                MadeNoChangesTutorialPath.Dispose();
                FlagellumPlacementTutorialPath.Dispose();
                ModifyOrganelleTutorialPath.Dispose();
                AtpBalanceIntroductionPath.Dispose();
                AtpBalanceBarHighlightPath.Dispose();
            }
        }

        base.Dispose(disposing);
    }
}
