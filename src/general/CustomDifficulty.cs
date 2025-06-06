﻿/// <summary>
///   Customised difficulty setting
/// </summary>
[CustomizedRegistryType]
public class CustomDifficulty : IDifficulty
{
    private bool applyGrowthOverride;
    private bool growthLimitOverride;

    private bool limitGrowthRate;

    public float MPMultiplier { get; set; }
    public float AIMutationMultiplier { get; set; }
    public float CompoundDensity { get; set; }
    public float PlayerDeathPopulationPenalty { get; set; }
    public float GlucoseDecay { get; set; }
    public float OsmoregulationMultiplier { get; set; }

    /// <summary>
    ///   The default value is picked here to preserve the old behaviour when loading older saves.
    /// </summary>
    public float PlayerAutoEvoStrength { get; set; } = 0.2f;

    /// <summary>
    ///   This defaults to 0 to keep this off in old saves
    /// </summary>
    public float PlayerSpeciesAIPopulationStrength { get; set; }

    public bool FreeGlucoseCloud { get; set; }

    public ReproductionCompoundHandling ReproductionCompounds { get; set; } =
        ReproductionCompoundHandling.TopUpOnPatchChange;

    public bool SwitchSpeciesOnExtinction { get; set; }

    public bool LimitGrowthRate
    {
        get
        {
            if (applyGrowthOverride)
                return growthLimitOverride;

            return limitGrowthRate;
        }
        set => limitGrowthRate = value;
    }

    public FogOfWarMode FogOfWarMode { get; set; }
    public bool OrganelleUnlocksEnabled { get; set; }

    public void SetGrowthRateLimitCheatOverride(bool newLimitSetting)
    {
        applyGrowthOverride = true;
        growthLimitOverride = newLimitSetting;
    }

    public void ClearGrowthRateLimitOverride()
    {
        applyGrowthOverride = false;
    }
}
