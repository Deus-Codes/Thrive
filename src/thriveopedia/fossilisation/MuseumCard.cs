﻿using Godot;

/// <summary>
///   Card displaying a fossilised species in the Thriveopedia museum.
/// </summary>
public partial class MuseumCard : Button
{
    private readonly NodePath modulateReference = new("modulate");

#pragma warning disable CA2213
    [Export]
    private Label? speciesNameLabel;

    [Export]
    private TextureRect? speciesPreview;

    [Export]
    private TextureButton deleteButton = null!;

    // TODO: check if this should be disposed
    private Image? fossilPreviewImage;

#pragma warning restore CA2213

    private Color defaultDeleteModulate;

    private Species? savedSpecies;

    [Signal]
    public delegate void OnSpeciesSelectedEventHandler(MuseumCard card);

    [Signal]
    public delegate void OnSpeciesDeletedEventHandler(MuseumCard card);

    /// <summary>
    ///   The fossilised species associated with this card.
    /// </summary>
    public Species? SavedSpecies
    {
        get => savedSpecies;
        set
        {
            savedSpecies = value;
            UpdateSpeciesName();
        }
    }

    /// <summary>
    ///   If the fossil file had a preview image, that is set here and is used used to preview the species
    /// </summary>
    public Image? FossilPreviewImage
    {
        get => fossilPreviewImage;
        set
        {
            if (value == fossilPreviewImage)
                return;

            fossilPreviewImage = value;
            UpdatePreviewImage();
        }
    }

    public string? FossilName { get; set; }

    public override void _Ready()
    {
        base._Ready();

        defaultDeleteModulate = deleteButton.SelfModulate;

        UpdateSpeciesName();
        UpdatePreviewImage();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            modulateReference.Dispose();
        }

        base.Dispose(disposing);
    }

    private void UpdateSpeciesName()
    {
        if (SavedSpecies == null || speciesNameLabel == null)
            return;

        speciesNameLabel.Text = SavedSpecies.FormattedName;
    }

    private void UpdatePreviewImage()
    {
        if (speciesPreview == null)
            return;

        if (FossilPreviewImage != null)
        {
            // Could add filter and mipmap flags here if the preview images look too bad at small sizes, but that
            // would presumably make this take more time, so maybe then this shouldn't be done in a blocking way here
            // and instead using ResourceManager
            var imageTexture = ImageTexture.CreateFromImage(FossilPreviewImage);

            speciesPreview.Texture = imageTexture;
        }
    }

    private void OnPressed()
    {
        // TODO: it's slightly non-optimal that this triggers first and then OnDeletePressed when pressing on the
        // delete button. Could maybe queue invoke the species select and skip that if the delete got pressed?

        GUICommon.Instance.PlayButtonPressSound();
        EmitSignal(SignalName.OnSpeciesSelected, this);
    }

    private void OnMouseEnter()
    {
        var tween = CreateTween();
        tween.TweenProperty(speciesPreview, modulateReference, Colors.Gray, 0.5);
    }

    private void OnMouseExit()
    {
        var tween = CreateTween();
        tween.TweenProperty(speciesPreview, modulateReference, Colors.White, 0.5);
    }

    private void OnDeletePressed()
    {
        GUICommon.Instance.PlayButtonPressSound();

        EmitSignal(SignalName.OnSpeciesDeleted, this);
    }

    private void OnDeleteMouseEntered()
    {
        // TODO: unify the approach here with CustomWindow or create variants of the image as another way to do this
        // properly (reason why that isn't done is due to this issue:
        // https://github.com/Revolutionary-Games/Thrive/issues/1581
        deleteButton.SelfModulate = new Color(0.4f, 0.4f, 0.4f);
    }

    private void OnDeleteMouseExited()
    {
        deleteButton.SelfModulate = defaultDeleteModulate;
    }
}
